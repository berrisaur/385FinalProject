using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class Key : MonoBehaviour
{
    public BoxCollider2D gridArea;
    public PlayerController player;

    int maxSpawn = 3;

    void Start()
    {
        RandomizePosition();
    }

    void RandomizePosition(){
        Bounds bounds = this.gridArea.bounds;

        float x = Mathf.Round(Random.Range(bounds.min.x, bounds.max.x));
        
        float y = Mathf.Round(Random.Range(bounds.min.y, bounds.max.y));

        this.transform.position = new Vector3(x, y, 0.0f);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player"){
            Debug.Log("Collision happened");
            // disable the key image
            player.keyImages[player.keys].enabled = false;

            // increment the keys count
            player.keys++;
            
            // only spawn up to 3 keys
            if(maxSpawn > 1){
                RandomizePosition();
                maxSpawn--;
            }
            else{
                // deactivate the key prefab if 3 keys are collected
                Destroy(gameObject);
            }
            
        }
    }

}
