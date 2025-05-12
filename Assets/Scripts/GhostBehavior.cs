using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public float chaseDistance = 5f; // Adjust the distance at which the ghost starts chasing
    public float moveSpeed = 2f;     // Adjust the ghost's movement speed
    public float idleRadius = 0.5f;  // Adjust the radius of the idle circle
    public float idleSpeed = 0.5f;   // Adjust the speed of the idle circle
    public float damageAmount = 25f; // Damage to inflict on contact

    private Transform playerTransform;
    private Animator animator;
    private Vector3 initialPosition; // Store the starting position

    void Start()
    {
        // Store the initial position when the game starts
        initialPosition = transform.position;

        // Find the player GameObject
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

        // Get the Animator component
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
            enabled = false;
        }
    }

    void Update()
    {
        if (playerTransform != null && animator != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer < chaseDistance)
            {
                animator.SetBool("IsChasing", true);
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            else
            {
                animator.SetBool("IsChasing", false);

                // Circular idle movement around the initial position
                float angle = Time.time * idleSpeed;
                float x = initialPosition.x + Mathf.Cos(angle) * idleRadius;
                float y = initialPosition.y + Mathf.Sin(angle) * idleRadius;
                transform.position = new Vector3(x, y, transform.position.z); // Maintain original Z
            }
        }
    }

    // Damage player on contact
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