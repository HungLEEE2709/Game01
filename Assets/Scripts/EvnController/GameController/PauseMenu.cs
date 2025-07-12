using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused = false;

    private void Update()
    {
            
        // Nhấn Escape để bật/tắt pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
    }

    public void Restart()
    {
        Time.timeScale = 1f; // ✅ Bảo đảm thời gian bình thường
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioListener.pause = false;
    }

    public void Home()
    {
        Time.timeScale = 1f; // ✅ Đảm bảo không bị dừng khi về menu
        SceneManager.LoadScene("Main Menu");
        AudioListener.pause = false;
    }
    public void ToggleMute()
    {
        if (AudioListener.volume > 0f)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
    }
}
