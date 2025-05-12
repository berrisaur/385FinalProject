using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public float chaseDistance = 5f;
    public float moveSpeed = 2f;
    public float returnSpeed = 1.5f;   // Speed when returning to initial position
    public float idleRadius = 0.5f;
    public float idleSpeed = 0.5f;
    public float damageAmount = 25f;
    public float idleActivationDistance = 0.1f; // Distance threshold to start idling again

    private Transform playerTransform;
    private Animator animator;
    private Vector3 initialPosition;

    private bool isReturningToStart = false;

    void Start()
    {
        initialPosition = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found with the 'Player' tag!");
            enabled = false;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
            enabled = false;
        }
    }

    void Update()
    {
        if (playerTransform == null || animator == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer < chaseDistance)
        {
            // Chase player
            isReturningToStart = false;
            animator.SetBool("IsChasing", true);

            Vector2 direction = (playerTransform.position - transform.position).normalized;
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("IsChasing", false);

            float distanceToStart = Vector2.Distance(transform.position, initialPosition);

            if (distanceToStart > idleActivationDistance)
            {
                // Move back to original position
                isReturningToStart = true;
                Vector2 directionToStart = (initialPosition - transform.position).normalized;
                transform.Translate(directionToStart * returnSpeed * Time.deltaTime);
            }
            else
            {
                // Snap to initial position and start idle movement
                if (isReturningToStart)
                {
                    transform.position = initialPosition;
                    isReturningToStart = false;
                }

                // Perform idle circular motion
                float angle = Time.time * idleSpeed;
                float x = initialPosition.x + Mathf.Cos(angle) * idleRadius;
                float y = initialPosition.y + Mathf.Sin(angle) * idleRadius;
                transform.position = new Vector3(x, y, transform.position.z);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HealthSystem health = other.GetComponent<HealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
