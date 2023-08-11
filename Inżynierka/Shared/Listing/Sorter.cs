using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Inżynierka_Services.Listing
{
    public class Sorter<TEntity>
    {
        public static IQueryable<TEntity> Sort(IQueryable<TEntity> entities, List<KeyValuePair<string, string>> sort)
        {
            bool IsFirst = true;
            for (int i = 0; i < sort.Count; i++)
            {
                if (!String.IsNullOrEmpty(sort[i].Key))
                {
                    PropertyInfo? prop = typeof(TEntity).GetProperty(sort[i].Key);
                    if (prop != null)
                    {
                        if (IsFirst)
                        {
                            entities = OrderBy(entities, sort[i]);
                            IsFirst = false;
                        }
                        else
                        {
                            entities = ThenBy((IOrderedQueryable<TEntity>)entities, sort[i]);
                        }
                    }
                }
            }

            return entities;
        }

        private static IQueryable<TEntity> OrderBy(IQueryable<TEntity> entities, KeyValuePair<string, string> sort)
        {
            if (sort.Value.ToUpper() == "DESC")
            {
                entities = entities.OrderByDescending(e => EF.Property<object>(e, sort.Key));
            }
            else
            {
                entities = entities.OrderBy(e => EF.Property<object>(e, sort.Key));
            }

            return entities;
        }

        private static IQueryable<TEntity> ThenBy(IOrderedQueryable<TEntity> entities, KeyValuePair<string, string> sort)
        {
            if (sort.Value.ToUpper() == "DESC")
            {
                entities = entities.ThenByDescending(e => EF.Property<object>(e, sort.Key));
            }
            else
            {
                entities = entities.ThenBy(e => EF.Property<object>(e, sort.Key));
            }

            return entities;
        }
    }
}