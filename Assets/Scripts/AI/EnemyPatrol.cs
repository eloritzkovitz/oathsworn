using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] public Transform[] patrolPoints;
    public float waitTime = 1f;

    private NavMeshAgent agent;
    private int currentPoint = 0;
    private float waitTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void Update()
    {
        var ai = GetComponent<EnemyAI>();
        if (ai != null && ai.isChasingPlayer) return;

        if (patrolPoints.Length == 0) return;

        // Set animation speed based on agent velocity
        var animator = GetComponent<Animator>();
        if (animator != null)
        {
            float speed = GetComponent<NavMeshAgent>().velocity.magnitude;
            animator.SetFloat("Speed", speed);
        }

        if (!GetComponent<NavMeshAgent>().pathPending && GetComponent<NavMeshAgent>().remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                currentPoint = (currentPoint + 1) % patrolPoints.Length;
                GetComponent<NavMeshAgent>().SetDestination(patrolPoints[currentPoint].position);
                waitTimer = 0f;
            }
        }
    }
}
