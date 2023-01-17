using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleInfoUI : MonoBehaviour
{

    public static BattleInfoUI Instance { get; private set; }

    
    [SerializeField] private RectTransform stepsBackground;
    [SerializeField] private Image stepPrefab;
    [SerializeField] private RectTransform stepsParent;

    [SerializeField] private Image attack;

    [SerializeField] private Image magic;

    List<Image> steps;
    private int stepsTraveled;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("BattleInfoUI already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        steps = new List<Image>();
        stepPrefab.gameObject.SetActive(false);
    }

    public void SetupSteps()
    {
        int stepsAmount = BattleManager.Instance.GetStepsRemains();
        int backgroundWidth = stepsAmount * 50 + 50;
        
        Vector2 size = stepsBackground.sizeDelta;
        size.x = backgroundWidth;
        stepsBackground.sizeDelta = size;

        Vector2 position = stepsBackground.anchoredPosition;
        position.x = backgroundWidth / 2;
        stepsBackground.anchoredPosition = position;

        foreach(Image step in steps)
        {
            Destroy(step.gameObject);
        }

        steps = new List<Image>();

        for(int i = 0; i < stepsAmount; i++)
        {
            Image step = Instantiate(stepPrefab, stepsParent);
            step.rectTransform.anchoredPosition = new Vector2(i * 50 + 50, -50);
            step.gameObject.SetActive(true);
            steps.Add(step);
        }

        stepsTraveled = 0;
    }

    public void DisableSteps(int stepsUsed)
    {
        stepsTraveled += stepsUsed;

        for(int i = 0; i < stepsTraveled; i++)
        {
            if (steps.Count - 1 - i >= 0)
            {
                Color color = steps[steps.Count - 1 - i].color;
                color.a = .3f;
                steps[steps.Count - 1 - i].color = color;
            }
        }
    }

    public void SetAttackVisibility(bool isVisible)
    {
        if(isVisible)
        {
            Color color = attack.color;
            color.a = 1f;
            attack.color = color;
        }
        else
        {
            Color color = attack.color;
            color.a = .3f;
            attack.color = color;
        }
    }

    public void SetMagicVisibility(bool isVisible)
    {
        if (isVisible)
        {
            Color color = magic.color;
            color.a = 1f;
            magic.color = color;
        }
        else
        {
            Color color = magic.color;
            color.a = .3f;
            magic.color = color;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
