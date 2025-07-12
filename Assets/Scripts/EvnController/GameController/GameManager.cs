using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject healthBarUI;     // Nếu bạn có thanh máu
    [SerializeField] private GameObject pauseButton;     // Nếu bạn có nút Pause

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateScore();

        if (GameOverUI != null)
            GameOverUI.SetActive(false);

        if (scoreText != null)
            scoreText.gameObject.SetActive(true);

        if (healthBarUI != null)
            healthBarUI.SetActive(true);

        if (pauseButton != null)
            pauseButton.SetActive(true);
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        Time.timeScale = 0;

        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (GameOverUI != null)
            GameOverUI.SetActive(true);

        Debug.Log("Game Over triggered!");
    }

    public void RestarGame()
    {
        Time.timeScale = 1f;

        if (GameOverUI != null)
            GameOverUI.SetActive(false);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
