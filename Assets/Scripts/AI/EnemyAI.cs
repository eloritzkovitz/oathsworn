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

            if (distanceToPlayer <= attackRange)
            {
                agent.ResetPath();
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
        else
        {
            agent.ResetPath();
            animator.SetFloat("Speed", 0);
        }
    }

    // Triggered when the enemy collides with the player
    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            //GameManager manager = FindFirstObjectByType<GameManager>();
            //if (manager != null)
            //{
            //    manager.TriggerDefeat();
            //}
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
