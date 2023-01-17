using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public static QuestManager Instance { get; private set; }


    public static event EventHandler OnQuestListUpdate;

    [SerializeField] private QuestSO priestRestaurantQuest;

    private List<QuestSO> questList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("BattleManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        questList = new List<QuestSO>();
    }

    private void Start()
    {
        ActiveQuestsData questData = SceneInfo.Instance.GetQuestData();
        if (questData.priestRestaurantQuestIsActive)
        {
            AddQuest(priestRestaurantQuest);
        }
    }

    public void AddQuest(QuestSO quest)
    {
        questList.Add(quest);
        OnQuestListUpdate?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveQuest(QuestSO quest)
    {
        questList.Remove(quest);
        OnQuestListUpdate?.Invoke(this, EventArgs.Empty);
    }

    public List<QuestSO> GetQuestList()
    {
        return questList;
    }

}
