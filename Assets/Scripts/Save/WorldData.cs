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

    public bool isTutorial;

    public int tutorialIndex;

    public bool tutorialBattleIsActive; //index 0
    public bool priestRestaurantBattleIsActive; //index 1


    public WorldData(int playerAmount, int sceneIndex, int fromSceneIndex, bool insideBuilding, bool isTutorial, int tutorialIndex, bool tutorialBattleIsActive, bool priestRestaurantBattleIsActive)
    {
        this.playerAmount = playerAmount;
        this.sceneIndex = sceneIndex;
        this.fromSceneIndex = fromSceneIndex;
        this.insideBuilding = insideBuilding;
        this.isTutorial = isTutorial;
        this.tutorialIndex = tutorialIndex;

        this.tutorialBattleIsActive = tutorialBattleIsActive;
        this.priestRestaurantBattleIsActive = priestRestaurantBattleIsActive;
    }
}
