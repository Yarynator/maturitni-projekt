using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{

    private Entity entity;

    private GridSystem<GridObject> gridSystem;
    private GridPosition gridPosition;

    public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString() + "\n" + entity;
    }

    public Entity GetEntity()
    {
        return entity;
    }

    public void SetEntity(Entity entity)
    {
        this.entity = entity;
        if(entity is Player player)
            player?.SetGridPosition(this.gridPosition);
    }

    public GridPosition GetGridPosition()
    {
        return this.gridPosition;
    }

}
