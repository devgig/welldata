using System;
using System.Linq;

namespace WellData.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool ImplementsInterfaceTemplate(this Type type, Type openGeneric)
        {
            return
                type.GetInterfaces().Any(
                    x => x.IsGenericType
                    && x.GetGenericTypeDefinition() == openGeneric);
        }

        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }

    }
}
