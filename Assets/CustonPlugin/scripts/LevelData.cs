
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string levelName;
    public Dimensions dimensions;
    public string background;
    public Collectible[] collectibles;
    public Obstacle[] obstacles;
}

[System.Serializable]
public class Dimensions
{
    public float width;
    public float height;
}

[System.Serializable]
public class Collectible
{
    public string type;
    public Vector3 position;
    public Vector3 size;
    public Vector3 rotation;
}

[System.Serializable]
public class Obstacle
{
    public string type;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 size;  // Optiona
}

