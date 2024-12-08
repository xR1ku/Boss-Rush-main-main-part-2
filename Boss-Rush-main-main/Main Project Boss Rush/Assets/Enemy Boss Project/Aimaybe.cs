using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform target; // The object you want to check the distance to
    public float range = 5f; // The range within which the player should start walking
    public Animator animator; // Reference to the Animator component

    void Update()
    {
        // Calculate the distance between the player and the target
        float distance = Vector3.Distance(transform.position, target.position);

        // Check if the target is within range
        if (distance <= range)
        {
            // Trigger the walk animation
            animator.SetBool("isWalking", true);
        }
        else
        {
            // Stop the walk animation if out of range
            animator.SetBool("isWalking", false);
        }
    }
}
