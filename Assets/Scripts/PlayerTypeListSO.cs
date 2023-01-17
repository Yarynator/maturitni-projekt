using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerTypeList")]
public class PlayerTypeListSO : ScriptableObject
{
    
    public List<string> playerTypeStringList;
    public List<PlayerTypeSO> playerTypeSOList;

}
