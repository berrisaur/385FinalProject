using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject[] locks;
    public Sprite unlockedDoorSprite; // Assign in Inspector
    private int currentLockIndex = 0;

    private AudioSource doorAudio;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        doorAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null && player.hasKey && currentLockIndex < locks.Length)
            {
                GameObject currentLock = locks[currentLockIndex];
                AudioSource lockAudio = currentLock.GetComponent<AudioSource>();

                float lockAudioLength = 0f;

                if (lockAudio != null && lockAudio.clip != null)
                {
                    lockAudio.Play();
                    lockAudioLength = lockAudio.clip.length;
                }

                // ❌ Removed animation
                // ✅ Just wait for sound, then destroy
                StartCoroutine(DestroyAfterDelay(currentLock, lockAudioLength));

                currentLockIndex++;
                player.OpenDoor();

                if (currentLockIndex == locks.Length)
                {
                    StartCoroutine(PlayDoorSequenceAfter(lockAudioLength));
                }
            }
        }
    }

    IEnumerator DestroyAfterDelay(GameObject lockObj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(lockObj);
    }

    IEnumerator PlayDoorSequenceAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        // ✅ Change door sprite
        if (spriteRenderer != null && unlockedDoorSprite != null)
        {
            spriteRenderer.sprite = unlockedDoorSprite;
        }

        // ✅ Play door open sound
        if (doorAudio != null)
        {
            doorAudio.Play();
        }
        SceneManager.LoadScene("Ending Scene");

    }
}
