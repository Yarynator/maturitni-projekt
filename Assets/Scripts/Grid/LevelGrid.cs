using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{

    public static LevelGrid Instance { get; private set; }


    [SerializeField] private Transform debugGridObject;
    [SerializeField] private int height = 15;
    [SerializeField] private int width = 20;

    private GridSystem<GridObject> grid;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("LevelGrid already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        float cellSize = 1f;

        grid = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));

        bool isDebug = false;
        if (isDebug)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Debug.DrawLine(new Vector2(-cellSize / 2, y * cellSize - cellSize / 2), new Vector2(width * cellSize - cellSize / 2, y * cellSize - cellSize / 2), Color.white, Mathf.Infinity);
                    Debug.DrawLine(new Vector2(x * cellSize - cellSize / 2, -cellSize / 2), new Vector2(x * cellSize - cellSize / 2, height * cellSize - cellSize / 2), Color.white, Mathf.Infinity);
                }
            }

            grid.CreateDebugObjects(debugGridObject);
        }

        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public int GetWidth() => grid.GetWidth();

    public int GetHeight() => grid.GetHeight();

    public Vector2 GetWorldPosition(GridPosition gridPosition)
    {
        return grid.GetWorldPosition(gridPosition);
    }

    public GridPosition GetGridPosition(Vector2 worldPosition)
    {
        return grid.GetGridPosition(worldPosition);
    }

    public Vector2 GetClickedGridWorldPosition(Vector3 worldPosition)
    {
        GridPosition gridPosition = GetGridPosition(worldPosition);
        return GetWorldPosition(gridPosition);
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return grid.GetGridObject(gridPosition);
    }

    public bool IsValidGridPosition(GridPosition gridPosition) => grid.IsValidGridPosition(gridPosition);

}
