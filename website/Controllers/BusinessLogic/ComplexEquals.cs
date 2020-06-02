using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;

namespace website.Controllers.BusinessLogic
{
    public static class ComplexEquals
    {
        //Snagged off the web. Pretty nice bit of code for recursive property checking of any two complex objects But i need a bit in here...


        public static object CompareEquals<T>(this T objectFromCompare, T objectToCompare)
        {
            if (objectFromCompare == null && objectToCompare == null)
                return true;

            else if (objectFromCompare == null && objectToCompare != null)
                return false;

            else if (objectFromCompare != null && objectToCompare == null)
                return false;

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {

                //custom bit for Entity Framework - Bypass ID fields - otherwise these are Always going to be different.  and that means this method
                // will never return true.

                if (prop.Name.ToLower() == "id")
                {
                    continue;
                }

                // bypass the ICollection fields because those are key markers that are not needed to compare in this case
                // again - Entity Framework thing. The entity framework object from the database won't have them either, which throws null errors later.

                Type collectionType = typeof(ICollection<>);
                Type propType = prop.PropertyType;

                if (propType.IsGenericType && collectionType.IsAssignableFrom(propType.GetGenericTypeDefinition()) ||
                        propType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == collectionType))
                {
                    continue;
                }

                object dataFromCompare =
                objectFromCompare.GetType().GetProperty(prop.Name).GetValue(objectFromCompare, null);

                object dataToCompare =
                objectToCompare.GetType().GetProperty(prop.Name).GetValue(objectToCompare, null);

                Type type =
                objectFromCompare.GetType().GetProperty(prop.Name).GetValue(objectToCompare, null).GetType();



                if (prop.PropertyType.IsClass &&
                !prop.PropertyType.FullName.Contains("System.String"))
                {
                    dynamic convertedFromValue = Convert.ChangeType(dataFromCompare, type);
                    dynamic convertedToValue = Convert.ChangeType(dataToCompare, type);

                    object result = ComplexEquals.CompareEquals(convertedFromValue, convertedToValue);

                    bool compareResult = (bool)result;
                    if (!compareResult)
                        return false;
                }

                else if (!dataFromCompare.Equals(dataToCompare))
                    return false;
            }

            return true;
        }
    }
}