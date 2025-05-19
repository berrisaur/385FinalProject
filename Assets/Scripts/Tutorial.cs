using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    [Header("Tutorial Settings")]
    [TextArea(3, 10)]
    public List<string> tutorialPages;
    public float textFadeDuration = 1f;
    public KeyCode nextKey = KeyCode.Space;

    [Header("Player Control")]
    public GameObject player;
    public PlayerController playerController;

    private int currentPage = 0;
    private bool isTextFullyVisible = false;
    private bool isTutorialActive = true;

    void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);

        if (playerController != null)
            playerController.EnableControl(false);
        StartCoroutine(ShowPage(tutorialPages[currentPage]));
    }

    void Update()
    {
        if (!isTutorialActive) return;

        if (Input.GetKeyDown(nextKey) && isTextFullyVisible)
        {
            currentPage++;
            if (currentPage < tutorialPages.Count)
            {
                StartCoroutine(ShowPage(tutorialPages[currentPage]));
            }
            else
            {
                EndTutorial();
            }
        }
    }

    IEnumerator ShowPage(string text)
    {
        isTextFullyVisible = false;
        tutorialText.text = "";
        tutorialText.alpha = 0f;

        float t = 0f;
        tutorialText.text = text;

        while (t < textFadeDuration)
        {
            tutorialText.alpha = Mathf.Lerp(0f, 1f, t / textFadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        tutorialText.alpha = 1f;
        isTextFullyVisible = true;
    }

    void EndTutorial()
    {
        isTutorialActive = false;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (tutorialText != null)
            tutorialText.gameObject.SetActive(false);

        if (playerController != null)
            playerController.EnableControl(true);
    }
}
