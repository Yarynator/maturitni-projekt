using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectsData 
{

    public bool tutorialBushIsDestroyed;

    public bool priestChestIsActive;
    public int priestChestXPosition;
    public int priestChestYPosition;

    public bool priestHelmetIsActive;
    public int priestHelmetXPosition;
    public int priestHelmetYPosition;

    public ObjectsData(bool tutorialBushIsDestroyed, bool priestChestIsActive, GridPosition priestChestPosition, bool priestHelmetIsActive, GridPosition priestHelmetPosition)
    {
        this.tutorialBushIsDestroyed = tutorialBushIsDestroyed;

        this.priestChestIsActive = priestChestIsActive;
        priestChestXPosition = priestChestPosition.x;
        priestChestYPosition = priestChestPosition.y;

        this.priestHelmetIsActive = priestHelmetIsActive;
        priestHelmetXPosition = priestHelmetPosition.x;
        priestHelmetYPosition = priestHelmetPosition.y;
    }

}
