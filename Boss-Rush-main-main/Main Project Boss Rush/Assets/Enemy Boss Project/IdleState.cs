using UnityEngine;

namespace EnemyAI
{
    public class IdleState : MonoBehaviour
    {
        private Enemy enemy;

        void Awake()
        {
            enemy = GetComponent<Enemy>();
            enabled = false; // Start with this state disabled
        }

        void OnEnable()
        {
            Debug.Log("Enemy entered Idle State");
        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(transform.position, enemy.player.position);

            // Transition to Attack state if within attack range
            if (distanceToPlayer <= enemy.attackRange)
            {
                enemy.SetState(GetComponent<AttackState>());
            }
            // Otherwise, transition to Move state
            else if (distanceToPlayer > enemy.attackRange)
            {
                enemy.SetState(GetComponent<MoveState>());
            }
        }

        void OnDisable()
        {
            Debug.Log("Enemy exited Idle State");
        }
    }
}
