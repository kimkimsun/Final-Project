using UnityEngine;
using UnityEditor;

public class FixMissingShadersWindow : EditorWindow
{
    private string targetShaderName = "Hidden/InternalErrorShader";
    private string replaceShaderName = "CartoonCoffee/ParticleAdditive";
    private string searchPath = "Assets/CartoonVFX9X/Super MegaPack/Materials";  // �⺻ ��� ����

    [MenuItem("Custom/Fix Missing Shaders Window")]
    public static void ShowWindow()
    {
        GetWindow<FixMissingShadersWindow>("Fix Missing Shaders");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Fix Missing Shaders in Specific Path", EditorStyles.boldLabel);

        targetShaderName = EditorGUILayout.TextField("Target Shader Name:", targetShaderName);
        replaceShaderName = EditorGUILayout.TextField("Replace With Shader Name:", replaceShaderName);
        searchPath = EditorGUILayout.TextField("Search Path:", searchPath);

        if (GUILayout.Button("Replace Shaders"))
        {
            ReplaceShaders();
        }
    }

    private void ReplaceShaders()
    {
        Shader newShader = Shader.Find(replaceShaderName);

        if (newShader == null)
        {
            Debug.LogError("Replacement shader not found.");
            return;
        }

        string[] materialGUIDs = AssetDatabase.FindAssets("t:Material", new string[] { searchPath });
        int replacedCount = 0;  // �߰�: ����� ���׸����� ���� ī��Ʈ�մϴ�.

        foreach (string guid in materialGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

            if (mat.shader.name == targetShaderName)
            {
                mat.shader = newShader;
                EditorUtility.SetDirty(mat);  // ����� ���׸����� '������' ���·� ǥ���Ͽ� ���� ������ �����մϴ�.
                replacedCount++;
            }
        }

        AssetDatabase.SaveAssets();  // ����� ���׸����� �����մϴ�.
        AssetDatabase.Refresh();     // ���� �����ͺ��̽��� �����մϴ�.

        Debug.Log($"Shader replacement done! {replacedCount} materials updated.");  // ����� ���׸����� ���� �α׷� ����մϴ�.
    }

}
