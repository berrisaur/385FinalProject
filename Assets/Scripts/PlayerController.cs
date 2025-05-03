using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public int keys = 0;
    int totalKeys = 3;
    public TMP_Text keysLeft;

    private Rigidbody2D rb;
    private Vector2 movement;

    public Image[] keyImages; // UI key icons

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Update UI
        keysLeft.text = "Keys left: " + (totalKeys - keys).ToString();
    }

    void FixedUpdate()
    {
        // Move the player smoothly using physics
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

}
