using UnityEngine;
using UnityEngine.AI;

public class KnightAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float runRange = 6f;
    public float attackRange = 2f;

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
                agent.speed = 5.0f; // Faster run for knight
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

        float deathAnimDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, deathAnimDuration + 0.5f);
    }
}

