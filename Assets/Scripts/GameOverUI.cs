using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

    public static GameOverUI Instance { get; private set; }


    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("TutorialUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        background.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

        button.onClick.AddListener(() =>
        {
            string path = $"{Application.persistentDataPath}/save{PlayerPrefs.GetInt("Save")}";

            var dir = new DirectoryInfo(path);
            dir.Delete(true);
            SceneManager.LoadScene(0);
        });
    }

    public void Show()
    {
        Time.timeScale = 0f;

        background.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
    }

}
