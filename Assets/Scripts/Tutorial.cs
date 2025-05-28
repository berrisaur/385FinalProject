using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;

    [Header("Tutorial Settings")]
    [TextArea(3, 10)]
    public List<string> tutorialPages;
    public KeyCode nextKey = KeyCode.Space;

    [Header("Scene Settings")]
    public string nextSceneName;

    private int currentPage = 0;
    private bool isTextFullyVisible = false;
    private bool isTutorialActive = true;

    void Start()
    {
        if (tutorialPanel != null)
            tutorialPanel.SetActive(true);
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
        tutorialText.text = text;
        tutorialText.alpha = 1f;
        yield return null;
        isTextFullyVisible = true;
    }


    void EndTutorial()
    {
        isTutorialActive = false;

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        if (tutorialText != null)
            tutorialText.gameObject.SetActive(false);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName); 
        }
        else
        {
            Debug.LogWarning("Next scene name not set in the TutorialManager.");
        }
    }
}
