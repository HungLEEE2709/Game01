using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerHealth = 10;
    private int score = 0;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject GameOverUI;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject pauseButton;

    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Tự động gọi khi qua màn
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Gán lại các UI nếu bị null sau khi qua màn
        if (scoreText == null)
            scoreText = GameObject.Find("Score")?.GetComponent<TextMeshProUGUI>();
        if (GameOverUI == null)
            GameOverUI = GameObject.Find("GameOverUI");
        if (healthBarUI == null)
            healthBarUI = GameObject.Find("Health");
        if (pauseButton == null)
            pauseButton = GameObject.Find("Button");

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

        Debug.Log("Game Over triggered!");

        // Reset điểm khi thua
        score = 0;
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

    private void OnDestroy()
    {
        // Xóa listener khi GameManager bị phá hủy (để tránh memory leak)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
