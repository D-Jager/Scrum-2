using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditor : MonoBehaviour
{
    enum ToolMode
    {
        Tile,
        Object
    }

    [Serializable]
    public class Definition
    {
        public string id;
        public string name;
        public string description;
    }

    [Serializable]
    public class TileDefinition : Definition
    {
        public TileBase tile;
    }

    [Serializable]
    public class ObjectDefinition : Definition
    {
        public GameObject prefab;
    }

    [Header("Object Container")]
    [SerializeField] private Transform objectContainer;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap previewTilemap;

    [Header("Tiles")]
    [SerializeField] private List<TileDefinition> tiles = new();

    [Header("Objects")]
    [SerializeField] private List<ObjectDefinition> objects = new();

    [Header("Preview")]
    [SerializeField, Range(0f, 1f)]
    private float previewAlpha = 0.5f;

    private ToolMode toolMode = ToolMode.Tile;

    private int selectedTileIndex;
    private TileDefinition selectedTile;

    private int selectedObjectIndex;
    private ObjectDefinition selectedObject;
    private List<GameObject> previewObjects = new();

    private Vector3Int lastPlacedCell;
    private Vector3 lastPlacedObjectPosition;
    private bool hasPlaced;
    private bool snapToGrid;

    private void Start()
    {
        SetToolMode(ToolMode.Tile);
    }

    private void Update()
    {
        HandleInput();
        UpdatePreview();
    }

    private void HandleInput()
    {
        snapToGrid = !(Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt));
        if (Input.GetKeyDown(KeyCode.Q)) ChangeToolMode(-1);
        if (Input.GetKeyDown(KeyCode.E)) ChangeToolMode(1);

        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0f) ChangeSelection(1);
        else if (scroll < 0f) ChangeSelection(-1);

        if (Input.GetMouseButton(0)) Place();
        if (Input.GetMouseButton(1)) Erase();
        if (Input.GetMouseButtonUp(0)) hasPlaced = false;
    }

    private void ChangeToolMode(int direction)
    {
        if (direction == 0) return;

        // Source - https://stackoverflow.com/a/643438
        // Posted by husayt, modified by community. See post 'Timeline' for change history
        // Retrieved 2026-09-29, License - CC BY-SA 4.0
        // ^ enums cyclen ;(

        ToolMode[] array = (ToolMode[])Enum.GetValues(typeof(ToolMode));
        int i = Array.IndexOf(array, toolMode) + direction;

        if (i >= array.Length) i = 0;
        if (i < 0) i = array.Length - 1;
        ToolMode next = array[i];

        SetToolMode(next);
    }

    private void SetToolMode(ToolMode mode)
    {
        if (toolMode == mode) return;

        toolMode = mode;
        hasPlaced = false;

        previewTilemap.ClearAllTiles();
        ClearObjectPreview();

        Debug.Log($"Set tool mode: {toolMode}");
    }

    private void ChangeSelection(int direction)
    {
        switch (toolMode)
        {
            case ToolMode.Tile:
                if (tiles.Count == 0) return;

                selectedTileIndex += direction;
                if (selectedTileIndex < 0) selectedTileIndex = tiles.Count - 1;
                if (selectedTileIndex >= tiles.Count) selectedTileIndex = 0;

                selectedTile = tiles[selectedTileIndex];
                break;

            case ToolMode.Object:
                if (objects.Count == 0) return;

                selectedObjectIndex += direction;
                if (selectedObjectIndex < 0) selectedObjectIndex = objects.Count - 1;
                if (selectedObjectIndex >= objects.Count) selectedObjectIndex = 0;

                selectedObject = objects[selectedObjectIndex];

                CreateObjectPreview();
                break;

            default: break;
        }

        Debug.Log($"selected tile: {selectedTile?.id}, selected object: {selectedObject?.id}");
    }

    private void UpdatePreview()
    {
        switch (toolMode) {
            case ToolMode.Tile:
                UpdateTilePreview();
                break;

            case ToolMode.Object:
                UpdateObjectPreview();
                break;

            default: break;
        }
    }

    private void UpdateTilePreview()
    {
        if (tilemap == null || previewTilemap == null || tiles.Count == 0) return;

        selectedTile ??= tiles[selectedTileIndex];
        Vector3Int cell = GetMouseCell();
        previewTilemap.ClearAllTiles();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighbour = cell + new Vector3Int(x, y, 0);

                TileBase existingTile = tilemap.GetTile(neighbour);

                if (existingTile != null)
                {
                    previewTilemap.SetTile(neighbour, existingTile);
                }
            }
        }

        previewTilemap.SetTile(cell, selectedTile.tile);

        Color color = previewTilemap.color;
        color.a = previewAlpha;

        previewTilemap.color = color;
    }

    private void CreateObjectPreview()
    {
        ClearObjectPreview();

        if (selectedObject == null || selectedObject.prefab == null) return;
        if (previewObjects.Count > 0) return;

        GameObject preview = Instantiate(selectedObject.prefab, Vector3.zero, Quaternion.identity, objectContainer);

        foreach (Collider2D collider in preview.GetComponentsInChildren<Collider2D>()) collider.enabled = false;

        foreach (SpriteRenderer sr in preview.GetComponentsInChildren<SpriteRenderer>())
        {
            Color color = sr.color;
            color.a *= previewAlpha;

            sr.color = color;
        }

        previewObjects.Add(preview);
    }

    private void ClearObjectPreview()
    {
        foreach (GameObject previewObject in previewObjects) Destroy(previewObject);

        previewObjects.Clear();
    }

    private void UpdateObjectPreview()
    {
        if (objects.Count == 0) return;

        selectedObject ??= objects[selectedObjectIndex];
        if (previewObjects.Count == 0) CreateObjectPreview();

        Vector3 position = GetMouseWorldPosition();

        if (snapToGrid)
        {
            int snapX = Mathf.FloorToInt(position.x);
            int snapY = Mathf.FloorToInt(position.y);

            position = new Vector3(snapX + 0.5f, snapY + 0.5f, position.z);
        }

        foreach (GameObject previewObject in previewObjects)
        {
            if (previewObject != null) previewObject.transform.position = position;
        }
    }



    private void Place()
    {
        switch (toolMode)
        {
            case ToolMode.Tile:
                if (selectedTile == null) return;

                Vector3Int cell = GetMouseCell();
                if (hasPlaced && cell == lastPlacedCell) return;

                tilemap.SetTile(cell, selectedTile.tile);

                lastPlacedCell = cell;
                break;

            case ToolMode.Object:
                if (selectedObject == null) return;

                Vector3 position = GetMouseWorldPosition();
                if (snapToGrid)
                {
                    int snapX = Mathf.FloorToInt(position.x);
                    int snapY = Mathf.FloorToInt(position.y);

                    position = new Vector3(snapX + 0.5f, snapY + 0.5f, position.z);
                }
                if (hasPlaced && Vector3.Distance(position, lastPlacedObjectPosition) < 1f) return;
                if (GetMouseGameObjects().Count > 0) return;
                lastPlacedObjectPosition = position;

                Instantiate(selectedObject.prefab, position, Quaternion.identity, objectContainer);
                break;

            default: break;
        }

        hasPlaced = true;
    }

    private void Erase()
    {
        switch (toolMode)
        {
            case ToolMode.Tile:
                Vector3Int cell = GetMouseCell();

                if (tilemap.HasTile(cell)) tilemap.SetTile(cell, null);
                break;

            case ToolMode.Object:
                List<GameObject> gameObjects = GetMouseGameObjects();

                foreach (GameObject gameObject in gameObjects) {
                    if (gameObject.GetComponent<Tilemap>()) continue;

                    gameObject.SetActive(false);
                    Destroy(gameObject);
                };
                break;

            default: break;
        }
    }

    private Vector3Int GetMouseCell()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();

        return tilemap.WorldToCell(mouseWorldPosition);
    }

    private List<GameObject> GetMouseGameObjects()
    {
        Vector3 mouseWorldPosition = GetMouseWorldPosition();
        Collider2D[] collisions = Physics2D.OverlapCircleAll(mouseWorldPosition, 0.05f);
        GameObject[] gameObjects = new GameObject[collisions.Length];

        int i = 0;
        foreach (Collider2D collision in collisions)
        {
            gameObjects.SetValue(collision.gameObject, i);
            i++;
        }

        return new List<GameObject>(gameObjects);
    }

    private Vector3 GetMouseWorldPosition()
    {
        Camera camera = Camera.main;
        if (camera == null) return Vector3.zero;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(camera.transform.position.z);

        Vector3 worldPosition = camera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;

        return worldPosition;
    }
}
