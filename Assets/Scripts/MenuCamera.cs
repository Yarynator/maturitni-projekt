using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{

    [SerializeField] private Transform[] waypointMap;
    [SerializeField] private float cameraSpeed = 2f;

    private int currentMapIndex;

    private void Start()
    {
        currentMapIndex = 0;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if(Vector2.Distance(waypointMap[currentMapIndex].position, transform.position) < .1f)
        {
            currentMapIndex++;
            if(currentMapIndex >= waypointMap.Length)
                currentMapIndex = 0;
        }

        Vector3 dir = (waypointMap[currentMapIndex].transform.position - transform.position).normalized;
        transform.position += dir * cameraSpeed * Time.deltaTime;
    }

}
