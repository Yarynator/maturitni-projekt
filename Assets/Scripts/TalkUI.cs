using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkUI : MonoBehaviour
{

    private enum State
    {
        FirstTalk,
        TalkIfDone,
        TalkIfNotDone,
        TalkIfPositiveAnswer,
        TalkIfNegativeAnswer,
        TalkIfComeBackAndHasNotAccepted,
        Question

    }

    public static TalkUI Instance { get; private set; }


    [SerializeField] private Transform background;
    [SerializeField] private Transform NPCTransform;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform questionTransform;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button positiveButton;
    [SerializeField] private Button negativeButton;

    [SerializeField] private Image NPCImage;
    [SerializeField] private TextMeshProUGUI NPCName;
    [SerializeField] private TextMeshProUGUI NPCText;

    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI playerText;

    private QuestSO quest;
    private State state;
    private int talkLevel;
    private int talkIndex;
    private bool isChoosing;
    private bool questAlreadyShowed;
    private bool questIsDone;

    private Action onTalkComplete;
    private Action onQuestAccepted;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("TaklUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        button.onClick.AddListener(NextTalk);
        positiveButton.onClick.AddListener(PositiveAnswer);
        negativeButton.onClick.AddListener(NegativeAnswer);

        Hide();
    }

    public void Hide()
    {
        background.gameObject.SetActive(false);
        NPCTransform.gameObject.SetActive(false);
        playerTransform.gameObject.SetActive(false);
        questionTransform.gameObject.SetActive(false);
        button.gameObject.SetActive(false);

        if (questIsDone)
        {
            QuestManager.Instance.RemoveQuest(quest);
        }
    }

    public void Show(QuestSO quest, bool questAlreadyShowed, bool questIsDone, int talkLevel, Sprite playerImage, string playerName, Action onTalkComplete, Action onQuestAccepted)
    {
        this.quest = quest;
        this.NPCImage.sprite = quest.NPCSprite;
        this.NPCName.text = quest.NPCName;
        this.playerImage.sprite = playerImage;
        this.playerName.text = playerName;
        this.questionText.text = quest.question;
        this.questAlreadyShowed = questAlreadyShowed;
        this.questIsDone = questIsDone;
        this.onTalkComplete = onTalkComplete;
        this.onQuestAccepted = onQuestAccepted;

        this.talkLevel = talkLevel;
        this.talkIndex = 0;
        this.isChoosing = false;

        background.gameObject.SetActive(true);
        button.gameObject.SetActive(true);

        if(questAlreadyShowed)
        {
            if(!QuestManager.Instance.GetQuestList().Contains(quest))
            {
                state = State.TalkIfComeBackAndHasNotAccepted;
            }
            else if (questIsDone)
            {
                state = State.TalkIfDone;
            }
            else
            {
                state = State.TalkIfNotDone;
            }
        }
        else
        {
            state = State.FirstTalk;
        }

        CheckStateAndTalk();

        #region showCommented
        /*this.quest = quest;
        this.NPCImage.sprite = quest.NPCSprite;
        this.NPCName.text = npcName;
        this.playerImage.sprite = playerImage;
        this.playerName.text = playerName;
        this.questionText.text = quest.question;
        this.questAlreadyShowed = questAlreadyShowed;
        this.onTalkComplete = onTalkComplete;

        firstTalk = true;
        isChoosing = false;

        background.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        talkIndex = 0;

        if (!questAlreadyShowed)
        {

            TalkSingle talk = quest.talk[talkIndex];
            if (talk.isPlayer)
            {
                playerTransform.gameObject.SetActive(true);
                playerText.text = talk.text;
            }
            else
            {
                NPCTransform.gameObject.SetActive(true);
                NPCText.text = talk.text;
            }
        }
        else
        {
            if (QuestManager.Instance.GetQuestList().Contains(quest))
            {
                TalkSingle talk = quest.talkIfComeBackAndHasntCompletedQuest[talkIndex];
                if (talk.isPlayer)
                {
                    playerTransform.gameObject.SetActive(true);
                    playerText.text = talk.text;
                }
                else
                {
                    NPCTransform.gameObject.SetActive(true);
                    NPCText.text = talk.text;
                }
            }
            else
            {
                TalkSingle talk = quest.talkIfComeBackAndHasntAcceptedQuest[talkIndex];
                if (talk.isPlayer)
                {
                    playerTransform.gameObject.SetActive(true);
                    playerText.text = talk.text;
                }
                else
                {
                    NPCTransform.gameObject.SetActive(true);
                    NPCText.text = talk.text;
                }
            }
        }*/
        #endregion
    }

    private void CheckStateAndTalk()
    {
        bool isEnded = false;
        TalkSingle talk = new TalkSingle();

        switch (state)
        {
            case State.TalkIfDone:
                if(talkIndex >= quest.talkList[talkLevel].talkIfDone.Count)
                {
                    isEnded = true;
                }
                else
                {
                    talk = quest.talkList[talkLevel].talkIfDone[talkIndex];
                }
                break;
            case State.TalkIfNotDone:
                if (talkIndex >= quest.talkList[talkLevel].talkIfNotDone.Count)
                {
                    isEnded = true;
                }
                else
                {
                    talk = quest.talkList[talkLevel].talkIfNotDone[talkIndex];
                }
                break;
            case State.TalkIfPositiveAnswer:
                if (talkIndex >= quest.talkIfAcceptQuest.Count)
                {
                    isEnded = true;
                }
                else
                {
                    talk = quest.talkIfAcceptQuest[talkIndex];
                }
                break;
            case State.TalkIfNegativeAnswer:
                if (talkIndex >= quest.talkIfNotAcceptQuest.Count)
                {
                    isEnded = true;
                }
                else
                {
                    talk = quest.talkIfNotAcceptQuest[talkIndex];
                }
                break;
            case State.TalkIfComeBackAndHasNotAccepted:
                if (talkIndex >= quest.talkIfComeBackAndHasNotAccepted.Count)
                {
                    talkIndex = 0;
                    state = State.Question;
                }
                else
                {
                    talk = quest.talkIfComeBackAndHasNotAccepted[talkIndex];
                }
                break;
            default:
                if(quest.talkList[talkLevel].firstTalkList.Count <= talkIndex)
                {
                    talkIndex = 0;
                    state = State.Question;
                }
                else
                {
                    talk = quest.talkList[talkLevel].firstTalkList[talkIndex];
                }
                break;
        }


        if(isEnded)
        {
            Hide();
            onTalkComplete();
        }
        else
        {
            if(state == State.Question)
            {
                NPCTransform.gameObject.SetActive(false);
                playerTransform.gameObject.SetActive(false);
                questionTransform.gameObject.SetActive(true);
                questionText.text = quest.question;
            }
            else
            {
                Talk(talk);
            }
        }
    }

    private void Talk(TalkSingle talk)
    {
        NPCTransform.gameObject.SetActive(false);
        playerTransform.gameObject.SetActive(false);
        questionTransform.gameObject.SetActive(false);

        if (talk.isPlayer)
        {
            playerTransform.gameObject.SetActive(true);
            NPCTransform.gameObject.SetActive(false);
            playerText.text = talk.text;
        }
        else
        {
            NPCTransform.gameObject.SetActive(true);
            playerTransform.gameObject.SetActive(false);
            NPCText.text = talk.text;
        }

    }

    private void NextTalk()
    {
        if (!isChoosing)
        {
            talkIndex++;
            CheckStateAndTalk();
        }

        #region nextTalkCommented
        /*if (!isChoosing)
        {
            talkIndex++;

            if (!questAlreadyShowed)
            {
                if (firstTalk)
                {
                    if (talkIndex >= quest.talk.Count)
                    {
                        isChoosing = true;

                        playerTransform.gameObject.SetActive(false);
                        NPCTransform.gameObject.SetActive(false);
                        questionTransform.gameObject.SetActive(true);
                    }
                    else
                    {
                        TalkSingle talk = quest.talk[talkIndex];
                        if (talk.isPlayer)
                        {
                            playerTransform.gameObject.SetActive(true);
                            NPCTransform.gameObject.SetActive(false);
                            playerText.text = talk.text;
                        }
                        else
                        {
                            NPCTransform.gameObject.SetActive(true);
                            playerTransform.gameObject.SetActive(false);
                            NPCText.text = talk.text;
                        }
                    }
                }
                else
                {
                    if (isPositiveAnswer)
                    {
                        if (talkIndex >= quest.talkIfPositiveAnswer.Count)
                        {
                            Hide();
                            QuestManager.Instance.AddQuest(quest);
                            onTalkComplete?.Invoke();
                        }
                        else
                        {
                            TalkSingle talk = quest.talkIfPositiveAnswer[talkIndex];
                            if (talk.isPlayer)
                            {
                                playerTransform.gameObject.SetActive(true);
                                NPCTransform.gameObject.SetActive(false);
                                playerText.text = talk.text;
                            }
                            else
                            {
                                NPCTransform.gameObject.SetActive(true);
                                playerTransform.gameObject.SetActive(false);
                                NPCText.text = talk.text;
                            }
                        }
                    }
                    else
                    {
                        if (talkIndex >= quest.talkIfNegativeAnswer.Count)
                        {
                            Hide();
                            onTalkComplete?.Invoke();
                        }
                        else
                        {
                            TalkSingle talk = quest.talkIfNegativeAnswer[talkIndex];
                            if (talk.isPlayer)
                            {
                                playerTransform.gameObject.SetActive(true);
                                NPCTransform.gameObject.SetActive(false);
                                playerText.text = talk.text;
                            }
                            else
                            {
                                NPCTransform.gameObject.SetActive(true);
                                playerTransform.gameObject.SetActive(false);
                                NPCText.text = talk.text;
                            }
                        }
                    }
                }
            }
            else
            {
                if(QuestManager.Instance.GetQuestList().Contains(quest))
                {
                    if (talkIndex >= quest.talkIfComeBackAndHasntCompletedQuest.Count)
                    {
                        Hide();
                        onTalkComplete?.Invoke();
                    }
                    else
                    {
                        TalkSingle talk = quest.talkIfComeBackAndHasntCompletedQuest[talkIndex];
                        if (talk.isPlayer)
                        {
                            playerTransform.gameObject.SetActive(true);
                            NPCTransform.gameObject.SetActive(false);
                            playerText.text = talk.text;
                        }
                        else
                        {
                            NPCTransform.gameObject.SetActive(true);
                            playerTransform.gameObject.SetActive(false);
                            NPCText.text = talk.text;
                        }
                    }
                }
                else
                {
                    if (firstTalk)
                    {
                        if (talkIndex >= quest.talkIfComeBackAndHasntAcceptedQuest.Count)
                        {
                            isChoosing = true;

                            playerTransform.gameObject.SetActive(false);
                            NPCTransform.gameObject.SetActive(false);
                            questionTransform.gameObject.SetActive(true);
                        }
                        else
                        {
                            TalkSingle talk = quest.talkIfComeBackAndHasntAcceptedQuest[talkIndex];
                            if (talk.isPlayer)
                            {
                                playerTransform.gameObject.SetActive(true);
                                NPCTransform.gameObject.SetActive(false);
                                playerText.text = talk.text;
                            }
                            else
                            {
                                NPCTransform.gameObject.SetActive(true);
                                playerTransform.gameObject.SetActive(false);
                                NPCText.text = talk.text;
                            }
                        }
                    }
                    else
                    {
                        if (isPositiveAnswer)
                        {
                            if (talkIndex >= quest.talkIfPositiveAnswer.Count)
                            {
                                Hide();
                                QuestManager.Instance.AddQuest(quest);
                                onTalkComplete?.Invoke();
                            }
                            else
                            {
                                TalkSingle talk = quest.talkIfPositiveAnswer[talkIndex];
                                if (talk.isPlayer)
                                {
                                    playerTransform.gameObject.SetActive(true);
                                    NPCTransform.gameObject.SetActive(false);
                                    playerText.text = talk.text;
                                }
                                else
                                {
                                    NPCTransform.gameObject.SetActive(true);
                                    playerTransform.gameObject.SetActive(false);
                                    NPCText.text = talk.text;
                                }
                            }
                        }
                        else
                        {
                            if (talkIndex >= quest.talkIfNegativeAnswer.Count)
                            {
                                Hide();
                                onTalkComplete?.Invoke();
                            }
                            else
                            {
                                TalkSingle talk = quest.talkIfNegativeAnswer[talkIndex];
                                if (talk.isPlayer)
                                {
                                    playerTransform.gameObject.SetActive(true);
                                    NPCTransform.gameObject.SetActive(false);
                                    playerText.text = talk.text;
                                }
                                else
                                {
                                    NPCTransform.gameObject.SetActive(true);
                                    playerTransform.gameObject.SetActive(false);
                                    NPCText.text = talk.text;
                                }
                            }
                        }
                    }
                }
            }
        }*/
        #endregion
    }

    private void PositiveAnswer()
    {
        questionTransform.gameObject.SetActive(false);
        isChoosing = false;

        talkIndex = 0;

        state = State.TalkIfPositiveAnswer;

        onQuestAccepted();
        QuestManager.Instance.AddQuest(quest);

        TalkSingle talk = quest.talkIfAcceptQuest[talkIndex];
        Talk(talk);

        switch (quest.questIndex)
        {
            case 0:
                SceneInfo.Instance.SetPriestRestaurantIsActive(true);
                break;
        }

        foreach (ItemSO item in quest.itemsNecessaryToAcceptQuest) 
        {
            foreach(ItemSO inventoryItem in PlayerManager.Instance.GetActualPlayer().GetInventory().GetItemsInInventory())
            {
                if(inventoryItem != null)
                {
                    if(inventoryItem.itemType == item.itemType)
                    {
                        PlayerManager.Instance.GetActualPlayer().GetInventory().RemoveItem(inventoryItem);
                    }
                }
            }
        }
    }

    private void NegativeAnswer()
    {
        questionTransform.gameObject.SetActive(false);
        isChoosing = false;

        talkIndex = 0;

        state = State.TalkIfNegativeAnswer;

        TalkSingle talk = quest.talkIfNotAcceptQuest[talkIndex];
        Talk(talk);
    }

}
