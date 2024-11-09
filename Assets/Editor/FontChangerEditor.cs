using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class FontChangerEditor : EditorWindow
{
    private TMP_FontAsset newFont;

    [MenuItem("Tools/Font Changer")]
    public static void ShowWindow()
    {
        GetWindow<FontChangerEditor>("Font Changer");
    }

    void OnGUI()
    {
        GUILayout.Label("Change Font of All TextMeshPro UI Components", EditorStyles.boldLabel);

        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Change Font"))
        {
            ChangeFontInScene();
        }
    }

    void ChangeFontInScene()
    {
        if (newFont == null)
        {
            Debug.LogWarning("Please assign a new font.");
            return;
        }

        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootObject in rootObjects)
        {
            ChangeFontInChildren(rootObject);
        }

        Debug.Log("Font changed for all TextMeshPro UI components in the scene.");
    }

    void ChangeFontInChildren(GameObject obj)
    {
        TextMeshProUGUI textComponent = obj.GetComponent<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.font = newFont;
            EditorUtility.SetDirty(textComponent);
        }

        foreach (Transform child in obj.transform)
        {
            ChangeFontInChildren(child.gameObject);
        }
    }
}
