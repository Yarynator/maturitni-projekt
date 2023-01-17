using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SceneData")]
public class SceneDataSO : ScriptableObject
{

    public int fromScene;
    public int toScene;
    public GridPosition spawnGridPosition;
    public GridPosition touchGridPosition;

}
