using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace GoogleApi.Extensions
{
    /// <summary>
    /// Enum extensions.
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Convert enum to string.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> type.</typeparam>
        /// <param name="enum">The <see cref="Enum"/> to convert of <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="string"/> representation of the enum value.</returns>
        public static string ToEnumString<T>(this T @enum)
            where T : struct
        {
            var enumType = typeof(T);
            var name = Enum.GetName(enumType, @enum); 
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetRuntimeField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();

            return enumMemberAttribute.Value;
        }

        /// <summary>
        /// Converts a <see cref="Enum"/> value to string. 
        /// If enum is a <see cref="FlagsAttribute"/>, values are separated by the passed <paramref name="delimeter"/>.
        /// </summary>
        /// <typeparam name="T">The <see cref="Enum"/> type.</typeparam>
        /// <param name="enum">The <see cref="Enum"/> to convert of <typeparamref name="T"/>.</param>
        /// <param name="delimeter">The separator inserted between each value if the <see cref="Enum"/>.</param>
        /// <returns>A <see cref="string"/> representation of the enum value.</returns>
        public static string ToEnumString<T>(this T @enum, char delimeter)
            where T : struct
        {
            if (@enum.GetType().GetTypeInfo().GetCustomAttributes(typeof(FlagsAttribute), true).FirstOrDefault() == null)
                return Convert.ToString(@enum, CultureInfo.InvariantCulture).ToLower();

            var stringBuilder = new StringBuilder();
            var binaryCharArray = Convert.ToString(@enum, CultureInfo.InvariantCulture).Reverse().ToArray();

            for (var i = 0; i < binaryCharArray.Length; i++)
            {
                if (binaryCharArray[i] != '1')
                    continue;

                stringBuilder.AppendFormat("{0}", 1 << i);

                if (i != binaryCharArray.Length - 1)
                    stringBuilder.Append(delimeter);
            }

            return stringBuilder.ToString().ToLower();
        }
    }
}