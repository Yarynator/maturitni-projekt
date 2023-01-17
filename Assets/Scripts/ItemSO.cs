using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Item")]
public class ItemSO : ScriptableObject
{

    public enum ItemType
    {
        PriestChest,
        PriestHead
    }

    public ItemType itemType;
    public string nameString;
    public string description;
    public Sprite sprite;

}
