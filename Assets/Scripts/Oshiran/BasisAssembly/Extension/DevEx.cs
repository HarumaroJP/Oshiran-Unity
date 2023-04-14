using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class DevEx {
    public static bool InRange(this float t, float start, float end) => start <= t && t <= end;


    public static T[] OrderByName<T>(this T[] array) where T : UnityEngine.Object {
        return array.OrderBy(sprite => sprite.name).ToArray();
    }


    public static void SetAlpha(this ref Color c, float a) {
        c.a = a;
    }


#if UNITY_EDITOR
    public static void ClearLog() {
        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
#endif
}
