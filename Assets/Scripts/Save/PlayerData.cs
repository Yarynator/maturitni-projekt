using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{

    public int xPosition;
    public int yPosition;
    public string playerName;
    public string playerType;
    public int level;
    public int attack;
    public int defense;
    public int maxMoveDistance;
    public int health;
    public int maxHealth;

    public string inventoryHead;
    public string inventoryBody;
    public string inventoryLegs;
    public string inventoryShoes;
    
    public string inventoryItem1;
    public string inventoryItem2;
    public string inventoryItem3;
    public string inventoryItem4;
    public string inventoryItem5;
    public string inventoryItem6;
    public string inventoryItem7;
    public string inventoryItem8;

    public PlayerData(int xPosition, int yPosition, string playerName, string playerType, int level, int attack, int defense, int maxMoveDistance, int health, int maxHealth, ItemSO[] itemArray)
    {
        this.xPosition = xPosition;
        this.yPosition = yPosition;
        this.playerName = playerName;
        this.playerType = playerType;
        this.level = level;
        this.attack = attack;
        this.defense = defense;
        this.maxMoveDistance = maxMoveDistance;
        this.health = health;
        this.maxHealth = maxHealth;

        inventoryHead = itemArray[0] != null ? itemArray[0].itemType.ToString() : "";
        inventoryBody = itemArray[1] != null ? itemArray[1].itemType.ToString() : "";
        inventoryLegs = itemArray[2] != null ? itemArray[2].itemType.ToString() : "";
        inventoryShoes = itemArray[3] != null ? itemArray[3].itemType.ToString() : "";

        inventoryItem1 = itemArray[4] != null ? itemArray[4].itemType.ToString() : "";
        inventoryItem2 = itemArray[5] != null ? itemArray[5].itemType.ToString() : "";
        inventoryItem3 = itemArray[6] != null ? itemArray[6].itemType.ToString() : "";
        inventoryItem4 = itemArray[7] != null ? itemArray[7].itemType.ToString() : "";
        inventoryItem5 = itemArray[8] != null ? itemArray[8].itemType.ToString() : "";
        inventoryItem6 = itemArray[9] != null ? itemArray[9].itemType.ToString() : "";
        inventoryItem7 = itemArray[10] != null ? itemArray[10].itemType.ToString() : "";
        inventoryItem8 = itemArray[11] != null ? itemArray[11].itemType.ToString() : "";
    }

}
