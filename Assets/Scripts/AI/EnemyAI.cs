using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float runRange = 6f;
    public float attackRange = 2f;    
    public float attackCooldown = 1f;
    public float deathAnimLength = 2f;

    public AudioClip discoverSound;
    public AudioClip attackSound;
    public AudioClip deathSound;    
    private AudioSource audioSource;

    private NavMeshAgent agent;
    private Animator animator;    
    private bool isDead = false;
    private float attackTimer = 0f;
    private bool hasHitPlayer = false;
    private bool hasPlayedDiscover = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        hasHitPlayer = false;        

        attackSound = Resources.Load<AudioClip>("Audio/SFX/slash-attack");
        discoverSound = Resources.Load<AudioClip>("Audio/SFX/discover");
        deathSound = Resources.Load<AudioClip>("Audio/SFX/death-scream");
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Play discover sound once on discovery
            if (!hasPlayedDiscover && discoverSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(discoverSound);
                hasPlayedDiscover = true;
            }

            agent.SetDestination(player.position);
            float speed = agent.velocity.magnitude;
            animator.SetFloat("Speed", speed);

            if (distanceToPlayer <= runRange)
            {
                agent.speed = 5.0f;
            }
            else
            {
                agent.speed = 2.5f;
            }

            // Attack logic
            if (distanceToPlayer <= attackRange)
            {
                agent.ResetPath();
                animator.SetFloat("Speed", 0);

                if (!hasHitPlayer)
                {
                    attackTimer -= Time.deltaTime;
                    if (attackTimer <= 0f)
                    {
                        animator.SetTrigger("Attack");
                        attackTimer = attackCooldown;
                        hasHitPlayer = true;
                    }
                }
            }
            else
            {
                attackTimer = 0f;
                hasHitPlayer = false;
            }                      
        }
        else
        {
            agent.ResetPath();
            animator.SetFloat("Speed", 0);
            attackTimer = 0f;
            hasHitPlayer = false;
            hasPlayedDiscover = false;
        }
    }

    // Called by Animation Event at the attack "hit" frame
    public void OnAttackAnimationHit()
    {
        if (player == null || isDead) return;

        // Play attack sound only when attack animation hits
        if (attackSound != null && audioSource != null)
            audioSource.PlayOneShot(attackSound);
 
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            animator.SetTrigger("Hit");
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
                playerHealth.ReturnToCheckpoint();
                Debug.Log("Player health reduced!");
            }
        }
    }

    // Called by Animation Event when the enemy dies
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        animator.SetFloat("Speed", 0);

        if (audioSource && deathSound)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathSound);
        }
    }

    // Called by Animation Event when the death animation completes
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
