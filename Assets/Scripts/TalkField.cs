using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TalkField
{

    public List<TalkSingle> firstTalkList;
    public List<TalkSingle> talkIfDone;
    public List<TalkSingle> talkIfNotDone;
    public string description;

}
