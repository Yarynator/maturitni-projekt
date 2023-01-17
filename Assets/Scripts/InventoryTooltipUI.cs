using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryTooltipUI : MonoBehaviour
{

    public static InventoryTooltipUI Instance { get; private set; }


    [SerializeField] private Transform background;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("InventoryTooltipUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Hide();
    }

    public void Show(ItemSO item, Vector2 canvasPosition)
    {
        background.gameObject.SetActive(true);
        nameText.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);

        nameText.text = item.nameString;
        descriptionText.text = item.description;

        GetComponent<RectTransform>().localPosition = canvasPosition - new Vector2(Screen.width / 2, Screen.height / 2);
    }

    public void Hide()
    {
        background.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }

}
