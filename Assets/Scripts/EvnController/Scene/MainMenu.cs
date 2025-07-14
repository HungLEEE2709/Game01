using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject OptionsPanel;

    public Slider volumeSlider;
    public AudioMixer mixer;
    private float value;

    private void Start()
    {
        mixer.GetFloat("volume",out value);
        volumeSlider.value = value;
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Forest");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SetVolume()
    {
        mixer.SetFloat("volume", volumeSlider.value);
    }
}
