using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BuildingSpawnPosition")]
public class BuildingSpawnPositionSO : ScriptableObject
{

    public GridPosition insideSpawnPosition;
    public GridPosition outsideSpawnPosition;

}
