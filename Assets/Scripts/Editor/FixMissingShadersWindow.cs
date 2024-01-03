using UnityEngine;
using UnityEditor;

public class FixMissingShadersWindow : EditorWindow
{
    private string targetShaderName = "Hidden/InternalErrorShader";
    private string replaceShaderName = "CartoonCoffee/ParticleAdditive";
    private string searchPath = "Assets/CartoonVFX9X/Super MegaPack/Materials";  // 기본 경로 설정

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
        int replacedCount = 0;  // 추가: 변경된 메테리얼의 수를 카운트합니다.

        foreach (string guid in materialGUIDs)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(assetPath);

            if (mat.shader.name == targetShaderName)
            {
                mat.shader = newShader;
                EditorUtility.SetDirty(mat);  // 변경된 메테리얼을 '더러운' 상태로 표시하여 변경 사항을 저장합니다.
                replacedCount++;
            }
        }

        AssetDatabase.SaveAssets();  // 변경된 메테리얼을 저장합니다.
        AssetDatabase.Refresh();     // 에셋 데이터베이스를 갱신합니다.

        Debug.Log($"Shader replacement done! {replacedCount} materials updated.");  // 변경된 메테리얼의 수를 로그로 출력합니다.
    }

}
