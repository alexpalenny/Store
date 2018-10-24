using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Linq.Dynamic;
using System.Text;

namespace SeaStore.FilterQuery
{
    /// <summary>
    /// Represents a filter expression of DataSource.
    /// </summary>
    [DataContract]
    public class Filter
    {
        /// <summary>
        /// Gets or sets the name of the sorted field (property). Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the filtering operator. Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        [DataMember(Name = "operator")]
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets the filtering value. Set to <c>null</c> if the <c>Filters</c> property is set.
        /// </summary>
        [DataMember(Name = "value")]
        public object Value { get; set; }

        /// <summary>
        /// Gets or sets the filtering logic. Can be set to "or" or "and". Set to <c>null</c> unless <c>Filters</c> is set.
        /// </summary>
        [DataMember(Name = "logic")]
        public string Logic { get; set; }

        /// <summary>
        /// Gets or sets the child filter expressions. Set to <c>null</c> if there are no child expressions.
        /// </summary>
        [DataMember(Name = "filters")]
        public IEnumerable<Filter> Filters { get; set; }

        /// <summary>
        /// Mapping of DataSource filtering operators to Dynamic Linq
        /// </summary>
        private static readonly IDictionary<string, string> operators = new Dictionary<string, string>
        {
            {"eq", "="},
            {"neq", "!="},
            {"lt", "<"},
            {"lte", "<="},
            {"gt", ">"},
            {"gte", ">="},
            {"startswith", "StartsWith"},
            {"endswith", "EndsWith"},
            {"contains", "Contains"},
            {"doesnotcontain", "Contains"},
            {"in", "in"},
            {"any", "any"},
            {"all", "all"}
        };

        /// <summary>
        /// Get a flattened list of all child filter expressions.
        /// </summary>
        public IList<Filter> All()
        {
            var filters = new List<Filter>();

            Collect(filters);

            return filters;
        }

        private void Collect(IList<Filter> filters)
        {
            if (Filters != null && Filters.Any())
            {
                foreach (Filter filter in Filters)
                {
                    filters.Add(filter);

                    filter.Collect(filters);
                }
            }
            else
            {
                filters.Add(this);
            }
        }

        /// <summary>
        /// Converts the filter expression to a predicate suitable for Dynamic Linq e.g. "Field1 = @1 and Field2.Contains(@2)"
        /// </summary>
        /// <param name="filters">A list of flattened filters.</param>
        public string ToExpression(IList<Filter> filters)
        {

            if (Filters != null && Filters.Any())
            {
                return "(" + string.Join(" " + Logic + " ", Filters.Select(filter => filter.ToExpression(filters)).ToArray()) + ")";
            }

            int index = filters.IndexOf(this);

            string comparison = operators[Operator];

            if (Operator == "doesnotcontain")
            {
                return string.Format("!{0}.{1}(@{2})", Field, comparison, index);
            }
            if (comparison == "StartsWith" || comparison == "EndsWith" || comparison == "Contains")
            {
                return string.Format("{0} != null AND {0}.ToLower().{1}((@{2}).ToLower())", Field, comparison, index);
            }
            if (comparison == "=" && Value is DateTime)
            {
                return string.Format("({0}) != null AND DateTime({0}) == DateTime(@{1})", Field, index);
            }
            if ((comparison == "lte" || comparison == "<=") && Value is DateTime)
            {
                return string.Format("({0}) != null AND DateTime({0}).Date <= DateTime(@{1}).Date", Field, index);
            }
            if ((comparison == "gte" || comparison == ">=") && Value is DateTime)
            {
                return string.Format("({0}) != null AND DateTime({0}).Date >= DateTime(@{1}).Date", Field, index);
            }
            if (comparison == "=" && Value is double)
            {
                return string.Format("({0}) != null AND ((@{1}) + Double(0.01)) > Double({0}) AND ((@{1}) - Double(0.01)) < Double({0})", Field, index);
            }
            if (comparison == "in" && Value is IEnumerable)
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("(");
                bool second = false;
                foreach (var value in Value as IEnumerable)
                {
                    if (second)
                        stringBuilder.AppendFormat(" OR ");
                    second = true;
                    stringBuilder.AppendFormat("{0} == {1}", Field, value);
                }

                stringBuilder.Append(")");
                return stringBuilder.ToString();
                // return string.Format("({0}) != null AND {0}.ToString().ToLower().Contains(@{1})", Field, index);
            }
            if (comparison == "any")
            {
                var properties = Field.Split(':');
                if (properties != null && properties.Count() == 2)
                {
                    var propertyList = properties[0];
                    var property = properties[1];
                    return string.Format("{0}.any({1} = ((@{2})))", propertyList, property, index);
                }
            }
            if (comparison == "all")
            {
                var properties = Field.Split(':');
                if (properties != null && properties.Count() == 2)
                {
                    var propertyList = properties[0];
                    var property = properties[1];
                    return string.Format("{0}.all({1} = ((@{2})))", propertyList, property, index);
                }
            }

            return string.Format("{0} {1} @{2}", Field, comparison, index);
        }
    }
}
