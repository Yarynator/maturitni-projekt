using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyType")]
public class EnemyTypeSO : ScriptableObject
{

    public string nameString;
    public Transform enemyPrefab;

}
