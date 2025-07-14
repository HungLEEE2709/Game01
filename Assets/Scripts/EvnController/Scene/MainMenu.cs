using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject OptionsPanel;
    public void LoadGame()
    {
        SceneManager.LoadScene("Forest");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
