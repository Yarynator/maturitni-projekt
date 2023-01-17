using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private Transform healthBarPrefab;

    private int health;
    private int maxHealth;
    Transform healthBar;

    private void Update()
    {
        if (healthBar != null) {
            if (transform.localScale.x < 0f)
            {
                if(healthBar.parent.localScale.x > 0f)
                {
                    healthBar.parent.localScale = new Vector2(healthBar.parent.localScale.x * -1, healthBar.parent.localScale.y);
                }
            }
            else
            {
                if (healthBar.parent.localScale.x < 0f)
                {
                    healthBar.parent.localScale = new Vector2(healthBar.parent.localScale.x * -1, healthBar.parent.localScale.y);
                }
            }
        }
    }

    public void InitUI()
    {
        healthBar = Instantiate(healthBarPrefab, transform).Find("bar");
        UpdateBar();
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetHealth(int health)
    {
        this.health = health;
    }

    public void AddHealth(int health)
    {
        this.health += health;
        if(this.health > maxHealth)
            this.health = maxHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        this.maxHealth = maxHealth;
    }

    public float GetHealthNormalized()
    {
        return (float)health / maxHealth;
    }

    public void UpdateBar()
    {
        float normalizedHealth = GetHealthNormalized();
        Vector3 scale = healthBar.transform.localScale;
        scale.x = normalizedHealth;
        healthBar.transform.localScale = scale;
    }
}
