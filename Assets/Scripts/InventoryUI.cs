using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    public static InventoryUI Instance { get; private set; }


    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private Image playerImageTransform;
    [SerializeField] private TextMeshProUGUI playerNameTransform;

    [SerializeField] private Transform armorTransform;
    [SerializeField] private Transform inventoryItemsTransform;

    [SerializeField] private Button button;

    [Header("Armor Images")]
    [SerializeField] private Image headImage;
    [SerializeField] private Image bodyImage;
    [SerializeField] private Image legsImage;
    [SerializeField] private Image shoesImage;

    [Header("Inventory Items")]
    [SerializeField] private Image inventoryItem1Image;
    [SerializeField] private Image inventoryItem2Image;
    [SerializeField] private Image inventoryItem3Image;
    [SerializeField] private Image inventoryItem4Image;
    [SerializeField] private Image inventoryItem5Image;
    [SerializeField] private Image inventoryItem6Image;
    [SerializeField] private Image inventoryItem7Image;
    [SerializeField] private Image inventoryItem8Image;

    private bool isOpen;

    private Inventory playerInventory;

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError("InventoryUI");
            return;
        }
        Instance = this;

        Hide();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isOpen)
            {
                Hide();
            }
        }



        if (EventSystem.current.IsPointerOverGameObject())
        {
            GameObject g = MouseWorld.Instance.GetInventoryItem(out Vector2 screenPosition);

            int index = -1;

            if (g == headImage.transform.parent.gameObject || g == headImage.gameObject)
            {
                index = 0;
            }
            else if (g == bodyImage.transform.parent.gameObject || g == bodyImage.gameObject)
            {
                index = 1;
            }
            else if (g == legsImage.transform.parent.gameObject || g == legsImage.gameObject)
            {
                index = 2;
            }
            else if (g == shoesImage.transform.parent.gameObject || g == shoesImage.gameObject)
            {
                index = 3;
            }
            else if (g == inventoryItem1Image.transform.parent.gameObject || g == inventoryItem1Image.gameObject)
            {
                index = 4;
            }
            else if (g == inventoryItem2Image.transform.parent.gameObject || g == inventoryItem2Image.gameObject)
            {
                index = 5;
            }
            else if (g == inventoryItem3Image.transform.parent.gameObject || g == inventoryItem3Image.gameObject)
            {
                index = 6;
            }
            else if (g == inventoryItem4Image.transform.parent.gameObject || g == inventoryItem4Image.gameObject)
            {
                index = 7;
            }
            else if (g == inventoryItem5Image.transform.parent.gameObject || g == inventoryItem5Image.gameObject)
            {
                index = 8;
            }
            else if (g == inventoryItem6Image.transform.parent.gameObject || g == inventoryItem6Image.gameObject)
            {
                index = 9;
            }
            else if (g == inventoryItem7Image.transform.parent.gameObject || g == inventoryItem7Image.gameObject)
            {
                index = 10;
            }
            else if (g == inventoryItem8Image.transform.parent.gameObject || g == inventoryItem8Image.gameObject)
            {
                index = 11;
            }

            if(index >= 0)
            {
                ItemSO item = playerInventory.GetItemsInInventory()[index];
                
                if(item != null)
                {
                    InventoryTooltipUI.Instance.Show(item, screenPosition);
                }
                else
                {
                    InventoryTooltipUI.Instance.Hide();
                }
            }
            else
            {
                InventoryTooltipUI.Instance.Hide();
            }

        }
    }

    public void Show(Player player)
    {
        playerInventory = player.GetInventory();

        backgroundTransform.gameObject.SetActive(true);
        playerImageTransform.gameObject.SetActive(true);
        playerNameTransform.gameObject.SetActive(true);

        playerImageTransform.sprite = player.GetSprite();
        playerNameTransform.text = player.GetName();

        armorTransform.gameObject.SetActive(true);
        inventoryItemsTransform.gameObject.SetActive(true);

        if(SceneInfo.Instance.IsTutorial()){
            if(SceneInfo.Instance.GetTutorialIndex() == 14) {
                TutorialUI.Instance.AddIndex();
            }
        }


        if (player.GetInventory().GetItemsInInventory()[4] != null)
        {
            inventoryItem1Image.sprite = player.GetInventory().GetItemsInInventory()[4].sprite;
            inventoryItem1Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem1Image.sprite = null;
            inventoryItem1Image.color = new Color(0, 0, 0, 0);
        }

        if (player.GetInventory().GetItemsInInventory()[5] != null)
        {
            inventoryItem2Image.sprite = player.GetInventory().GetItemsInInventory()[5].sprite;
            inventoryItem2Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem2Image.sprite = null;
            inventoryItem2Image.color = new Color(0, 0, 0, 0);
        }

        if (player.GetInventory().GetItemsInInventory()[6] != null)
        {
            inventoryItem3Image.sprite = player.GetInventory().GetItemsInInventory()[6].sprite;
            inventoryItem3Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem3Image.sprite = null;
            inventoryItem3Image.color = new Color(0, 0, 0, 0);
        }

        if (player.GetInventory().GetItemsInInventory()[7] != null)
        {
            inventoryItem4Image.sprite = player.GetInventory().GetItemsInInventory()[7].sprite;
            inventoryItem4Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem4Image.sprite = null;
            inventoryItem4Image.color = new Color(0, 0, 0, 0);
        }

        if (player.GetInventory().GetItemsInInventory()[8] != null)
        {
            inventoryItem5Image.sprite = player.GetInventory().GetItemsInInventory()[8].sprite;
            inventoryItem5Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem5Image.sprite = null;
            inventoryItem5Image.color = new Color(0, 0, 0, 0);
        }

        if (player.GetInventory().GetItemsInInventory()[9] != null)
        {
            inventoryItem6Image.sprite = player.GetInventory().GetItemsInInventory()[9].sprite;
            inventoryItem6Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem6Image.sprite = null;
            inventoryItem6Image.color = new Color(0, 0, 0, 0);
        }

        if (player.GetInventory().GetItemsInInventory()[10] != null)
        {
            inventoryItem7Image.sprite = player.GetInventory().GetItemsInInventory()[10].sprite;
            inventoryItem7Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem7Image.sprite = null;
            inventoryItem7Image.color = new Color(0, 0, 0, 0);
        }

        if (player.GetInventory().GetItemsInInventory()[11] != null)
        {
            inventoryItem8Image.sprite = player.GetInventory().GetItemsInInventory()[11].sprite;
            inventoryItem8Image.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            inventoryItem8Image.sprite = null;
            inventoryItem8Image.color = new Color(0, 0, 0, 0);
        }


        button.gameObject.SetActive(true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            Hide();
        });

        isOpen = true;
    }

    public void Hide()
    {
        backgroundTransform.gameObject.SetActive(false);
        playerImageTransform.gameObject.SetActive(false);
        playerNameTransform.gameObject.SetActive(false);

        armorTransform.gameObject.SetActive(false);
        inventoryItemsTransform.gameObject.SetActive(false);

        button.onClick.RemoveAllListeners();
        button.gameObject.SetActive(false);

        isOpen = false;
    }

    public bool IsOpen()
    {
        return isOpen;
    }

}
