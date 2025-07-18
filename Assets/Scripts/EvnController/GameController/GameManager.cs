using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Gameplay Data")]
    public int playerHealth = 10;
    private int score = 0;
    private bool isGameOver = false;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject pauseButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeGame();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignUIFromScene();
        SetupUI();

        if (IsFirstLevel())
        {
            ResetGameData();
        }
        else
        {
            LoadGameData();
        }

        UpdateScore();
        isGameOver = false;
    }

    private void AssignUIFromScene()
    {
        GameObject gameOverCanvas = GameObject.Find("GameOver");
        if (gameOverCanvas == null)
            return;

        gameOverUI = FindChildByName(gameOverCanvas.transform, "GameOverUI")?.gameObject;

        Transform finalScoreTransform = FindChildByName(gameOverCanvas.transform, "FinalScoreText");
        if (finalScoreTransform != null)
            finalScoreText = finalScoreTransform.GetComponent<TextMeshProUGUI>();

        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();

        if (healthBarUI == null)
            healthBarUI = GameObject.Find("HealthBarUI");

        if (pauseButton == null)
            pauseButton = GameObject.Find("PauseButton");

        Transform playAgainTransform = FindChildByName(gameOverUI.transform, "PlayAgain");
        if (playAgainTransform != null)
        {
            Button PlayAgain = playAgainTransform.GetComponent<Button>();
            if (PlayAgain != null)
            {
                PlayAgain.onClick.RemoveAllListeners();
                PlayAgain.onClick.AddListener(RestartGame);
            }
        }
    }

    private Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child;
        }
        return null;
    }

    public void AddScore(int points)
    {
        if (isGameOver) return;

        score += points;
        UpdateScore();
    }

    public void GameOver()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = "Điểm của bạn: " + score;

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        ResetGameData();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string sceneName)
    {
        SaveGameData();
        Time.timeScale = 1f;
        score = 0;
        playerHealth = 10;
        isGameOver = false;
        SceneManager.LoadScene(sceneName);
    }

    private void SaveGameData()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.SetInt("PlayerHealth", playerHealth);
        PlayerPrefs.Save();
    }

    private void LoadGameData()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        playerHealth = PlayerPrefs.GetInt("PlayerHealth", 10);
    }

    private void ResetGameData()
    {
        score = 0;
        playerHealth = 10;
        isGameOver = false;
    }

    private bool IsFirstLevel()
    {
        return SceneManager.GetActiveScene().name == "Forest";
    }

    private void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void SetupUI()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);
        ToggleGameplayUI(true);
    }

    private void ToggleGameplayUI(bool show)
    {
        if (scoreText != null) scoreText.gameObject.SetActive(show);
        if (healthBarUI != null) healthBarUI.SetActive(show);
        if (pauseButton != null) pauseButton.SetActive(show);
    }

    private void InitializeGame()
    {
        if (IsFirstLevel())
            ResetGameData();
        else
            LoadGameData();

        isGameOver = false;

        SetupUI();
        UpdateScore();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
