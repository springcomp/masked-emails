using System;
using System.Linq;
using Data.Model;

namespace WebApi.Services
{
    internal static class QueryableExtensions
    { 
        public static IQueryable<Address> OrderBy(this IQueryable<Address> collection, string sort_by, bool descending)
        { 
            if (sort_by == "address")
            {
                collection = descending
                        ? collection.OrderByDescending(a => a.EmailAddress)
                        : collection.OrderBy(a => a.EmailAddress)
                        ;
            }
            else if (sort_by == "created-utc")
            {
                collection = descending
                        ? collection.OrderByDescending(a => a.CreatedUtc)
                        : collection.OrderBy(a => a.CreatedUtc)
                        ;
            }
            else if (sort_by == "name")
            {
                collection = descending
                        ? collection.OrderByDescending(a => a.Name)
                        : collection.OrderBy(a => a.Name)
                        ;
            }
            else if (sort_by == "description")
            {
                collection = descending
                        ? collection.OrderByDescending(a => a.Description)
                        : collection.OrderBy(a => a.Name)
                        ;
            }
            else
            {
                throw new ArgumentException($"Unsupported sort criteria '{sort_by}'.");
            }

            return collection;
        }
    }
}