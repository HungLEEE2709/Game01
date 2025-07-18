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
            Destroy(gameObject); // Xoá bản copy thừa
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
        {
            Debug.LogError("❌ Không tìm thấy GameOver (Canvas)");
            return;
        }

        // Tìm GameOverUI trong các con của GameOver (kể cả bị ẩn)
        gameOverUI = FindChildByName(gameOverCanvas.transform, "GameOverUI")?.gameObject;
        if (gameOverUI == null)
        {
            Debug.LogError("❌ Không tìm thấy GameOverUI trong GameOver");
        }

        // Tìm FinalScoreText trong GameOverUI
        Transform finalScoreTransform = FindChildByName(gameOverCanvas.transform, "FinalScoreText");
        if (finalScoreTransform != null)
        {
            finalScoreText = finalScoreTransform.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            Debug.LogError("❌ Không tìm thấy FinalScoreText trong GameOver");
        }

        // Tìm ScoreText (không quan trọng nằm ở đâu)
        if (scoreText == null)
            scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();

        if (healthBarUI == null)
            healthBarUI = GameObject.Find("HealthBarUI");

        if (pauseButton == null)
            pauseButton = GameObject.Find("PauseButton");
        // Tìm và gán lại sự kiện cho nút Play Again
        Transform playAgainTransform = FindChildByName(gameOverUI.transform, "PlayAgain");
        if (playAgainTransform != null)
        {
            Button PlayAgain = playAgainTransform.GetComponent<Button>();
            if (PlayAgain != null)
            {
                PlayAgain.onClick.RemoveAllListeners();
                PlayAgain.onClick.AddListener(RestartGame); 
                Debug.Log("✅ Gán lại nút Play Again");
            }
            else
            {
                Debug.LogWarning("⚠️ PlayAgainButton không có component Button!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy PlayAgainButton trong GameOverUI!");
        }

    }

    // Hàm hỗ trợ tìm child theo tên trong cây transform (kể cả object ẩn)
    private Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true)) // true => cả object ẩn
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
        Debug.Log($"[GameManager] +{points} điểm, Tổng: {score}");
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
        Debug.Log($"[GameManager] Dữ liệu đã lưu: Score={score}, HP={playerHealth}");
    }

    private void LoadGameData()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        playerHealth = PlayerPrefs.GetInt("PlayerHealth", 10);
        Debug.Log($"[GameManager] Đã load dữ liệu: Score={score}, HP={playerHealth}");
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
