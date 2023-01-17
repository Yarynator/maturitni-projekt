using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public static CameraMovement Instance;


    [SerializeField] private float cameraSpeed = 2f;
    [SerializeField] private Vector2 minCameraPosition = Vector2.zero;
    [SerializeField] private Vector2 maxCameraPosition = Vector2.one;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("CameraMovement already exists!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 newPosition = transform.position;
        newPosition += new Vector3(x, y, 0).normalized * Time.deltaTime * cameraSpeed;
        newPosition.x = Mathf.Clamp(newPosition.x, minCameraPosition.x, maxCameraPosition.x);
        newPosition.y = Mathf.Clamp(newPosition.y, minCameraPosition.y, maxCameraPosition.y);

        transform.position = newPosition;
    }

}
