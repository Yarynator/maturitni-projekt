using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListUI : MonoBehaviour
{
    
    public static QuestListUI Instance { get; private set; }


    [SerializeField] private GameObject backgroundGO;
    [SerializeField] private Button hamburgerMenu;
    [SerializeField] private GameObject nameGO;
    [SerializeField] private RectTransform questTemplate;

    private List<Transform> questTransformList;

    private bool isClosed;
    private bool isMoving;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("QuestListUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        questTemplate.gameObject.SetActive(false);
        questTransformList = new List<Transform>();

        isClosed = false;
        isMoving = false;

        hamburgerMenu.onClick.AddListener(() =>
        {
            isClosed = !isClosed;
            isMoving = true;

            if(isClosed)
            {
                for (int i = questTransformList.Count - 1; i >= 0; i--)
                {
                    Destroy(questTransformList[i].gameObject);
                    questTransformList.RemoveAt(i);
                }
            }
        });

        Hide();
    }

    private void Start()
    {
        QuestManager.OnQuestListUpdate += QuestManager_OnQuestListUpdate;
    }

    private void Update()
    {
        if(isMoving)
        {
            if(isClosed)
            {
                Vector2 size = backgroundGO.GetComponent<RectTransform>().sizeDelta;
                size -= Vector2.one * Time.deltaTime * 300f;
                backgroundGO.GetComponent<RectTransform>().sizeDelta = size;

                Vector2 pos = nameGO.GetComponent<RectTransform>().anchoredPosition;
                pos.x += Time.deltaTime * 200f;
                if(pos.x <= -195)
                    nameGO.GetComponent<RectTransform>().anchoredPosition = pos;

                if (size.y <= 125)
                    isMoving = false;
            }
            else
            {
                Vector2 size = backgroundGO.GetComponent<RectTransform>().sizeDelta;
                size += Vector2.one * Time.deltaTime * 300f;
                backgroundGO.GetComponent<RectTransform>().sizeDelta = size;

                Vector2 pos = nameGO.GetComponent<RectTransform>().anchoredPosition;
                pos.x -= Time.deltaTime * 200f;
                if (pos.x >= -300)
                    nameGO.GetComponent<RectTransform>().anchoredPosition = pos;

                if (size.y >= 427.5)
                {
                    isMoving = false;

                    List<QuestSO> questList = QuestManager.Instance.GetQuestList();
                    int index = 0;
                    foreach (QuestSO quest in questList)
                    {
                        try
                        {
                            RectTransform questTransform = Instantiate(questTemplate, transform);
                            pos = questTransform.anchoredPosition;
                            pos.y -= index * 100;
                            questTransform.anchoredPosition = pos;
                            questTransform.gameObject.SetActive(true);
                            questTransform.Find("name").GetComponent<TextMeshProUGUI>().text = quest.nameString;
                            questTransform.Find("description").GetComponent<TextMeshProUGUI>().text = quest.talkList[SceneInfo.Instance.GetQuestData().priestRestaurantTalkLevel].description;
                            questTransformList.Add(questTransform);
                        }
                        catch { }

                        index++;
                    }
                }
            }
        }
    }

    private void QuestManager_OnQuestListUpdate(object sender, System.EventArgs e)
    {
        UpdateListUI();
    }

    public void Hide()
    {
        backgroundGO.SetActive(false);
        hamburgerMenu.gameObject.SetActive(false);
        nameGO.SetActive(false);
        foreach(Transform questTransform in questTransformList)
        {
            questTransform.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        try
        {
            backgroundGO.SetActive(true);
            hamburgerMenu.gameObject.SetActive(true);
            nameGO.SetActive(true);
            foreach (Transform questTransform in questTransformList)
            {
                questTransform.gameObject.SetActive(true);
            }
        } catch { }
    }

    public void UpdateListUI()
    {
        List<QuestSO> questList = QuestManager.Instance.GetQuestList();

        for (int i = questTransformList.Count - 1; i >= 0; i--)
        {
            try
            {
                Destroy(questTransformList[i].gameObject);
            }
            catch { Debug.Log("Object can not be destroyed"); }
            questTransformList.RemoveAt(i);
        }

        int index = 0;
        foreach (QuestSO quest in questList)
        {
            try
            {
                RectTransform questTransform = Instantiate(questTemplate, transform);
                Vector2 pos = questTransform.anchoredPosition;
                pos.y -= index * 100;
                questTransform.anchoredPosition = pos;
                questTransform.gameObject.SetActive(true);
                questTransform.Find("name").GetComponent<TextMeshProUGUI>().text = quest.nameString;
                questTransform.Find("description").GetComponent<TextMeshProUGUI>().text = quest.talkList[SceneInfo.Instance.GetQuestData().priestRestaurantTalkLevel].description;
                questTransformList.Add(questTransform);
            }
            catch { }

            index++;
        }

        if (questList.Count > 0)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

}
