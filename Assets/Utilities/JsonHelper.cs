using System;
using UnityEngine;

namespace Utilities
{
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<WrapperArray<T>>(json);
            return wrapper.items;
        }

        public static T[] FromJsonOverwrite<T>(string json)
        {
            var newArray = new WrapperArray<T>();
            JsonUtility.FromJsonOverwrite(json, newArray);
            return newArray.items;
        }

        public static string ToJson<T>(T[] array)
        {
            var wrapper = new WrapperArray<T>
            {
                items = array
            };
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            var wrapper = new WrapperArray<T>
            {
                items = array
            };
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static string FixJson(string value)
        {
            value = "{\"items\":" + value + "}";
            return value;
        }

        [Serializable]
        public class WrapperArray<T>
        {
            public T[] items;
        }
    }
}