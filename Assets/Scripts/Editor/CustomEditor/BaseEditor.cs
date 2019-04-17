using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomEditor(typeof(MonoBehaviour), true)]
public class BaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Type type = target.GetType();
        MethodInfo[] methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        foreach(var m in methods)
        {
            if(m.GetCustomAttribute<ButtonAttribute>() != null)
            {
                if (GUILayout.Button(m.Name))
                {
                    m.Invoke(target, null);
                }
            }
        }
    }
}
