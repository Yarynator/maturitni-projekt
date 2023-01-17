using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    
    public static SceneInfo Instance { get; private set; }


    [SerializeField] private int sceneIndex;
    [SerializeField] private NPCMoveMapInSceneListSO npcMoveMapInSceneList;
    private int battleIndex;

    private bool insideBuilding;

    private bool tutorialBattleIsActive;
    private bool priestRestaurantBattleIsActive;

    private ActiveQuestsData questData;
    private ObjectsData objectsData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("SceneInfo already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (sceneIndex != 0)
        {
            WorldData worldData = SaveSystemWorldData.LoadWorldData(PlayerPrefs.GetInt("Save"));
            questData = SaveSystemWorldData.LoadQuestData(PlayerPrefs.GetInt("Save"));
            objectsData = SaveSystemWorldData.LoadObjectData(PlayerPrefs.GetInt("Save"));
            for (int i = 0; i < worldData.playerAmount; i++)
            {
                PlayerData playerData = SaveSystemWorldData.LoadPlayerData(PlayerPrefs.GetInt("Save"), i);
            }

            insideBuilding = worldData.insideBuilding;
            tutorialBattleIsActive = worldData.tutorialBattleIsActive;
            priestRestaurantBattleIsActive = worldData.priestRestaurantBattleIsActive;
        }
    }

    public int GetSceneIndex()
    {
        return sceneIndex;
    }

    public int GetBattleIndex()
    {
        return battleIndex;
    }

    public void SetBattleIndex(int battleIndex)
    {
        this.battleIndex = battleIndex;
    }

    public bool GetInsideBuilding()
    {
        return insideBuilding;
    }

    public void SetInsideBuilding(bool value)
    {
        insideBuilding = value;
    }

    public bool TutorialBattleIsActive()
    {
        return tutorialBattleIsActive;
    }

    public void SetTutorialBattleIsActive(bool isActive)
    {
        tutorialBattleIsActive = isActive;
    }

    public bool PriestRestaurantIsActive()
    {
        return priestRestaurantBattleIsActive;
    }

    public void SetPriestRestaurantIsActive(bool isActive)
    {
        priestRestaurantBattleIsActive = isActive;
    }

    public void SetTutorialBushIsDestroyed(bool isDestroyed)
    {
        objectsData.tutorialBushIsDestroyed = isDestroyed;
    }

    public List<NPCMoveMapSO> GetNPCMoveMapList()
    {
        if (npcMoveMapInSceneList == null)
            return null;
        else
            return npcMoveMapInSceneList.moveMapInSceneList;
    }

    public ActiveQuestsData GetQuestData()
    {
        return questData;
    }

    public ObjectsData GetObjectsData()
    {
        return objectsData;
    }

}
