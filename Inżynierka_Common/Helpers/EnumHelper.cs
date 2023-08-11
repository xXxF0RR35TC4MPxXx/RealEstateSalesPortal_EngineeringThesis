using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka_Common.Helpers
{
    public class EnumHelper
    {
        public static string GetDescriptionFromEnum(Enum? value)
        {
            if (value == null)
            {
                return "";
            }

            try
            {
                DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
                return attribute == null ? value.ToString() : attribute.Description;
            }
            catch(NullReferenceException e)
            {
                return "";
            }
            
        }

        public static T GetEnumFromDescription<T>(string? description) where T : Enum
        {
            if(description == null || description == "") 
            {  
                return default(T);
            }
            foreach (var field in typeof(T).GetFields())
            {
                if (Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
            // Or return default(T);
        }
    }
}
