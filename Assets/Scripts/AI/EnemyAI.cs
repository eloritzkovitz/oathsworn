using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float runRange = 6f;
    public float attackRange = 2f;    
    public float deathAnimLength = 2f;

    public AudioClip deathSound;
    private AudioSource audioSource;

    private NavMeshAgent agent;
    private Animator animator;
    private bool isAttacking = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (!isAttacking)
            {
                agent.SetDestination(player.position);
                float speed = agent.velocity.magnitude;
                animator.SetFloat("Speed", speed);
 
                if (distanceToPlayer <= runRange)
                {
                    agent.speed = 5.0f; // Faster run for enemy
                }
                else
                {
                    agent.speed = 2.5f; // Normal walk
                }
            }

            if (distanceToPlayer <= attackRange && !isAttacking)
            {
                agent.ResetPath();
                animator.SetFloat("Speed", 0);
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
        }
        else
        {
            agent.ResetPath();
            animator.SetFloat("Speed", 0);
            isAttacking = false;
        }
    }

    // Triggered when the enemy collides with the player
    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            // Play attack animation
            animator.SetTrigger("Attack");
        }
    }

    // Called when the attack animation hits the player
    public void OnAttackAnimationHit()
    {
        if (player == null || isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                playerHealth.ReturnToCheckpoint();
                Debug.Log("Player health reduced!");
            }
        }
    }    

    // Called when the enemy dies
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        animator.SetBool("isAttacking", false);
        animator.SetFloat("Speed", 0);

        if (audioSource && deathSound)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathSound);
        }      
    } 

    // Called when the death animation is complete
    public void OnDeathAnimationComplete()
    {
        Level1Goal goal = FindFirstObjectByType<Level1Goal>();
        if (goal != null)
        {
            goal.OnEnemyKilled();
        }
        Destroy(gameObject);
    }  
}
