using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    private ItemSO[] itemsInInventory;

    private void Awake()
    {
        itemsInInventory = new ItemSO[12];
    }

    public bool TryAddItem(ItemSO item)
    {
        for(int i = 4; i < itemsInInventory.Length; i++)
        {
            if(itemsInInventory[i] == null)
            {
                Debug.Log(item.itemType.ToString());
                AddItemAtIndex(item, i);
                return true;
            }
        }

        return false;
    }

    public void AddItemAtIndex(ItemSO item, int index)
    {
        itemsInInventory[index] = item;
    }

    public void RemoveItem(ItemSO item)
    {
        for(int i = 0; i < itemsInInventory.Length; i++)
        {
            if(itemsInInventory[i] == item)
            {
                itemsInInventory[i] = null;
                break;
            }
        }
    }

    public ItemSO[] GetItemsInInventory()
    {
        return itemsInInventory;
    }

}
