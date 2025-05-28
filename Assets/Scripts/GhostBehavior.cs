using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public float chaseDistance = 5f;
    public float moveSpeed = 2f;
    public float returnSpeed = 1.5f;
    public float idleRadius = 0.5f;
    public float idleSpeed = 0.5f;
    public float damageAmount = 25f;
    public float damageInterval = 1f;
    public float idleActivationDistance = 0.1f;
    public AudioSource GhostHit;

    private Transform playerTransform;
    [SerializeField] private PlayerController playerController;
    private Animator animator;
    private Vector2 initialPosition;
    private Rigidbody2D rb2d;

    private bool isReturningToStart = false;
    private float lastDamageTime;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d == null)
        {
            Debug.LogError("Rigidbody2D not found on Ghost!");
            enabled = false;
            return;
        }

        // Set Rigidbody2D properties for proper collision and movement
        rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 0f;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;

        initialPosition = rb2d.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerController = player.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogError("PlayerController not found!");
                enabled = false;
                return;
            }
        }
        else
        {
            Debug.LogError("Player GameObject with 'Player' tag not found!");
            enabled = false;
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator not found on Ghost!");
            enabled = false;
            return;
        }

        lastDamageTime = -damageInterval;
    }

    void FixedUpdate()
    {
        if (playerTransform == null || animator == null || playerController == null || rb2d == null) return;

        Vector2 playerPos2D = playerTransform.position;
        float distanceToPlayer = Vector2.Distance(rb2d.position, playerPos2D);

        if (distanceToPlayer < chaseDistance)
        {
            isReturningToStart = false;
            animator.SetBool("IsChasing", true);
            Vector2 direction = (playerPos2D - rb2d.position).normalized;
            rb2d.linearVelocity = direction * moveSpeed;
        }
        else
        {
            animator.SetBool("IsChasing", false);
            float distanceToStart = Vector2.Distance(rb2d.position, initialPosition);

            if (distanceToStart > idleActivationDistance)
            {
                isReturningToStart = true;
                Vector2 directionToStart = (initialPosition - rb2d.position).normalized;
                rb2d.linearVelocity = directionToStart * returnSpeed;
            }
            else
            {
                if (isReturningToStart)
                {
                    rb2d.position = initialPosition;
                    isReturningToStart = false;
                }

                float angle = Time.time * idleSpeed;
                float x = initialPosition.x + Mathf.Cos(angle) * idleRadius;
                float y = initialPosition.y + Mathf.Sin(angle) * idleRadius;
                Vector2 idlePosition = new Vector2(x, y);
                Vector2 directionToIdle = (idlePosition - rb2d.position).normalized;
                rb2d.linearVelocity = directionToIdle * idleSpeed;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Collider2D otherCollider = collision.collider;
        if (otherCollider.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageInterval)
            {
                if (GhostHit != null) GhostHit.Play();
                HealthSystem health = otherCollider.GetComponent<HealthSystem>();
                if (health != null)
                {
                    health.TakeDamage(damageAmount);
                    lastDamageTime = Time.time;
                    Debug.Log($"Ghost dealt {damageAmount} damage to Player. Player Health: {health.currentHealth}");
                }
                else
                {
                    Debug.LogError("HealthSystem not found on Player during collision!");
                }
            }
        }
    }
}
