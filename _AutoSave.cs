using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Reflection;

[InitializeOnLoad]
public static class _AutoSave
{
    static _AutoSave()
    {
        EditorApplication.playModeStateChanged += _saveSceneWhenEntersPlayMode;
    }

    private static void _saveSceneWhenEntersPlayMode(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            _ClearConsole();
            Debug.Log("Auto-saving...");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }

    [MenuItem("CovicDev/Clear Console #%q")]
    public static void _ClearConsole()
    {
        var assembly = Assembly.GetAssembly(typeof(SceneView));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(null, null);
    }

    [MenuItem("CovicDev/Toggle Lock Inspector &e")]
    static void _ToggleLockInspector()
    {
        EditorWindow inspectorToBeLocked = EditorWindow.mouseOverWindow;
        if (inspectorToBeLocked != null && inspectorToBeLocked.GetType().Name == "InspectorWindow")
        {
            Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
            PropertyInfo propertyInfo = type.GetProperty("isLocked");

            bool value = (bool)propertyInfo.GetValue(inspectorToBeLocked, null);
            propertyInfo.SetValue(inspectorToBeLocked, !value, null);

            inspectorToBeLocked.Repaint();
        }
    }

    [MenuItem("CovicDev/Toggle Debug Inspector &q")]
    static void _ToggleInspector()
    {
        EditorWindow targetInspector = EditorWindow.mouseOverWindow;
        if (targetInspector != null && targetInspector.GetType().Name == "InspectorWindow")
        {
            Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
            FieldInfo field = type.GetField("m_InspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            InspectorMode mode = (InspectorMode)field.GetValue(targetInspector);
            mode = (mode == InspectorMode.Normal ? InspectorMode.Debug : InspectorMode.Normal);

            MethodInfo method = type.GetMethod("SetMode", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(targetInspector, new object[] { mode });

            targetInspector.Repaint();
        }
    }
}

