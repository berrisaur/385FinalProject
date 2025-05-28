using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{

    public void OnRestartButtonPressed()
    {
        SceneManager.LoadScene("Game");  // Replace with your actual scene name
    }
}