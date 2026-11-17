using UnityEngine;

public class LevelBouwer : MonoBehaviour
{
    [Header("Level bouwer objects")]
    public GameObject[] tiles;
    public GameObject[] objecten;

    void Start()
    {
        if (LevelOverdracht.GeselecteerdLevel == null)
        {
            Debug.LogWarning("Er is geen level gevonden om te bouwen!");
            return;
        }

        LevelData level = LevelOverdracht.GeselecteerdLevel;

        foreach (TileData tile in level.alleTiles)
        {
            if (tile.type >= 0 && tile.type < tiles.Length)
            {
                Vector3 positie = new Vector3(tile.xPos, tile.yPos, 0);
                Instantiate(tiles[tile.type], positie, Quaternion.Euler(0, 0, tile.rotatie));
            }
        }

        foreach (ObjectData obj in level.alleObjecten)
        {
            if (obj.objectType >= 0 && obj.objectType < objecten.Length)
            {
                Vector3 positie = new Vector3(obj.xPos, obj.yPos, 0);
                GameObject nieuwObject = Instantiate(objecten[obj.objectType], positie, Quaternion.Euler(0, 0, obj.rotatieZ));
                
                nieuwObject.transform.localScale = new Vector3(obj.schaalX, obj.schaalY, 1);
            }
        }
    }
}