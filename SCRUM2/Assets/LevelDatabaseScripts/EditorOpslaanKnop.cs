using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorOpslaan : MonoBehaviour
{
    public LevelDatabaseManager databaseManager;
    public TMP_InputField levelNameInput;
    public TMP_InputField playerNameInput;
    public GameObject savePanel;

    public void VerzamelAllesEnSlaOp()
    {
        LevelData nieuwLevel = new LevelData(); 
        nieuwLevel.levelNaam = levelNameInput.text;
        nieuwLevel.makerNaam = playerNameInput.text;

        LevelObjectInfo[] allesOpHetScherm = FindObjectsOfType<LevelObjectInfo>();

        foreach (LevelObjectInfo ding in allesOpHetScherm)
        {
            float xPos = ding.transform.position.x;
            float yPos = ding.transform.position.y;
            float zRot = ding.transform.eulerAngles.z; 
            float schaalX = ding.transform.localScale.x;
            float schaalY = ding.transform.localScale.y;

            if (ding.isTegel)
            {
                nieuwLevel.alleTiles.Add(new TileData(ding.objectType, xPos, yPos, zRot));
            }
            else
            {
                nieuwLevel.alleObjecten.Add(new ObjectData(ding.objectType, xPos, yPos, zRot, schaalX, schaalY, ding.extraData));
            }
        }

        databaseManager?.SlaLevelOp(nieuwLevel);
    }

    public void SlaOpPanel()
    {
        if (savePanel.activeSelf == false) savePanel.SetActive(true);
        else savePanel.SetActive(false);
    }
}