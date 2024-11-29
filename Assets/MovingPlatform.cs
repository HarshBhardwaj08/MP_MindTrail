using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform pointA; // First point the platform moves to
    public Transform pointB; // Second point the platform moves to
    public float speed = 2f; // Movement speed

    private Vector3 targetPosition; // Current target position

    void Start()
    {
        // Set the initial target position to point A
        if (pointA != null)
            targetPosition = pointA.position;
    }

    void Update()
    {
        // Move the platform towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Check if the platform reached the target position
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            // Switch target to the other point
            if (targetPosition == pointA.position)
                targetPosition = pointB.position;
            else
                targetPosition = pointA.position;
        }
    }

    void OnDrawGizmos()
    {
        // Draw lines to visualize the movement points
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}
