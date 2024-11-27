using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slammer : MonoBehaviour
{
    public float slamSpeed = 20f;        // Speed of the slam
    public float resetDelay = 1f;       // Time before resetting the slammer
    public Transform resetPosition;     // Position to reset the slammer
    public Transform groundPosition;    // Position where the slammer impacts

    private bool isSlamming = true;     // Is the slammer moving down?

    private void Start()
    {
        resetPosition = this.gameObject.transform;
    }
    void Update()
    {
        if (isSlamming)
        {
            // Move down
            transform.position = Vector2.MoveTowards(transform.position, groundPosition.position, slamSpeed * Time.deltaTime);

            // Check if reached ground
            if (Vector2.Distance(transform.position, groundPosition.position) < 0.1f)
            {
                isSlamming = false;
              //  CameraShake.Instance.ShakeCamera(0.2f, 0.5f); // Trigger camera shake
                Invoke(nameof(ResetSlammer), resetDelay);
            }
        }
    }

    void ResetSlammer()
    {
        // Reset position
        transform.position = resetPosition.position;
        isSlamming = true;
    }
}
