using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


public class LevelEditorWindow : EditorWindow
{
    private string jsonFilePath = "Assets/CustonPlugin/Json/collectibles.json";
    private LevelData levelData; 
    private LevelGenerator levelGenerator;
    private Dictionary<string, string> levelPaths = new Dictionary<string, string>();

    [MenuItem("Tools/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("Level Generator");
    }
   private void OnGUI()
        {
           
                GUILayout.Label("Level Generator", EditorStyles.boldLabel);

                // Input field for JSON file path
                jsonFilePath = EditorGUILayout.TextField("JSON File Path", jsonFilePath);

                // Button to open file browser for selecting JSON file
                if (GUILayout.Button("Browse JSON File"))
                {
                    jsonFilePath = EditorUtility.OpenFilePanel("Select JSON File", Application.dataPath, "json");
                }

                // Button to load and preview JSON
                if (GUILayout.Button("Load JSON File"))
                {
                    LoadAndPreviewLevel();
                }

                // Display Level Data Preview (if loaded)
                if (levelData != null)
                {
                    EditorGUILayout.Space();
                    GUILayout.Label($"Level Name: {levelData.levelName}");
                    GUILayout.Label($"Dimensions: {levelData.dimensions.width} x {levelData.dimensions.height}");
                    GUILayout.Label($"Collectibles: {levelData.collectibles.Length}");
                    GUILayout.Label($"Obstacles: {levelData.obstacles.Length}");
                }

                // Button to generate the level
                if (GUILayout.Button("Generate Level"))
                {
                    GenerateLevel();
                }
                GUILayout.Space(10);
                GUILayout.Label("Instructions:", EditorStyles.boldLabel);
                GUILayout.Label("1. Make sure your JSON file is correctly formatted.", EditorStyles.label);
                GUILayout.Label("2. Make sure your the address of JSON File is correctely given.", EditorStyles.label);
                GUILayout.Label("3. If not click on Browse JSON File button and select the file from your system.", EditorStyles.label);
                GUILayout.Label("4. Press the 'Generate Level' button to load and spawn objects.", EditorStyles.label);


        }
    



    private void LoadAndPreviewLevel()
    {
        if (System.IO.File.Exists(jsonFilePath))
        {
            try
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                levelData = JsonUtility.FromJson<LevelData>(jsonContent);

                Debug.Log($"Level Loaded: {levelData.levelName}");
                Debug.Log($"Dimensions: {levelData.dimensions.width} x {levelData.dimensions.height}");
                Debug.Log($"Collectibles Count: {levelData.collectibles.Length}");
                Debug.Log($"Obstacles Count: {levelData.obstacles.Length}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error loading JSON file: {ex.Message}");
            }
        }
        else
        {
            Debug.LogError("JSON file not found at the specified path!");
        }
    }

    private void GenerateLevel()
    {
        levelGenerator = FindObjectOfType<LevelGenerator>();

        if (levelGenerator != null)
        {
            levelGenerator.jsonFilePath = jsonFilePath;
            levelGenerator.GenerateLevel();
        }
        else
        {
            Debug.LogError("LevelGenerator script not found in the scene!");
        }
    }
}
