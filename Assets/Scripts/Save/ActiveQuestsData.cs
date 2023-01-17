using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActiveQuestsData
{

    public bool priestRestaurantQuestIsActive;
    public int priestRestaurantTalkLevel;

    public bool priestRestaurantShowed;
    public bool priestRestaurantAlreadyShowed;
    public bool priestRestarurantIsDone;

    public ActiveQuestsData(bool priestRestaurantQuestIsActive, bool priestRestaurantAlreadyShowed, bool priestRestarurantIsDone, int priestRestaurantTalkLevel, bool priestRestaurantShowed)
    {
        this.priestRestaurantQuestIsActive = priestRestaurantQuestIsActive;
        this.priestRestaurantAlreadyShowed = priestRestaurantAlreadyShowed;
        this.priestRestarurantIsDone = priestRestarurantIsDone;
        this.priestRestaurantTalkLevel = priestRestaurantTalkLevel;
        this.priestRestaurantShowed = priestRestaurantShowed;
    }

}
