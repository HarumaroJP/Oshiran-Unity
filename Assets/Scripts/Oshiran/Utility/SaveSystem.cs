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
        static readonly string SAVEDIR = $"{Application.dataPath}/SaveData/";

        static string NameToFilePath(string name) => SAVEDIR + name + ".json";

        public static bool FileExists(string name)
        {
            return File.Exists(NameToFilePath(name));
        }

        public static bool DirExists(string name)
        {
            return Directory.Exists(Application.dataPath + name);
        }


        public static async UniTask Save(object obj, string name, bool format = false)
        {
            if (!DirExists("SaveData"))
            {
                Directory.CreateDirectory(SAVEDIR);
            }

            string text = obj.GetType().IsPrimitiveOrString() ? obj.ToString() : JsonUtility.ToJson(obj, format);

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
                using FileStream fs = new FileStream(NameToFilePath(name), FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    rawData = await sr.ReadToEndAsync();
                }
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
            T deserializedData = default;

            try
            {
                using FileStream fs = new FileStream(NameToFilePath(name), FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    string result = await sr.ReadToEndAsync();

                    bool isPrimitive = typeof(T).IsPrimitive;
                    deserializedData = isPrimitive ? GenericParser.Parse<T>(result) : JsonUtility.FromJson<T>(result);
                }
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
                using FileStream fs = new FileStream(NameToFilePath(name), FileMode.Open);

                using (StreamReader sr = new StreamReader(fs))
                {
                    string result = await sr.ReadToEndAsync();

                    JsonUtility.FromJsonOverwrite(result, scriptableObject);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load the file!");
                Debug.LogException(e);
            }
        }
    }
}