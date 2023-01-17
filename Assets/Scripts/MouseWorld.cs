using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseWorld : MonoBehaviour
{
    
    public static MouseWorld Instance { get; private set; }


    [SerializeField] private LayerMask inventoryItemLayerMask;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("MouseWorld already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Vector2 GetWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public GameObject GetInventoryItem(out Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1
        };

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        List<GameObject> list = new List<GameObject>();
        foreach(RaycastResult result in results)
        {
            if (result.gameObject.tag == "inventoryItem")
            {
                list.Add(result.gameObject);
            }
        }

        if(list.Count > 0)
        {
            screenPosition = results[0].screenPosition;
            return results[0].gameObject;
        }

        screenPosition = Vector2.zero;
        return null;
    }

}
