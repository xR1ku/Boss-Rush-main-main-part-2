using UnityEngine;

namespace EnemyAI
{
    public class MoveState : MonoBehaviour
    {
        private Enemy enemy;
        private Animator animator;

        void Awake()
        {
            enemy = GetComponent<Enemy>();
            animator = GetComponent<Animator>(); // Reference the Animator
            enabled = false; // Start with this state disabled
        }

        void OnEnable()
        {
            Debug.Log("Enemy entered Move State");
            animator.SetBool("isMoving", true); // Set the 'isMoving' parameter to true
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, enemy.player.position);

            // Move toward the player
            Vector3 direction = (enemy.player.position - transform.position).normalized;
            transform.position += direction * enemy.moveSpeed * Time.deltaTime;

            // Transition to Attack state if within range
            if (distanceToPlayer <= enemy.attackRange)
            {
                enemy.SetState(GetComponent<AttackState>());
            }
        }

        void OnDisable()
        {
            Debug.Log("Enemy exited Move State");
            animator.SetBool("isMoving", false); // Set the 'isMoving' parameter to false
        }
    }
}
