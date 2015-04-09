using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Common
{
    public class PropertyCompare<T> where T : class
    {
        public CompareReport Compare(T obj1, T obj2)
        {
            CompareReport report = new CompareReport();

            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    object firstValue = propertyInfo.GetValue(obj1, null);
                    object secondValue = propertyInfo.GetValue(obj2, null);

                    if (!object.Equals(firstValue, secondValue))
                    {
                        object[] changedValues = new object[2];

                        changedValues[0] = firstValue;
                        changedValues[1] = secondValue;

                        report.PropertiesChanged.Add(propertyInfo.Name, changedValues);
                    }
                }
            }

            return report;
        }
    }
}
