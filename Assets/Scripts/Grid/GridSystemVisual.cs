using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{

    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeColor
    {
        public GridVisualType gridVisualType;
        public Color color;
    }

    public enum GridVisualType
    {
        Move,
        Attack,
        Fireball,
        Interact,
        Cookie,
    }


    [SerializeField] private List<GridVisualTypeColor> gridVisualTypeColorList;
    [SerializeField] private Transform gridSystemVisualSinglePrefab;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private GridVisualType lastGridVisualType;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("GridSystemVisual already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetHeight()];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++) 
        {
            for (int y = 0; y < LevelGrid.Instance.GetHeight(); y++) 
            {
                GridPosition gridPosition = new GridPosition(x, y);
                gridSystemVisualSingleArray[x, y] = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity).GetComponent<GridSystemVisualSingle>();
            }
        }

        HideAllGridPosition();
    }

    public void HideAllGridPosition()
    {
        foreach(GridSystemVisualSingle gridSystemVisualSingle in gridSystemVisualSingleArray)
        {
            gridSystemVisualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        if (PlayerManager.Instance.GetActualPlayer() != null)
        {
            foreach (GridPosition gridPosition in gridPositionList)
            {
                gridSystemVisualSingleArray[gridPosition.x, gridPosition.y].Show(GetGridVisualTypeColor(gridVisualType));
            }
            lastGridVisualType = gridVisualType;
        } 
        else
        {
            HideAllGridPosition();
        }
    }

    public void ReloadGridPositionList(List<GridPosition> gridPositionList)
    {
        HideAllGridPosition();
        ShowGridPositionList(gridPositionList, lastGridVisualType);
    }

    private Color GetGridVisualTypeColor(GridVisualType gridVisualType)
    {
        foreach(GridVisualTypeColor gridVisualTypeColor in gridVisualTypeColorList)
        {
            if(gridVisualTypeColor.gridVisualType == gridVisualType)
                return gridVisualTypeColor.color;
        }

        Debug.LogError("Could not find GridVisualTypeColor for GridVisualType " + gridVisualType);
        return new Color();
    }

}
