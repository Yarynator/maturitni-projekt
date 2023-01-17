using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionsUI : MonoBehaviour
{

    public static ActionsUI Instance { get; private set; }


    [SerializeField] private Transform baseActionPrefab;
    private List<GameObject> baseActionList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("ActionsUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        baseActionList = new List<GameObject>();
    }

    public void Hide()
    {
        foreach(GameObject actionGO in baseActionList)
        {
            Destroy(actionGO);
        }
        baseActionList.Clear();
    }

    public void Show(BaseAction[] actions)
    {
        Hide();
        int index = 0;
        foreach (BaseAction action in actions)
        {
            GameObject actionGO = Instantiate(baseActionPrefab, transform).gameObject;
            actionGO.transform.Find("text").GetComponent<TextMeshProUGUI>().text = action.GetName();
            actionGO.GetComponent<RectTransform>().anchoredPosition = new Vector2(index * 110, 0);
            actionGO.GetComponent<Button>().onClick.AddListener(() =>
            {
                foreach(GameObject actionGameObj in baseActionList)
                {
                    actionGameObj.transform.Find("selected").gameObject.SetActive(false);
                }

                actionGO.transform.Find("selected").gameObject.SetActive(true);
                ActionManager.Instance.SetActualAction(action, PlayerManager.Instance.GetActualPlayer());
            });

            baseActionList.Add(actionGO);
            index++;
        }

        if (actions.Length > 0)
        {
            foreach (GameObject actionGameObj in baseActionList)
            {
                if (actionGameObj == baseActionList[0])
                {
                    actionGameObj.transform.Find("selected").gameObject.SetActive(true);
                }
                else
                {
                    actionGameObj.transform.Find("selected").gameObject.SetActive(false);
                }
            }

            ActionManager.Instance.SetActualAction(actions[0], PlayerManager.Instance.GetActualPlayer());
        }
    }

}
