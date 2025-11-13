using UnityEngine;
using UnityEngine.SceneManagement; // Sahne kontrolü için


public class GameManager : MonoBehaviour 
{
    public static GameManager instance;
    public GameObject restartButton;
    public GameObject gameOverUI;

    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        gameOverUI.SetActive(false);
        restartButton.SetActive(false);
    }

    void Update()
    {
        if (isGameOver && Input.GetKeyDown(KeyCode.R)) // Örneğin R tuşuna basınca restart
        {
            RestartScene();
        }
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return; // tekrar tekrar tetiklenmesin

        isGameOver = true;
        ShowGameOver();
    }

    private void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        restartButton.SetActive(true);
        Time.timeScale = 0f; // Oyunu durdur
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
