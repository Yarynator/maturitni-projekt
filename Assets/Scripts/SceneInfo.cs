using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour
{
    
    public static SceneInfo Instance { get; private set; }


    [SerializeField] private int sceneIndex;
    [SerializeField] private NPCMoveMapInSceneListSO npcMoveMapInSceneList;
    private int battleIndex;
    private int fromSceneIndex;

    private bool insideBuilding;

    private bool isTutorial;

    private int tutorialIndex;

    private bool tutorialBattleIsActive;
    private bool priestRestaurantBattleIsActive;

    private ActiveQuestsData questData;
    private ObjectsData objectsData;
    private MusicManager.MusicSaveData musicSaveData;

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
            musicSaveData = new MusicManager.MusicSaveData(worldData.musicType, worldData.musicIndex, worldData.musicTime, worldData.fromSceneIndex);
            for (int i = 0; i < worldData.playerAmount; i++)
            {
                PlayerData playerData = SaveSystemWorldData.LoadPlayerData(PlayerPrefs.GetInt("Save"), i);
            }

            insideBuilding = worldData.insideBuilding;
            tutorialBattleIsActive = worldData.tutorialBattleIsActive;
            priestRestaurantBattleIsActive = worldData.priestRestaurantBattleIsActive;
            isTutorial = worldData.isTutorial;
            tutorialIndex = worldData.tutorialIndex;
            fromSceneIndex = worldData.fromSceneIndex;
        }
    }

    public int GetSceneIndex()
    {
        return sceneIndex;
    }

    public int ComeFromSceneIndex()
    {
        return fromSceneIndex;
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

    public MusicManager.MusicSaveData GetMusicSaveData()
    {
        return musicSaveData;
    }

    public bool IsTutorial(){
        return isTutorial;
    }

    public void SetIsTutorial(bool isTutorial){
        this.isTutorial = isTutorial;
    }

    public int GetTutorialIndex(){
        return tutorialIndex;
    }

    public void SetTutorialIndex(int tutorialIndex){
        this.tutorialIndex = tutorialIndex;
    }

}
