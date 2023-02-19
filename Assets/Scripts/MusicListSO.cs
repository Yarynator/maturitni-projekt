using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/MusicList")]
public class MusicListSO : ScriptableObject
{
    public int musicType;
    public List<MusicSO> list;
}
