using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public string jsonFilePath = "Assets/CustonPlugin/Json/collectibles.json";
    public GameObject coinPrefab; // Assign your collectible prefabs in the Inspector
    public GameObject gemPrefab;
    public GameObject healthPackPrefab;
    public GameObject rockPrefab;
    public GameObject treePrefab;// Assign your obstacle prefabs in the Inspector
    public GameObject ground;

    public void GenerateLevel()
    {
        // Validate the file path
        if (!System.IO.File.Exists(jsonFilePath))
        {
            Debug.LogError("JSON file not found at the specified path!");
            return;
        }

        // Read the JSON file
        string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

        try
        {
            // Deserialize the JSON content
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonContent);

            // Clear existing objects in the scene (optional)
            ClearExistingLevel();

            // Generate level elements
            GenerateLevelElements(levelData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing JSON file: {ex.Message}");
        }
    }

    private void ClearExistingLevel()
    {
        // Remove all collectibles and obstacles in the scene
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    public void GenerateLevelElements(LevelData levelData)
    {
        // Set the background (optional logic based on your game)
        Debug.Log($"Setting background to: {levelData.background}");
        

        // Adjust the ground size based on level dimensions
        //GameObject ground = GameObject.Find("Ground"); // Assuming you have a GameObject named "Ground"
        if (ground != null && levelData.dimensions != null)
        {
            // Resize the ground to match the level dimensions
            ground.transform.localScale = new Vector3(levelData.dimensions.width, 1, levelData.dimensions.height);
            Debug.Log($"Ground size set to: {levelData.dimensions.width} x {levelData.dimensions.height}");
        }

        // Create collectibles
        foreach (var collectible in levelData.collectibles)
        {
            GameObject prefab = GetCollectiblePrefab(collectible.type);
            if (prefab != null)
            {
                GameObject collectibleObject = Instantiate(prefab);
                collectibleObject.name = collectible.type;
                collectibleObject.transform.position = collectible.position; 
                collectibleObject.transform.localScale = collectible.size;// Apply position directly
                collectibleObject.transform.rotation = Quaternion.Euler(collectible.rotation);  // Apply rotation directly using Vector3
                collectibleObject.transform.SetParent(transform); // Attach to LevelGenerator for organization
            }
        }

        // Create obstacles
        foreach (var obstacle in levelData.obstacles)
        {
            GameObject prefab = GetObstaclePrefab(obstacle.type);
            if (prefab != null)
            {
                GameObject obstacleObject = Instantiate(prefab);
                obstacleObject.name = obstacle.type;
                obstacleObject.transform.position = obstacle.position;  // Apply position directly
                obstacleObject.transform.localScale = obstacle.size;  // Apply size if available
                obstacleObject.transform.rotation = Quaternion.Euler(obstacle.rotation);  // Apply rotation directly using Vector3
                obstacleObject.transform.SetParent(transform); // Attach to LevelGenerator for organization
            }
        }

        Debug.Log("Level generation complete!");
    }



    private GameObject GetCollectiblePrefab(string type)
    {
        // Return the appropriate prefab based on the collectible type
        switch (type)
        {
            case "Coin":
                return coinPrefab;
            case "Gem":
                return gemPrefab;
            case "HealthPack":
                return healthPackPrefab;
            default:
                Debug.LogWarning($"Unknown collectible type: {type}");
                return null;
        }
    }

    private GameObject GetObstaclePrefab(string type)
    {
        // Return the appropriate prefab based on the obstacle type
        switch (type)
        {
            case "Rock":
                return rockPrefab;
            case "Tree":
                return treePrefab;
            default:
                Debug.LogWarning($"Unknown obstacle type: {type}");
                return null;
        }
    }
}
