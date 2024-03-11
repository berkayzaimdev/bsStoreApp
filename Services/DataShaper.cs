using AutoMapper.Internal;
using Entities.Models;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DataShaper<T> : IDataShaper<T>
        where T : class
    {
        public PropertyInfo[] Properties { get; set; } // Book üzerinde çalışıyorsak Id, Title, Price ifadelerini kapsar

        public DataShaper()
        {
            Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            // Nesne yönelimli programlamada yapılan hatalardan biri; referans tipli parametre kullanıyorsak ya tanımlandığı yerde ya da constructor'da atama yapılmalıdır
        }

        public IEnumerable<ShapedEntity> ShapeData(IEnumerable<T> entities, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredProperties);
        }

        public ShapedEntity ShapeData(T entity, string fieldsString)
        {
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchData(entity, requiredProperties);
        }

        private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
        {
            // books?fields=id, title
            // gibi bir ifade dönerse buradan id ve title'ı çekmek için private metod yazdık
            var requiredFields = new List<PropertyInfo>();

            if(!string.IsNullOrWhiteSpace(fieldsString))
            {
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries);

                foreach(var field in fields)
                {
                    var property = Properties.FirstOrDefault(pi => pi.Name.Equals(field.Trim(), StringComparison.InvariantCultureIgnoreCase));

                    if (property is null)
                        continue;

                    requiredFields.Add(property);
                }

            }
            else 
            {
                requiredFields = Properties.ToList();
            }

            return requiredFields;
        }

        private ShapedEntity FetchData(T entity, 
            IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ShapedEntity();

            foreach(var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
            }

            var objectProperty = entity.GetType().GetProperty("Id"); // Ortak alan olan ID'yi çözmek için manuel bir metod uyguladık
            shapedObject.Id = (int)objectProperty.GetValue(entity);

            return shapedObject;
        }

        private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities,
            IEnumerable<PropertyInfo> requiredProperties)
        {
            var shapedData = new List<ShapedEntity>();
            
            foreach(var entity in entities)
            {
                var shapedObject = FetchData(entity, requiredProperties);
                shapedData.Add(shapedObject);
            }

            return shapedData;
        }
    }
}
