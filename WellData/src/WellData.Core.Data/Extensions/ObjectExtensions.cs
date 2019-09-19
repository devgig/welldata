using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace WellData.Core.Data.Extensions
{
    public static class ObjectExtensions
    {

        public static int ToNumber(this object value)
        {
            return int.TryParse(value?.ToString(), out int n) ? n : 0;
        }

        public static double ToDouble(this object value)
        {
            return double.TryParse(value?.ToString(), out double n) ? n : 0;
        }
        
        public static decimal ToDecimal(this object value)
        {
            return decimal.TryParse(value?.ToString(), NumberStyles.AllowCurrencySymbol | NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, out decimal n) ? n : 0.00M;
        }

        public static void IfNotNullThen<T>(this T o, Action<T> action) where T : class
        {
            if (o.IsNotNull())
                action.Invoke(o);
        }

        public static bool IsInList<T>(this T o, params T[] args)
        {
            return args.Any(x => o.Equals(x));
        }

        public static R IfNotNull<T, R>(this T o, Func<T, R> returnFunc, R elseVal)
        {
            if (o.IsNull())
                return elseVal;
            return returnFunc(o);
        }

        public static R IfNotNull<T, R>(this T o, Func<T, R> returnFunc)
        {
            return o.IfNotNull(returnFunc, default(R));
        }

        public static void IfIsOfType<T>(this object o, Action<T> action) where T : class
        {
            T t = o as T;
            if (!Object.ReferenceEquals(null, t))
                action(t);
        }
        public static bool IfIsOfType<T>(this object o, Func<T, bool> predicate) where T : class
        {
            T t = o as T;
            if (Object.ReferenceEquals(null, t))
                return false;
            return predicate(t);
        }

        public static bool IsTypeOf<T>(this object o)
        {
            return o.IsTypeOf(typeof(T));
        }

        public static bool IsTypeOf(this object o, Type type)
        {
            return o.GetType() == type;
        }

        public static T As<T>(this object o) where T : class
        {
            return o as T;
        }

        public static bool IsNull(this object obj)
        {
            return ReferenceEquals(obj, null);
        }

        public static bool IsNotNull(this object obj)
        {
            return !ReferenceEquals(obj, null);
        }

        public static bool IsTrue(this object obj)
        {
            return obj.IsNotNull() && (bool)obj == true;
        }

        public static VALUE TryGetValue<KEY, VALUE>(this IDictionary<KEY, VALUE> dict, KEY key)
        {
            return dict.ContainsKey(key) ? dict[key] : default(VALUE);
        }
        public static void ForceSetValue<KEY, VALUE>(this IDictionary<KEY, VALUE> dict, KEY key, Func<VALUE, VALUE> prevToNewValue, VALUE @default = default(VALUE))
        {
            if (dict.ContainsKey(key))
                dict[key] = prevToNewValue(dict[key]);
            else
                dict.Add(key, prevToNewValue(@default));
        }

        public static IEnumerable<T> WrapInEnumerable<T>(this T obj)
        {
            if (obj.IsNull())
                return new T[0];
            return new T[] { obj };
        }

        public static bool NullSafeEquals(this object x, object y)
        {
            return (x.IsNull() && y.IsNull()) || (x.IsNotNull() && y.IsNotNull() && x.Equals(y));
        }

        public static object ConvertToNullableType(this object value)
        {
            if (value == null)
                return value;

            var type = value.GetType();

            var isnullableType = type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
            if (isnullableType || !type.IsValueType) return value;

            var nullableConverter = new NullableConverter(typeof(Nullable<>).MakeGenericType(type));
            return nullableConverter.ConvertFrom(value);

        }
    }
}