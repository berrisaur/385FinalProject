using UnityEngine;

public class Key : MonoBehaviour
{
    public BoxCollider2D[] spawnZones;
    public PlayerController player;

    private int maxSpawn = 3;

    void Start()
    {
        if (player != null)
        {
            player.keyScript = this; // Link this script to the player
        }
        RandomizePosition();
    }

    void RandomizePosition()
    {
        if (spawnZones.Length == 0)
        {
            Debug.LogWarning("No spawn zones assigned!");
            return;
        }

        BoxCollider2D zone = spawnZones[Random.Range(0, spawnZones.Length)];
        Bounds bounds = zone.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        transform.position = new Vector3(x, y, 0f);
        gameObject.SetActive(true); // Reactivate when ready
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

            if (!pc.hasKey)
            {
                pc.PickupKey();
                gameObject.SetActive(false); // Temporarily hide the key
            }
        }
    }

    // Called by the player when the door is opened
    public void OnKeyUsed()
    {
        if (maxSpawn > 1)
        {
            maxSpawn--;
            RandomizePosition(); // Waited respawn
        }
        else
        {
            Destroy(gameObject); // No more spawns left
        }
    }
}
