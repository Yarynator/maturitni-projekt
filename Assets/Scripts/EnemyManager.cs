using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public static EnemyManager Instance;


    private List<Enemy> enemyList;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("EnemyManager already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        enemyList = new List<Enemy>();
    }

    public List<Enemy> GetEnemyList()
    {
        return enemyList;
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    public bool RemoveEnemyFromList(Enemy enemy)
    {
        if(enemyList.Contains(enemy))
        {
            enemyList.Remove(enemy);
            return true;
        }

        return false;
    }

}
