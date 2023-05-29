using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 0f, 20f); // Offset of the camera from the player
    public float turnSpeed = 2f; // Speed of the camera's turn

    private float mouseX; // Mouse X-axis input for horizontal turn
    private float mouseY; // Mouse Y-axis input for vertical turn

    // LateUpdate is called after all Update functions have been called
    void LateUpdate()
    {
        // Get mouse input for camera rotation
        mouseX += Input.GetAxis("Mouse X") * turnSpeed;
        mouseY -= Input.GetAxis("Mouse Y") * turnSpeed;

        // Clamp the vertical rotation to limit the camera's angle
        mouseY = Mathf.Clamp(mouseY, -80f, 80f);

        // Quaternion calculation for camera rotation
        Quaternion rotationX = Quaternion.Euler(0f, mouseX, 0f);
        Quaternion rotationY = Quaternion.Euler(mouseY, 0f, 0f);

        // Calculate the desired position of the camera with rotation applied
        Vector3 rotatedOffset = rotationX * rotationY * offset;
        Vector3 desiredPosition = target.position + rotatedOffset;

        // Smoothly move the camera towards the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.05f);

        // Make the camera look at the player
        transform.LookAt(target);
    }
}