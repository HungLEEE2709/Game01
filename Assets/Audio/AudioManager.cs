using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip enemyAttackClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayMusic();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayMusic()
    {
        musicSource.clip = musicClip;
        musicSource.Play();
    }
    public void PlayJumpSound()
    {
        effectAudioSource.PlayOneShot(jumpClip);
    }
    public void PlayAttackSound()
    {
        effectAudioSource.PlayOneShot(attackClip);
    }
    public void PlayEnemyAttackSound()
    {
        effectAudioSource?.PlayOneShot(enemyAttackClip);
    }
}
