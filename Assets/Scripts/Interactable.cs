using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{

    public enum InteractableType
    {
        PriestRestaurant,
        RestaurantBuilding,
        LeaveRestaurant,
        PriestChest,
        PriestHelmet
    }

    public abstract InteractableType GetInteractableType();

}
