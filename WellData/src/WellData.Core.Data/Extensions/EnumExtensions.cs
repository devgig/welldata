using System;
using System.ComponentModel;
using System.Linq;

namespace WellData.Core.Data.Extensions
{
    public static class EnumExtensions
    {
        public static string Description(this Enum eValue)
        {
            var nAttributes = eValue.GetType().GetField(eValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (!nAttributes.Any())
                return eValue.ToString();

            var descriptionAttribute = nAttributes.First() as DescriptionAttribute;
            if (descriptionAttribute != null)
            {
                return descriptionAttribute.Description;
            }
            return null;
        }
    }
}
