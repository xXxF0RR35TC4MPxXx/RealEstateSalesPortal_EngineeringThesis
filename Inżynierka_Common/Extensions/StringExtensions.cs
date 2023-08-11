using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Inżynierka_Common.Extensions
{
    public static class StringExtensions
    {
        public static string PadBoth(this string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft, '.').PadRight(length, '.');
        }

        public static string GetDescription<T>(string prop) where T : new()
        {
            var property = typeof(T).GetProperty(prop);
            var attribute = property.GetCustomAttributes(typeof(DescriptionAttribute), true)[0];
            var description = (DescriptionAttribute)attribute;
            var text = description.Description;
            return text;
        }

        //public static string GetDescription<T>(this string propertyName) where T : class
        //{
        //    MemberInfo memberInfo = typeof(T).GetProperty(propertyName);
        //    if (memberInfo == null)
        //    {
        //        return null;
        //    }

        //    return memberInfo.GetCustomAttribute<DisplayAttribute>()?.GetDescription();
        //}
    }
}
