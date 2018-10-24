using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace SeaStore.FilterQuery
{
    public static class QueryableExtensions
    {
        public static IMemoryCache Cache { get; set; }

        internal static string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }


        /// <summary>
        /// Applies data processing (paging, sorting, filtering and aggregates) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the DataSource.</param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <param name="aggregates">Specifies the current aggregates.</param>
        /// <param name="cacheTotal">Flag to specify if the now totla should be cahced.</param>
        /// <param name="resetCache">Flag to specify if the existing cached total should be removed and recalculated. Note: <paramref name="cacheTotal"/>  must be set to true in order for reset to be applied.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>
        public static FilteredResult GetFilteredResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, Filter filter, IEnumerable<Aggregator> aggregates, bool cacheTotal = false, bool resetCache = false)
        {
            // Filter the data first
            queryable = Filter(queryable, filter);

            var key = GetStringSha256Hash(queryable.ToString());
            int total;
            // var cache = MemoryCache.Default;

            if (cacheTotal)
            {
                if (resetCache)
                {
                    Cache.Remove(key);
                }

                var totalCached = Cache.Get<int?>(key);
                if (totalCached.HasValue)
                {
                    total = totalCached.Value;
                }
                else
                {
                    // Calculate the total number of records (needed for paging)
                    total = queryable.Count();

                    Cache.Set(key, total, DateTimeOffset.Now.AddDays(1));
                }
            }
            else
            {
                total = queryable.Count();
            }

            // Calculate the aggregates
            var aggregate = Aggregate(queryable, aggregates);

            // Sort the data
            // (CHRIS) TEMP SHORT CIRCUIT
            queryable = Sort(queryable, sort);

            // Finally page the data
            if (take > 0)
            {
                queryable = Page(queryable, take, skip);
            }

            var data = queryable.ToList();
            foreach (var item in data)
            {
                var dates = item.GetType().GetProperties().Where(cv => cv.PropertyType == typeof(DateTime) || cv.PropertyType == typeof(DateTime?));
                foreach (var date in dates)
                {
                    var currdate = (DateTime?)date.GetValue(item, null);
                    if (currdate == null) continue;
                    currdate = DateTime.SpecifyKind(currdate.Value, DateTimeKind.Local);
                    //date.SetValue(item, date.PropertyType == typeof(DateTime?) ? currdate : currdate.Value, null);
                }
            }
            return new FilteredResult
            {
                Data = data,
                Total = total,
                Aggregates = aggregate
            };
        }

        /// <summary>
        /// Applies data processing (paging, sorting and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="take">Specifies how many items to take. Configurable via the pageSize setting of the DataSource.</param>
        /// <param name="skip">Specifies how many items to skip.</param>
        /// <param name="sort">Specifies the current sort order.</param>
        /// <param name="filter">Specifies the current filter.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>
        public static FilteredResult GetFilteredResult<T>(this IQueryable<T> queryable, int take, int skip, IEnumerable<Sort> sort, Filter filter)
        {
            return queryable.GetFilteredResult(take, skip, sort, filter, null);
        }

        /// <summary>
        ///  Applies data processing (paging, sorting and filtering) over IQueryable using Dynamic Linq.
        /// </summary>
        /// <typeparam name="T">The type of the IQueryable.</typeparam>
        /// <param name="queryable">The IQueryable which should be processed.</param>
        /// <param name="request">The DataSourceRequest object containing take, skip, order, and filter data.</param>
        /// <returns>A DataSourceResult object populated from the processed IQueryable.</returns>
	    public static FilteredResult GetFilteredResult<T>(this IQueryable<T> queryable, FilterRequest request)
        {
            return queryable.GetFilteredResult(request.Take, request.Skip, request.Sort, request.Filter, null, request.CacheTotal, request.ResetCache);
        }

        //map - temporary solution. Needs to be generated from Automapper fields.
        public static FilteredResult GetFilteredResult<T>(this IQueryable<T> queryable, FilterRequest request, IDictionary<string, string> map = null)
        {
            if (map == null) map = new Dictionary<string, string>();
            request.Sort.Map(map);
            request.Filter.Map(map);
            return GetFilteredResult(queryable, request);
        }

        /* JM (7/25/2018):  removing this so I can get rid of System.Net.Http.Formatting.Extension (not compatible with .Net Core) */
        //public static FilterRequest GetRequest(this HttpRequestMessage message)
        //{
        //    return JsonConvert.DeserializeObject<FilterRequest>(message.RequestUri.ParseQueryString().GetKey(0));
        //}

        public static FilterRequest GetRequest(this HttpRequest request)
        {
            return JsonConvert.DeserializeObject<FilterRequest>(request.Query.Keys.First(cv => cv != "ViewTypeId").Replace("%22", "\"").Replace("%20", " "));
        }

        public static T GetRequest<T>(this HttpRequest request) where T : FilterRequest
        {
            return JsonConvert.DeserializeObject<T>(request.Query.Keys.First(cv => cv != "ViewTypeId").Replace("%22", "\"").Replace("%20", " "));
        }

        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, Filter filter)
        {
            // (CHRIS) TEMP SHORT CIRCUIT
            //return queryable;

            if (filter != null && filter.Logic != null)
            {
                // Collect a flat list of all filters
                if (filter.Filters.Any())
                {
                    var filters = filter.All();
                    // Get all filter values as array (needed by the Where method of Dynamic Linq)
                    var values = filters.Select(f => f.Value).ToArray();

                    // Create a predicate expression e.g. Field1 = @0 And Field2 > @1
                    string predicate = filter.ToExpression(filters);

                    // Use the Where method of Dynamic Linq to filter the data
                    queryable = DynamicQueryable.Where(queryable, predicate, values);
                }

            }

            return queryable;
        }

        private static object Aggregate<T>(IQueryable<T> queryable, IEnumerable<Aggregator> aggregates)
        {
            if (aggregates != null && aggregates.Any())
            {
                var objProps = new Dictionary<DynamicProperty, object>();
                var groups = aggregates.GroupBy(g => g.Field);
                Type type = null;
                foreach (var group in groups)
                {
                    var fieldProps = new Dictionary<DynamicProperty, object>();
                    foreach (var aggregate in group)
                    {
                        var prop = typeof(T).GetProperty(aggregate.Field);
                        var param = Expression.Parameter(typeof(T), "s");
                        var selector = aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                            ? Expression.Lambda(Expression.NotEqual(Expression.MakeMemberAccess(param, prop), Expression.Constant(null, prop.PropertyType)), param)
                            : Expression.Lambda(Expression.MakeMemberAccess(param, prop), param);
                        var mi = aggregate.MethodInfo(typeof(T));
                        if (mi == null)
                            continue;

                        var val = queryable.Provider.Execute(Expression.Call(null, mi,
                            aggregate.Aggregate == "count" && (Nullable.GetUnderlyingType(prop.PropertyType) == null)
                                ? new[] { queryable.Expression }
                                : new[] { queryable.Expression, Expression.Quote(selector) }));

                        fieldProps.Add(new DynamicProperty(aggregate.Aggregate, typeof(object)), val);
                    }
                    type = DynamicExpression.CreateClass(fieldProps.Keys);
                    var fieldObj = Activator.CreateInstance(type);
                    foreach (var p in fieldProps.Keys)
                        type.GetProperty(p.Name).SetValue(fieldObj, fieldProps[p], null);
                    objProps.Add(new DynamicProperty(group.Key, fieldObj.GetType()), fieldObj);
                }

                type = DynamicExpression.CreateClass(objProps.Keys);

                var obj = Activator.CreateInstance(type);

                foreach (var p in objProps.Keys)
                {
                    type.GetProperty(p.Name).SetValue(obj, objProps[p], null);
                }

                return obj;
            }
            else
            {
                return null;
            }
        }

        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IEnumerable<Sort> sort)
        {
            if (sort != null && sort.Any())
            {
                // Create ordering expression e.g. Field1 asc, Field2 desc
                var ordering = String.Join(",", sort.Select(s => s.ToExpression()));

                // Use the OrderBy method of Dynamic Linq to sort the data
                return queryable.OrderBy(ordering);
            }

            return queryable;
        }

        public static IQueryable<T> ConditionalQueryableFilterAdjustment<T>(this IQueryable<T> queryable, FilterRequest request, string requestItem, Expression<Func<T, bool>> predicate)
        {
            var requestItems = request.SecurityFilter?.Filters?.Where(f => f.Filters != null)
                .SelectMany(cv => cv.Filters?.Where(x => x != null && x.Field == requestItem).Select(a => a.Value)).ToList();

            //need to strip out the unit & facility filters from request object to query separately
            if (request.Filter?.Filters != null)
            {
                foreach (var filter in request.Filter.Filters)
                {
                    if (filter.Filters == null)
                    {
                        if (filter.Field == requestItem)
                        {
                            filter.Field = null;
                        }
                        continue;
                    }

                    var newFilters = filter.Filters
                        .Where(innerFilter => innerFilter.Field != null && innerFilter.Field != requestItem)
                        .ToList();

                    filter.Filters = newFilters.Count > 0 ? newFilters : null;
                }

                request.Filter.Filters =
                    request.Filter.Filters.Where(f => f.Field != null || (f.Field == null && f.Filters != null));
            }

            var query = queryable;

            if (requestItems != null && requestItems.Any())
                query = queryable.Where(predicate);

            return query;
        }

        private static IQueryable<T> Page<T>(IQueryable<T> queryable, int take, int skip)
        {
            return queryable.Skip(skip).Take(take);
        }

        private static Filter Map(this Filter filter, IDictionary<string, string> map)
        {
            if (filter == null) return filter;
            if (filter.Field != null && map.ContainsKey(filter.Field))
            {
                if (map[filter.Field].Contains("||") || map[filter.Field].Contains("&&"))
                {
                    var splitter = map[filter.Field].Contains("||") ? "||" : "&&";
                    var innerFilters = map[filter.Field].Split(new string[] { splitter }, StringSplitOptions.None)
                        .Select(cv => cv.Trim())
                        .Where(cv => !string.IsNullOrWhiteSpace(cv))
                        .Select(cv => Map(new Filter
                        {
                            Field = filter.Field,
                            Operator = filter.Operator,
                            Value = filter.Value
                        },
                        new Dictionary<string, string> { { filter.Field, cv } }));
                    return new Filter
                    {
                        Filters = innerFilters.ToArray(),
                        Logic = splitter == "||" ? "or" : "and"
                    };
                }
                else if (map[filter.Field].Contains("|"))
                {
                    var expression = map[filter.Field].Split('|');
                    if (expression.Count() == 2)
                    {
                        filter.Operator = expression[1].Trim();
                        filter.Field = expression[0].Trim();
                    }
                }
                else filter.Field = map[filter.Field];
            }
            if (filter.Filters != null && filter.Filters.Any())
                filter.Filters = filter.Filters.Select(cv => Map(cv, map)).ToArray();
            return filter;
        }
        private static void Map(this IEnumerable<Sort> sort, IDictionary<string, string> map)
        {
            sort.ToList().ForEach(cv =>
            {
                if (map.ContainsKey(cv.Field))
                    cv.Field = map[cv.Field];
            });
        }
    }
}
