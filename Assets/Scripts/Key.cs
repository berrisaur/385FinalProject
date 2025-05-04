using UnityEngine;

public class Key : MonoBehaviour
{
    public BoxCollider2D[] spawnZones;
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

        BoxCollider2D zone = spawnZones[Random.Range(0, spawnZones.Length)];
        Bounds bounds = zone.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        transform.position = new Vector3(x, y, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();

            // Only allow pickup if the player doesn't already have a key
            if (!pc.hasKey)
            {
                pc.PickupKey();

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

}
