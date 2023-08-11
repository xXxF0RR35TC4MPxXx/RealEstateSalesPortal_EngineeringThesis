using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inżynierka.Client.Handlers
{
    public class DescriptionHelper
    {
        public static List<string> GetAttributeDescription<T>(string propName)
        {
            List<string> result = new();

            var prop = typeof(T).GetProperty(propName);

            var att = prop?.GetCustomAttributes(typeof(DescriptionAttribute), true);
            
            if (att?.Length > 0)
            {

                result.Add(((DescriptionAttribute)att[0]).Description);
            }
            return result;
        }
    }
}
