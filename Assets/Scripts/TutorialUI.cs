using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{

    public static TutorialUI Instance { get; private set;}
    public event EventHandler OnNextTutorialStep;


    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI continueText;
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
        HideAll();

        button.onClick.AddListener(() =>
        {
            AddIndex();
        });
    }

    public void Show(string text)
    {
        background.gameObject.SetActive(true);
        this.text.gameObject.SetActive(true);
        this.text.text = text;
        continueText.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
    }

    public void ShowWithoutButton(string text)
    {
        background.gameObject.SetActive(true);
        this.text.gameObject.SetActive(true);
        this.text.text = text;
    }

    public void HideAll()
    {
        background.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        continueText.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
    }

    public void HideClick(){
        continueText.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
    }

    public void AddIndex()
    {
        int index = SceneInfo.Instance.GetTutorialIndex() + 1;
        SceneInfo.Instance.SetTutorialIndex(index);
        OnNextTutorialStep?.Invoke(this, EventArgs.Empty);
    }

}
