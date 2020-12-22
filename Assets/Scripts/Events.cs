using UnityEngine.SceneManagement;
using UnityEngine;

public class Events : MonoBehaviour
{
    public GameObject quizPanal;
    public GameObject gameOverPanel;

    public void ReplayGame()
    {
        SceneManager.LoadScene("Level");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        PlayerManager.gameOver = false;
        quizPanal.SetActive(true);
    }
}
