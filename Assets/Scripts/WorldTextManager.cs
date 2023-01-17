using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTextManager : MonoBehaviour
{
    
    public static WorldTextManager Instance { get; private set; }


    [SerializeField] private Transform worldTextPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("WorldTextManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void CreateWorldText(Vector2 worldPosition, string text, float lifetime = 2f)
    {
        WorldText worldText = Instantiate(worldTextPrefab, worldPosition, Quaternion.identity).GetComponent<WorldText>();
        worldText.Setup(text, lifetime);
    }

}
