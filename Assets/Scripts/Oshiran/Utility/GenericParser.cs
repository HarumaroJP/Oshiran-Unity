using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


namespace Oshiran.Utility
{
    public static class GenericParser
    {
        /// <summary>
        /// stringをTのオブジェクトに変換する
        /// </summary>
        /// <param name="input">オブジェクトに変換したいstring</param>
        /// <typeparam name="T">変換後のオブジェクトの型</typeparam>
        /// <returns>変換後のオブジェクト</returns>
        public static T Parse<T>(string input)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromString(input);
        }

        public static bool TryParse<T>(string input, ref T result)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

                result = (T)converter.ConvertFromString(input);
                return true;
            }
            catch
            {
                Debug.Log($"指定された型{typeof(T)}にはConverterがありません！");
                return false;
            }
        }
    }
}