using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{

    private void Start()
    {

        ObjectsData objectsData = SceneInfo.Instance.GetObjectsData();

        switch (SceneInfo.Instance.GetSceneIndex())
        {
            case 2:
                if(objectsData.priestChestIsActive)
                {
                    Item item = Instantiate(Resources.Load<Item>("Chest"), LevelGrid.Instance.GetWorldPosition(new GridPosition(objectsData.priestChestXPosition, objectsData.priestChestYPosition)), Quaternion.identity);
                    item.name = "ChestItem";
                }
                break;
            case 6:
                if(objectsData.priestHelmetIsActive)
                {
                    Instantiate(Resources.Load<Item>("PriestHelmet"), LevelGrid.Instance.GetWorldPosition(new GridPosition(objectsData.priestHelmetXPosition, objectsData.priestHelmetYPosition)), Quaternion.identity);
                }
                break;
        }
    }

}
