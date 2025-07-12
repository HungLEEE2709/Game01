using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1f; // đảm bảo game tiếp tục
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu"); // sửa tên thành tên scene menu của bạn
    }

    public void QuitGame()
    {
        Application.Quit(); // Chỉ hoạt động sau khi build game
        Debug.Log("Quit Game");
    }
}
