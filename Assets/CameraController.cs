using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotateSpeed = 500.0f;
    private float zoomAmount = 0.0f;

    private float zoomSpeed = 1.0f; // Adjust zoom speed multiplier for scroll
    private float minZoom = -5.0f;
    private float maxZoom = 5.0f;

    

    // Start is called before the first frame update
    void Start()
    {
        // tourManager = FindObjectOfType<TourManager>();
        // if (tourManager == null)
        // {
        //     Debug.LogError("TourManager not found!");
        // }
    }


    // Update is called once per frame
    void Update()
    {

        if (true)
        {
            // Rotate the camera based on left mouse button
            if (Input.GetMouseButton(0))
            {
                float rotationX = Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
                float rotationY = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;

                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x - rotationX, transform.localEulerAngles.y + rotationY, 0);
            }

            // Zoom based on scroll wheel
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0.0f)
            {
                // Adjust zoom speed based on scroll input for more noticeable zoom increments
                float zoomIncrement = Mathf.Sign(scroll) * zoomSpeed;

                // Apply zoom increment while ensuring it stays within minZoom and maxZoom
                zoomAmount = Mathf.Clamp(zoomAmount + zoomIncrement, minZoom, maxZoom);

                // Update camera position based on zoom amount
                Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
            }
        }


    }
    public void ResetCamera()
    {
        transform.localEulerAngles = new Vector3(3, 0, 0);
        zoomAmount = 0.0f;
        Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
    }
}
