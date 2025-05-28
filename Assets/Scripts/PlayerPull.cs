using UnityEngine;

public class PlayerPull : MonoBehaviour
{
    public KeyCode pullKey = KeyCode.E;
    public KeyCode throwKey = KeyCode.F; // <- New key to throw
    public float pullRange = 1f;
    public float throwForce = 10f; // <- Strength of throw
    private GameObject currentBox;
    private bool isPulling = false;

    void Update()
    {
        if (Input.GetKeyDown(pullKey))
        {
            TryStartPull();
        }

        if (Input.GetKeyUp(pullKey))
        {
            StopPull();
        }

        if (isPulling && currentBox != null)
        {
            Vector2 pullDirection = (transform.position - currentBox.transform.position).normalized;
            currentBox.transform.position = Vector2.MoveTowards(
                currentBox.transform.position,
                transform.position - (Vector3)pullDirection * 1f,
                Time.deltaTime * 5f
            );
        }

        if (isPulling && Input.GetKeyDown(throwKey))
        {
            ThrowObject();
        }
    }

    void TryStartPull()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pullRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Object"))
            {
                currentBox = hit.gameObject;
                isPulling = true;
                break;
            }
        }
    }

    void StopPull()
    {
        isPulling = false;
        currentBox = null;
    }

    void ThrowObject()
    {
        if (currentBox == null) return;

        Rigidbody2D rb = currentBox.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 throwDir = (currentBox.transform.position - transform.position).normalized;
            rb.linearVelocity = throwDir * throwForce;
        }

        StopPull();
    }
}
