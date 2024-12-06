using UnityEngine;

namespace EnemyAI
{
    public class AttackState : MonoBehaviour
    {
        private Enemy enemy;
        private float attackCooldown = 2f; // Time between attacks
        private float lastAttackTime = 0f;

        void Awake()
        {
            enemy = GetComponent<Enemy>();
            enabled = false; // Start with this state disabled
        }

        void OnEnable()
        {
            Debug.Log("Enemy entered Attack State");
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, enemy.player.position);

            // Transition to Move state if the player moves out of range
            if (distanceToPlayer > enemy.attackRange)
            {
                enemy.SetState(GetComponent<MoveState>());
                return;
            }

            // Perform attack if cooldown is over
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }

        void OnDisable()
        {
            Debug.Log("Enemy exited Attack State");
        }

        private void Attack()
        {
            Debug.Log("Enemy attacks the player!");
            // Add damage logic here
        }
    }
}
