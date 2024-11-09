using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexEffect : MonoBehaviour
{
    public GameObject[] backgrounds; // Array of background layers for parallax
    public float[] parallaxEffectMultipliers; // Parallax speed for each background layer
    public Transform player; // The player object the camera should follow
    public Vector2 offset; // The offset for the camera relative to the player (x and y)
    public float smoothSpeed = 0.125f; // Smoothing speed for the camera follow

    private Vector3 previousCamPos; // Previous frame's camera position

    void Start()
    {
        // Store the initial camera position
        previousCamPos = transform.position;

        // Ensure the parallaxEffectMultipliers array matches the backgrounds array length
        if (parallaxEffectMultipliers.Length != backgrounds.Length)
        {
            Debug.LogError("Parallax Effect Multipliers and Backgrounds length mismatch.");
        }
    }
    private void OnEnable()
    {
        FlowChanger.flowDelegate += ChangeVector;
    }

    private void ChangeVector(float camDis, int musicIndex)
    {
        offset.x = camDis;
    }

    private void OnDisable()
    {
        FlowChanger.flowDelegate -= ChangeVector;
    }

    void Update()
    {
        // Parallax effect for backgrounds
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Calculate the parallax based on camera movement and multiplier
            float parallax = (previousCamPos.x - transform.position.x) * parallaxEffectMultipliers[i];

            // Apply the parallax to the background layer's position
            float backgroundTargetPosX = backgrounds[i].transform.position.x + parallax;

            // Move the background layer smoothly
            backgrounds[i].transform.position = new Vector3(backgroundTargetPosX, backgrounds[i].transform.position.y, backgrounds[i].transform.position.z);
        }

        // Update the camera position for the next frame
        previousCamPos = transform.position;
    }

    void LateUpdate()
    {
        // Camera follows the player smoothly with an offset
        if (player != null)
        {
            // Calculate the desired position with the offset
            Vector3 desiredPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }
}
