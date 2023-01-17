using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangePlace : MonoBehaviour
{

    [SerializeField] private Collider2D changePlaceCollider;
    [SerializeField] private int placeSceneIndex;
    [SerializeField] private string placeName;
    [SerializeField] private SceneDataSO sceneData;
    /*[SerializeField] private GridPosition nearestStartGridPosition;
    [SerializeField] private GridPosition endGridPosition;*/

    public bool IsThisCollider(Collider2D collider)
    {
        return collider == changePlaceCollider;
    }

    public string GetPlaceName()
    {
        return placeName;
    }

    /*public GridPosition GetNearestStartGridPosition()
    {
        return nearestStartGridPosition;
    }

    public GridPosition GetEndGridPosition()
    {
        return endGridPosition;
    }*/

    public SceneDataSO GetSceneData()
    {
        return sceneData;
    }

    public int GetSceneIndex()
    {
        return placeSceneIndex;
    }

}
