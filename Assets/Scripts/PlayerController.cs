using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10f;
    public int keys = 0;

    void Update()
    {
        Vector3 pos = transform.position; //change to .move position?????

        if (Input.GetKey(KeyCode.W))
        {
            pos.y += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            pos.y -= speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            pos.x += speed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= speed * Time.deltaTime;
        }

        transform.position = pos;

       
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Key"){
            keys++;
            Destroy(collision.gameObject);
        }
    }
}