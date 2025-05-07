using UnityEngine;

public class GhostBehavior : MonoBehaviour
{
    public float chaseDistance = 5f; // Adjust the distance at which the ghost starts chasing
    public float moveSpeed = 2f;     // Adjust the ghost's movement speed
    private Transform playerTransform;
    private Animator animator;

    void Start()
    {
        // Find the player GameObject (you might need to adjust this based on your player setup)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player GameObject not found with the 'Player' tag!");
            enabled = false; // Disable the script if no player is found
        }

        // Get the Animator component attached to the ghost
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on this GameObject!");
            enabled = false; // Disable the script if no animator is found
        }
    }

    void Update()
    {
        if (playerTransform != null && animator != null)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            // Check if the player is within the chase distance
            if (distanceToPlayer < chaseDistance)
            {
                // Set the IsChasing parameter to true
                animator.SetBool("IsChasing", true);

                // Move towards the player
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
            else
            {
                // Set the IsChasing parameter to false
                animator.SetBool("IsChasing", false);

                // Simple circular idle movement
                float angle = Time.time * 0.5f;
                float radius = 0.5f;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                transform.localPosition = new Vector2(x, y);
            }
        }
    }
}