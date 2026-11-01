using UnityEngine;

public class GridTileLayout : MonoBehaviour
{
    GridTile[,] gridTiles;
    [SerializeField] Vector2Int gridSize = Vector2Int.one * 10;
    [SerializeField] float gridTileHeight = 10f;
    [SerializeField] Vector2 gridBounds = Vector2.one * 10f;

    void Awake()
    {
        gridTiles = new GridTile[gridSize.x, gridSize.y];
        float xSize = gridBounds.x / gridSize.x;
        float ySize = gridBounds.y / gridSize.y;
        for (int x = 0; x < gridSize.x; x++)
        {
            float xOffset = (x + 0.5f) / gridSize.x;
            xOffset -= 0.5f;
            xOffset *= gridBounds.x;
            for (int y = 0; y < gridSize.y; y++)
            {
                float yOffset = (y + 0.5f) / gridSize.y;
                yOffset -= 0.5f;
                yOffset *= gridBounds.y;
                GameObject newTile = new GameObject("Grid Tile " + x + ", " + y);
                newTile.transform.SetParent(transform);
                newTile.transform.localPosition = new Vector3(xOffset, 0f, yOffset);
                BoxCollider tileCollider = newTile.AddComponent<BoxCollider>();
                tileCollider.isTrigger = true;
                tileCollider.size = new Vector3(xSize, gridTileHeight, ySize);
                GridTile gridTile = newTile.AddComponent<GridTile>();
                gridTiles[x, y] = gridTile;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (Application.isPlaying || gridSize.x == 0f || gridSize.y == 0f) return;
        Gizmos.color = Color.red;
        float xSize = gridBounds.x / gridSize.x;
        float ySize = gridBounds.y / gridSize.y;
        for (int x = 0; x < gridSize.x; x++)
        {
            float xOffset = (x + 0.5f) / gridSize.x;
            xOffset -= 0.5f;
            xOffset *= gridBounds.x;
            for (int y = 0; y < gridSize.y; y++)
            {
                float yOffset = (y + 0.5f) / gridSize.y;
                yOffset -= 0.5f;
                yOffset *= gridBounds.y;
                Gizmos.DrawWireCube(transform.position - new Vector3(xOffset, 0f, yOffset), new Vector3(xSize, gridTileHeight, ySize));
            }
        }
    }
}
