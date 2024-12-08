using UnityEngine;

public class DragonAI : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float attackRange = 5f; // Range to trigger attack
    public float moveSpeed = 2f; // Dragon's move speed
    private Animator animator;
    private Vector3 lastPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position; // Store the initial position
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate distance to player
            float distance = Vector3.Distance(transform.position, player.position);

            // Set PlayerInRange parameter based on distance
            bool inRange = distance <= attackRange;
            animator.SetBool("PlayerInRange", inRange);

            // Check if the dragon is moving
            bool isMoving = Vector3.Distance(transform.position, lastPosition) > 0.1f;
            animator.SetBool("isMoving", isMoving);

            // Update the last position for movement detection
            lastPosition = transform.position;
           
        }
    }
}
