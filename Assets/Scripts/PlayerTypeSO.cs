using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/PlayerType")]
public class PlayerTypeSO : ScriptableObject
{
    
    public string nameString;
    public Sprite sprite;
    public GameObject playerPrefab;

}
