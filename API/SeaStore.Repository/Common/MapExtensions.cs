using AutoMapper;
using SeaStore.FilterQuery;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SeaStore.Repository.Common
{
  public static class MapExtensions
  {
    private static readonly MethodInfo _listMethod = typeof(Enumerable).GetMethod("ToList");

    public static T MapTo<T>(this object source)
    {
      if (source == null) return default(T);
      return Mapper.Map<T>(source);
    }

    public static T MapTo<T>(this IQueryable source)
    {
      if (source == null) return default(T);
      var genericToList = _listMethod.MakeGenericMethod(new Type[] { source.ElementType });
      var list = (IList)genericToList.Invoke(null, new[] { source });
      return Mapper.Map<T>(list);
    }

    public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
    {
      return Mapper.Map(source, destination);
    }

    public static void MapRangeTo<TSource, TDestination>(this IEnumerable<TSource> sourceList, ICollection<TDestination> destinationList, Func<TSource, TDestination, bool> keyMap)
    {
      destinationList.Where(dest => sourceList.All(source => !keyMap(source, dest))).ToList()
                      .ForEach(cv => destinationList.Remove(cv));
      sourceList.ToList().ForEach(source =>
      {
        var destination = destinationList.FirstOrDefault(dest => keyMap(source, dest));
        if (destination == null) destinationList.Add(source.MapTo<TDestination>());
        else destination = source.MapTo(destination);
      });
    }

    public static Expression<Func<T, bool>> MapTo<F, T>(this Expression<Func<F, bool>> predicate)
    {
      if (predicate == null) return null;
      return ExpressionRewriter.CastParam<F, T>(predicate);
    }

    public static FilteredResult MapTo<T>(this FilteredResult result)
    {
      if (result == null) return null;
      result.Data = result.Data.MapTo<IEnumerable<T>>();
      return result;
    }

    public static IEnumerable<T> MapToEnumerable<T>(this IEnumerable result)
    {
      return result?.MapTo<IEnumerable<T>>();
    }

    public static IList<T> MapToList<T>(this IList result)
    {
      return result?.MapTo<IList<T>>();
    }
  }
}
