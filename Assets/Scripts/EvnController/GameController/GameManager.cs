using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerHealth = 10;
    private int score = 0;


    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject pauseButton;

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

    private void Start()
    {
        UpdateScore();
        SetupUI();
    }

    private void SetupUI()
    {
        if (GameOverUI != null)
            GameOverUI.SetActive(false);

        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
            UpdateScore();
        }

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
        if (isGameOver)
            return;

        isGameOver = true;
        Time.timeScale = 0;

        if (scoreText != null)
            scoreText.gameObject.SetActive(false);

        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (GameOverUI != null)
            GameOverUI.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Điểm của bạn: " + score.ToString();

        Debug.Log("Game Over triggered!");
    }

    public void RestarGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
