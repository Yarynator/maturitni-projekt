using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystemWorldData 
{
    
    public static void SaveData(PlayerData[] playerList, int saveIndex, int sceneIndex, int fromSceneIndex, bool insideBuilding, bool tutorialBattleIsActive, bool priestRestaurantBattleIsActive, ActiveQuestsData questData, ObjectsData objectsData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        if (!Directory.Exists($"{Application.persistentDataPath}/save{saveIndex}"))
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/save{saveIndex}");
        }
        string path = $"{Application.persistentDataPath}/save{saveIndex}/worldData.rpg";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            //GameObject bush = GameObject.Find("TileDoorClosedMiddle");
            WorldData data = new WorldData(playerList.Length, sceneIndex, fromSceneIndex, insideBuilding, tutorialBattleIsActive, priestRestaurantBattleIsActive);

            /*if(SceneInfo.Instance.GetSceneIndex() == 0)
            {
                data = new WorldData(playerList.Length, sceneIndex, fromSceneIndex, insideBuilding, true, tutorialBattleIsActive, priestRestaurantBattleIsActive);
            }
            else if (bush == null)
            {
                data = new WorldData(playerList.Length, sceneIndex, fromSceneIndex, insideBuilding, false, tutorialBattleIsActive, priestRestaurantBattleIsActive);
            }
            else
            {
                data = new WorldData(playerList.Length, sceneIndex, fromSceneIndex, insideBuilding, GameObject.Find("TileDoorClosedMiddle").TryGetComponent<BushDoor>(out BushDoor bushDoor), tutorialBattleIsActive, priestRestaurantBattleIsActive);
            }*/

            formatter.Serialize(stream, data);
            stream.Close();
        }

        int playerIndex = 0;
        foreach (PlayerData player in playerList)
        {
            string playerPath = $"{Application.persistentDataPath}/save{saveIndex}/player{playerIndex}.rpg";

            using (FileStream playerStream = new FileStream(playerPath, FileMode.Create))
            {
                formatter.Serialize(playerStream, player);
                playerStream.Close();
            }
                

            playerIndex++;
        }

        string questsPath = $"{Application.persistentDataPath}/save{saveIndex}/quests.rpg";
        using (FileStream stream = new FileStream(questsPath, FileMode.Create))
        {
            formatter.Serialize(stream, questData);
            stream.Close();
        }

        string objectsPath = $"{Application.persistentDataPath}/save{saveIndex}/objects.rpg";
        using (FileStream stream = new FileStream(objectsPath, FileMode.Create))
        {
            formatter.Serialize(stream, objectsData);
            stream.Close();
        }
    }

    public static WorldData LoadWorldData(int saveIndex)
    {
        string path = $"{Application.persistentDataPath}/save{saveIndex}/worldData.rpg";
        if (File.Exists(path))
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                WorldData data = formatter.Deserialize(stream) as WorldData;
                stream.Close();
                
                return data;
            }
        }
        else
        {
            return null;
        }
    }

    public static PlayerData LoadPlayerData(int saveIndex, int playerIndex)
    {
        string path = $"{Application.persistentDataPath}/save{saveIndex}/player{playerIndex}.rpg";
        if (File.Exists(path))
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                return data;
            }

        }
        else
        {
            return null;
        }
    }

    public static ActiveQuestsData LoadQuestData(int saveIndex)
    {
        string path = $"{Application.persistentDataPath}/save{saveIndex}/quests.rpg";
        if (File.Exists(path))
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                ActiveQuestsData data = formatter.Deserialize(stream) as ActiveQuestsData;
                return data;
            }

        }
        else
        {
            return null;
        }
    }

    public static ObjectsData LoadObjectData(int saveIndex)
    {

        string path = $"{Application.persistentDataPath}/save{saveIndex}/objects.rpg";
        if (File.Exists(path))
        {
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {

                BinaryFormatter formatter = new BinaryFormatter();
                ObjectsData data = formatter.Deserialize(stream) as ObjectsData;
                return data;
            }

        }
        else
        {
            return null;
        }
    }

}
