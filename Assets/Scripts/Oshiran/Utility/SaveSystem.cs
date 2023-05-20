using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Oshiran.Utility
{
    public static class SaveSystem
    {
        static readonly string WebGLKey = "JsonSaveData";

#if UNITY_EDITOR
        static readonly string SAVEDIR = $"{Application.dataPath}/SaveData/";
#else
        static readonly string SAVEDIR = $"{Application.persistentDataPath}/SaveData/";
#endif

        static string NameToFilePath(string name) => SAVEDIR + name + ".json";

        public static bool FileExists(string name)
        {
            return File.Exists(NameToFilePath(name));
        }

        public static bool DirExists(string name)
        {
            return Directory.Exists(SAVEDIR + name);
        }


        public static async UniTask Save(object obj, string name, bool format = false)
        {
            string text = obj.GetType().IsPrimitiveOrString() ? obj.ToString() : JsonUtility.ToJson(obj, format);

#if UNITY_WEBGL
            PlayerPrefs.SetString(WebGLKey, text);
            PlayerPrefs.Save();
            return;
#endif

            if (!DirExists("SaveData"))
            {
                Directory.CreateDirectory(SAVEDIR);
            }

            using (StreamWriter sw = new StreamWriter(NameToFilePath(name), false))
            {
                try
                {
                    await sw.WriteLineAsync(text);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to save the file!");
                    Debug.LogException(e);
                    throw;
                }
            }
        }


        static bool IsPrimitiveOrString(this Type type)
        {
            return type.IsPrimitive || type == typeof(string);
        }

        public static void Delete(string name)
        {
            if (FileExists(name))
            {
                File.Delete(NameToFilePath(name));
            }
        }

        //System.Stringで読み込む場合はこっちだけ
        public static async UniTask<string> LoadRaw(string name)
        {
            string rawData = null;

            try
            {
#if UNITY_WEBGL
                rawData = PlayerPrefs.GetString(WebGLKey);
#else
                using FileStream fs = new FileStream(NameToFilePath(name), FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    rawData = await sr.ReadToEndAsync();
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load the file!");
                Debug.LogException(e);
            }

            return rawData;
        }

        //System.String, ScriptableObject, MonoBehaviourクラス以外で利用可能なロード機能
        public static async UniTask<T> Load<T>(string name)
        {
            bool isPrimitive = typeof(T).IsPrimitive;
            T deserializedData = default;

            try
            {
#if UNITY_WEBGL
                string result = PlayerPrefs.GetString(WebGLKey);
                deserializedData = isPrimitive ? GenericParser.Parse<T>(result) : JsonUtility.FromJson<T>(result);
#else
                using FileStream fs = new FileStream(NameToFilePath(name), FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    string result = await sr.ReadToEndAsync();

                    deserializedData = isPrimitive ? GenericParser.Parse<T>(result) : JsonUtility.FromJson<T>(result);
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load the file!");
                Debug.LogException(e);
            }

            return deserializedData;
        }

        //ScriptableObjectを継承している場合はこっちを利用する
        public static async UniTask LoadOverwrite<T>(string name, T scriptableObject) where T : ScriptableObject
        {
            try
            {
#if UNITY_WEBGL
                string result = PlayerPrefs.GetString(WebGLKey);
                JsonUtility.FromJsonOverwrite(result, scriptableObject);
#else
                using FileStream fs = new FileStream(NameToFilePath(name), FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    string result = await sr.ReadToEndAsync();

                    JsonUtility.FromJsonOverwrite(result, scriptableObject);
                }
#endif
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load the file!");
                Debug.LogException(e);
            }
        }
    }
}