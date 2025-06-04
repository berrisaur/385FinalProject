using UnityEngine;

public class PlayerPull : MonoBehaviour
{
    public KeyCode pullKey = KeyCode.E;
    public float pullRange = 1f;
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
    }

    void TryStartPull()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pullRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Box"))
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
}
