using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/NPCMoveMap")]
public class NPCMoveMapSO : ScriptableObject
{

    public List<GridPosition> moveMap;

}
