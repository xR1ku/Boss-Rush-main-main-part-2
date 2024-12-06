using UnityEngine;

namespace EnemyAI
{
    public class Enemy : MonoBehaviour
    {
        public Transform player; // Assign the player in the Inspector
        public float moveSpeed = 3f;
        public float attackRange = 2f;
        public float rotationSpeed = 5f; // Speed at which the enemy rotates

        // References to state components
        private IdleState idleState;
        private MoveState moveState;
        private AttackState attackState;

        private MonoBehaviour currentState; // Tracks the active state

        void Start()
        {
            // Get references to the states
            idleState = GetComponent<IdleState>();
            moveState = GetComponent<MoveState>();
            attackState = GetComponent<AttackState>();

            // Start in the Idle state
            SetState(idleState);
        }

        void Update()
        {
            RotateTowardsPlayer(); // Always rotate towards the player
        }

        public void SetState(MonoBehaviour newState)
        {
            if (currentState != null)
                currentState.enabled = false; // Disable the current state

            currentState = newState;
            currentState.enabled = true; // Enable the new state
        }

        private void RotateTowardsPlayer()
        {
            if (player == null) return; // If the player is not assigned, skip

            // Calculate the direction to the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Ignore rotation on the Y-axis
            direction.y = 0;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
