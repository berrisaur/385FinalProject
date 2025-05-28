using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal; // Needed for Light2D


[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public int keys = 0;
    int totalKeys = 3;
    public TMP_Text keysLeft;
    private bool canMove = true;

    public Light2D playerLight; // Assign in Inspector
    public float lightExpandAmount = 1.5f; // How much to expand when key is picked

    public Image[] keyImages; // UI key icons
    public ParticleSystem keyParticles; // Assign in Inspector
    public bool hasKey = false; // Prevent multiple pickups
    private AudioSource keySound;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 lastPosition; // To track movement
    private HealthSystem healthSystem;

    public Key keyScript; // Set by Key script at runtime


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        keySound = GetComponent<AudioSource>();
        healthSystem = GetComponent<HealthSystem>();
        lastPosition = rb.position; // Initialize last position

        if (healthSystem != null)
        {
            healthSystem.OnHealthChanged += UpdateLightColor; // Subscribe to health event
            UpdateLightColor(healthSystem.currentHealth, healthSystem.maxHealth); // Initial color set
        }
    }

    void Update()
    {
        if (!canMove) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        keysLeft.text = "Keys left: " + (totalKeys - keys).ToString();

        if (hasKey && !keyParticles.isPlaying)
        {
            keyParticles.Play();
        }
        else if (!hasKey && keyParticles.isPlaying)
        {
            keyParticles.Stop();
        }
    }


    void FixedUpdate()
    {
        if (!canMove) return;

        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
        lastPosition = rb.position; // Update last position after moving
    }

    public bool IsMoving()
    {
        // Check if the player's current position is significantly different from the last recorded position
        return Vector2.Distance(rb.position, lastPosition) > 0.01f; // Adjust threshold if needed
    }

    public void EnableControl(bool enable)
    {
        canMove = enable;
    }


    public void PickupKey()
    {
        if (!hasKey)
        {
            if (keySound != null) keySound.Play();
            hasKey = true;

            if (playerLight != null)
            {
                playerLight.pointLightOuterRadius += lightExpandAmount;
            }

            keyParticles.Play();

            // Heal 50% of max health
            if (healthSystem != null)
            {
                float healAmount = healthSystem.maxHealth * 0.5f;
                healthSystem.Heal(healAmount);
            }
        }
    }


    public void OpenDoor()
    {
        if (hasKey)
        {
            hasKey = false;

            if (keys < keyImages.Length)
                keyImages[keys].enabled = false;

            keys++;
            keyParticles.Stop();

            if (keyScript != null)
            {
                keyScript.OnKeyUsed(); // Tell key to respawn
            }
        }
    }

    // Update Light2D color based on health percentage
    private void UpdateLightColor(float currentHealth, float maxHealth)
    {
        if (playerLight == null || maxHealth <= 0f) return;

        float healthPercent = currentHealth / maxHealth;

        // Interpolate from red (low) to white (full health)
        Color targetColor = Color.Lerp(Color.red, Color.white, healthPercent);
        playerLight.color = targetColor;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(2);
    }
}