using UnityEngine;

public class Key : MonoBehaviour
{
    public BoxCollider2D[] spawnZones; // Assign multiple room colliders in Inspector
    public PlayerController player;

    private int maxSpawn = 3;

    void Start()
    {
        RandomizePosition();
    }

    void RandomizePosition()
    {
        if (spawnZones.Length == 0)
        {
            Debug.LogWarning("No spawn zones assigned!");
            return;
        }

        // Pick a random room zone
        BoxCollider2D zone = spawnZones[Random.Range(0, spawnZones.Length)];
        Bounds bounds = zone.bounds;

        // Generate a random position within that zone
        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        transform.position = new Vector3(x, y, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Collision happened");

            if (player.keys < player.keyImages.Length)
                player.keyImages[player.keys].enabled = false;

            player.keys++;

            if (maxSpawn > 1)
            {
                maxSpawn--;
                RandomizePosition();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
