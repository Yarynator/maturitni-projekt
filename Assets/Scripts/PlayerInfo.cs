using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    
    public static PlayerInfo Instance { get; private set; }


    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI defenseText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Button inventoryButton;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("PlayerInfo already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Hide();
    }

    public void Show(Player player)
    {
        gameObject.SetActive(true);

        playerImage.sprite = player.GetSprite();
        nameText.text = "Name: " + player.GetName();
        levelText.text = "Level: " + player.GetLevel();
        attackText.text = "Attack: " + player.GetAttack();
        defenseText.text = "Defense: " + player.GetDefense();
        healthText.text = "Health: " + player.GetHealth().GetHealth() + "/" + player.GetHealth().GetMaxHealth();
        healthBar.localScale = new Vector2(player.GetHealth().GetHealthNormalized(), healthBar.localScale.y);

        inventoryButton.onClick.RemoveAllListeners();
        inventoryButton.onClick.AddListener(() =>
        {
            InventoryUI.Instance.Show(player);
        });
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        inventoryButton.onClick.RemoveAllListeners();
    }

}
