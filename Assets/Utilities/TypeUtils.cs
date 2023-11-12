using System;

namespace Utilities
{
    public class TypeUtils
    {
        public static string FormatTime(float timeInSecond)
        {
            var minutes = timeInSecond / 60;
            var seconds = timeInSecond % 60;

            return $"{minutes:00}:{seconds:00}";
        }
        
        
        public static string TrimUTF8Bom(string value)
        {
            value = value.Trim('\uFEFF', '\u200B');
            return value;
        }
        
        public static bool IsTypeNullable<T>()
        {
            return Nullable.GetUnderlyingType( typeof(T) )!=null;
        }
    }
}