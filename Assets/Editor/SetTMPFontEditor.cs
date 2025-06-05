using UnityEngine;
using UnityEditor;
using TMPro;

public class SetTMPFontEditor : EditorWindow
{
    TMP_FontAsset fontAsset;
    GameObject rootObject;

    [MenuItem("Tools/Set TMP Font")]
    public static void ShowWindow()
    {
        GetWindow<SetTMPFontEditor>("Set TMP Font");
    }

    void OnGUI()
    {
        GUILayout.Label("Set TMP Font in Scene", EditorStyles.boldLabel);

        fontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("Font Asset", fontAsset, typeof(TMP_FontAsset), false);
        rootObject = (GameObject)EditorGUILayout.ObjectField("Root GameObject", rootObject, typeof(GameObject), true);

        GUI.enabled = fontAsset != null && rootObject != null;
        if (GUILayout.Button("Apply Font"))
        {
            ApplyFontToChildren();
        }
        GUI.enabled = true;
    }

    void ApplyFontToChildren()
    {
        int count = 0;

        // Set font on TextMeshPro (3D)
        var textComponents3D = rootObject.GetComponentsInChildren<TextMeshPro>(true);
        foreach (var tmp in textComponents3D)
        {
            Undo.RecordObject(tmp, "Set TMP Font");
            tmp.font = fontAsset;
            EditorUtility.SetDirty(tmp);
            count++;
        }

        // Set font on TextMeshProUGUI (UI)
        var textComponentsUI = rootObject.GetComponentsInChildren<TextMeshProUGUI>(true);
        foreach (var tmp in textComponentsUI)
        {
            Undo.RecordObject(tmp, "Set TMP Font");
            tmp.font = fontAsset;
            EditorUtility.SetDirty(tmp);
            count++;
        }

        Debug.Log($"Updated {count} TextMeshPro components.");
    }
}
