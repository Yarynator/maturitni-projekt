using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSaveUIManager : MonoBehaviour
{

    [SerializeField] private Transform canvas;

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        Button[] buttonArray = canvas.GetComponentsInChildren<Button>();

        for (int i = 0; i < buttonArray.Length; i++)
        {
            string path = $"{Application.persistentDataPath}/save{i}/worldData.rpg";
            if (File.Exists(path))
            {
                buttonArray[i].GetComponentInChildren<TextMeshProUGUI>().text = "Load This Save";
            }

            int saveIndex = i;
            buttonArray[i].onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("Save", saveIndex);
                if (File.Exists(path)) 
                {
                    SceneManager.LoadScene(SaveSystemWorldData.LoadWorldData(saveIndex).sceneIndex);
                }
                else
                {

                    PlayerData[] playerDataArray = new PlayerData[2];

                    
                    playerDataArray[0] = new PlayerData(6, 3, "Stanny", "Stanny", 1, 1, 3, 7, 50, 50, new ItemSO[12]);
                    playerDataArray[1] = new PlayerData(6, 4, "Hurix", "Hurix", 1, 2, 1, 7, 40, 40, new ItemSO[12]);


                    ActiveQuestsData questsData = new ActiveQuestsData(false, false, false, 0, true);
                    ObjectsData objectsData = new ObjectsData(false, false, new GridPosition(), false, new GridPosition());
                    SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"), 1, -1, false, true, false, questsData, objectsData);
                    SceneManager.LoadScene(1);
                }
            });
        }
    }

}
