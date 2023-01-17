using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BattleInfoList")]
public class BattleInfoListSO : ScriptableObject
{
    
    public List<BattleInfoSO> battleInfoList;

}
