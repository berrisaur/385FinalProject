using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public int keys = 0;
    int totalKeys = 3;
    public TMP_Text keysLeft;
    private bool canMove = true;


    public Image[] keyImages; // UI key icons
    public ParticleSystem keyParticles; // Assign in Inspector
    public bool hasKey = false; // Prevent multiple pickups
    private AudioSource keySound; 

    private Rigidbody2D rb;
    private Vector2 movement;
    private HealthSystem healthSystem;

    public Key keyScript; // Set by Key script at runtime


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        keySound = GetComponent<AudioSource>();
        healthSystem = GetComponent<HealthSystem>();

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

    public void RestartGame(){
        
        SceneManager.LoadScene(0);
    }
}
