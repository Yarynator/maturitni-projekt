using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BattleInfo")]
public class BattleInfoSO : ScriptableObject
{

    [System.Serializable]
    public struct EnemyData
    {
        public GridPosition gridPosition;
        public string playerType;
        public int attack;
        public int defense;
        public int maxMoveDistance;
        public List<string> actions;
        public int health;
    }

    public int battleScene;
    public List<EnemyData> enemyList;

}
