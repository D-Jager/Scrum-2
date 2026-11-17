using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public string id;           
    public string levelNaam;    
    public string makerNaam;  
    
    public List<TileData> alleTiles = new List<TileData>();       
    public List<ObjectData> alleObjecten = new List<ObjectData>(); 
}

[System.Serializable]
public class TileData
{
    public int type; 
    public float xPos, yPos;
    public float rotatie; 

    public TileData(int type, float x, float y, float rot = 0f)
    {
        this.type = type;
        xPos = x;
        yPos = y;
        rotatie = rot;
    }
}

[System.Serializable]
public class ObjectData
{
    public int objectType; 
    public float xPos, yPos;
    public float rotatieZ; 
    public float schaalX = 1f; 
    public float schaalY = 1f;
    public string extraData; 

    public ObjectData(int type, float x, float y, float rot = 0f, float sX = 1f, float sY = 1f, string extra = "")
    {
        objectType = type;
        xPos = x;
        yPos = y;
        rotatieZ = rot;
        schaalX = sX;
        schaalY = sY;
        extraData = extra;
    }
}