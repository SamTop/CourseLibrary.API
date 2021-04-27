using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseLibrary.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id", new PropertyMappingValue(new List<string>() {"Id"} ) },
                { "MainCategory", new PropertyMappingValue(new List<string>() {"MainCategory"} ) },
                { "Age", new PropertyMappingValue(new List<string>() {"DateOfBirth"}, true ) },
                { "Name", new PropertyMappingValue(new List<string>() {"FirstName", "LastName"} ) }
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
        }
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping
            <TSource, TDestination>()
        {
            // get matching mapping
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
            {
                return matchingMapping.First()._mappingDictionary;
            }

            throw new Exception($"Cannot find exact property mapping instance " +
                $"for <{typeof(TSource)}, {typeof(TDestination)}>");
        }

        public bool ValidMappingExistsFor<TSource, TDestination>(string orderBy) {
            Dictionary<string, PropertyMappingValue> propertyMapping = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(orderBy))
                return true;

            // list of all orderby clauses
            var orderByAfterSplit = orderBy.Split(",");

            foreach(var orderByClause in orderByAfterSplit)
            {
                var orderByTrimmed = orderByClause.Trim();

                var firstSpace = orderByTrimmed.IndexOf(" ");
                var orderByKey = firstSpace == -1 ?
                    orderByTrimmed : orderByTrimmed.Remove(firstSpace);

                if(!propertyMapping.ContainsKey(orderByKey))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
