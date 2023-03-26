using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSaveUIManager : MonoBehaviour
{

    [SerializeField] private Button[] savesArray;
    [SerializeField] private Button[] removeSaveArray;
    [SerializeField] private Transform mainMenuTransform;
    [SerializeField] private Transform savesContainerTransform;
    [SerializeField] private Transform settingsTransform;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    private void Start()
    {
        savesContainerTransform.gameObject.SetActive(false);
        mainMenuTransform.gameObject.SetActive(true);
        settingsTransform.gameObject.SetActive(false);

        if (!PlayerPrefs.HasKey("Music"))
        {
            PlayerPrefs.SetFloat("Music", .5f);
        }
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        soundSlider.value = PlayerPrefs.GetFloat("Music");

        Debug.Log(Application.persistentDataPath);

        musicSlider.onValueChanged.AddListener((float value) =>
        {
            PlayerPrefs.SetFloat("Music", value);
            MusicManager.Instance.SetMusicVolume(value);
        });
        soundSlider.onValueChanged.AddListener((float value) =>
        {
            PlayerPrefs.SetFloat("Sound", value);
        });

        for (int i = 0; i < savesArray.Length; i++)
        {
            string path = $"{Application.persistentDataPath}/save{i}/worldData.rpg";
            if (File.Exists(path))
            {
                savesArray[i].GetComponentInChildren<TextMeshProUGUI>().text = "Load This Save";
                removeSaveArray[i].gameObject.SetActive(true);
                int index = i;
                removeSaveArray[index].onClick.AddListener(() =>
                {
                    string path = $"{Application.persistentDataPath}/save{index}";

                    var dir = new DirectoryInfo(path);
                    dir.Delete(true);
                    ReloadButtons();
                });
            }
            else
            {
                removeSaveArray[i].gameObject.SetActive(false);
            }

            int saveIndex = i;
            savesArray[i].onClick.AddListener(() =>
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
                    SaveSystemWorldData.SaveData(playerDataArray, PlayerPrefs.GetInt("Save"), 1, -1, false, true, 0, true, false, questsData, objectsData, MusicManager.Instance.GetMusicSaveData());
                    SceneManager.LoadScene(1);
                }
            });
        }
    }

    private void ReloadButtons()
    {
        for (int i = 0; i < savesArray.Length; i++)
        {
            string path = $"{Application.persistentDataPath}/save{i}/worldData.rpg";
            if (File.Exists(path))
            {
                savesArray[i].GetComponentInChildren<TextMeshProUGUI>().text = "Load This Save";
                removeSaveArray[i].gameObject.SetActive(true);
                
            }
            else
            {
                savesArray[i].GetComponentInChildren<TextMeshProUGUI>().text = "Empty Save";
                removeSaveArray[i].gameObject.SetActive(false);
            }
        }
    }

    public void Play()
    {
        mainMenuTransform.gameObject.SetActive(false);
        savesContainerTransform.gameObject.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Back()
    {
        mainMenuTransform.gameObject.SetActive(true);
        savesContainerTransform.gameObject.SetActive(false);
        settingsTransform.gameObject.SetActive(false);
    }

    public void ShowSettings()
    {
        mainMenuTransform.gameObject.SetActive(false);
        savesContainerTransform.gameObject.SetActive(false);
        settingsTransform.gameObject.SetActive(true);
    }

}
