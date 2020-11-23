using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_control : MonoBehaviour
{
    public float moveSpeed;
    public float zoomSpeed;

    void Awake()
    {
        cam = Camera.main;
    }
    private Camera cam;
    // Start is called before the first frame update
    void Update()
    {
        Move();
        Zoom();
        
    }
    /// <summary>
    /// Moves the camera
    /// </summary>
    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        Vector3 dir = transform.up * zInput + transform.right * xInput;

        transform.position += dir * moveSpeed * Time.deltaTime;

    }

    /// <summary>
    /// Zooms with the mouse wheel
    /// </summary>
    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
    }
    public void FocusOnPosition(Vector3 pos)
    {
        transform.position = pos;
    }
}
