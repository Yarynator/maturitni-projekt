using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldData
{
    
    public int playerAmount;
    public int sceneIndex;
    public int fromSceneIndex;
    public bool insideBuilding;

    public bool tutorialBattleIsActive; //index 0
    public bool priestRestaurantBattleIsActive; //index 1


    public WorldData(int playerAmount, int sceneIndex, int fromSceneIndex, bool insideBuilding, bool tutorialBattleIsActive, bool priestRestaurantBattleIsActive)
    {
        this.playerAmount = playerAmount;
        this.sceneIndex = sceneIndex;
        this.fromSceneIndex = fromSceneIndex;
        this.insideBuilding = insideBuilding;

        this.tutorialBattleIsActive = tutorialBattleIsActive;
        this.priestRestaurantBattleIsActive = priestRestaurantBattleIsActive;
    }
}
