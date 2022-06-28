using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

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
            Debug.Log("Auto-saving...");
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
        }
    }
}
