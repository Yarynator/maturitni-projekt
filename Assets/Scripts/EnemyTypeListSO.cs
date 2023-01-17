using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/EnemyTypeList")]
public class EnemyTypeListSO : ScriptableObject
{

    public List<EnemyTypeSO> enemyTypeList;

}
