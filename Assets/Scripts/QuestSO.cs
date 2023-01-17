using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Quest")]
public class QuestSO : ScriptableObject
{

    public string nameString;
    public int questIndex;
    public Sprite NPCSprite;
    public string NPCName;

    public List<TalkField> talkList;
    public List<TalkSingle> talkIfAcceptQuest;
    public List<TalkSingle> talkIfNotAcceptQuest;
    public List<TalkSingle> talkIfComeBackAndHasNotAccepted;
    public string question;
    public List<string> rewardList;
    public List<ItemSO> itemsNecessaryToAcceptQuest;


    /*public List<TalkSingle> talk;
    public string question;
    public List<TalkSingle> talkIfPositiveAnswer;
    public List<TalkSingle> talkIfNegativeAnswer;
    public List<TalkSingle> talkIfComeBackAndHasntCompletedQuest;
    public List<TalkSingle> talkIfComeBackAndHasntAcceptedQuest;
    public List<string> rewardList;*/

}
