using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundMusicAudio;
    private AudioSource pauseAudio;
    private AudioSource marioDeathAudio;
    private AudioSource gameOverAudio;
    private List<AudioSource> audioSources;
    private bool isRestartButtonPressed = true;

    void Awake()
    {
        // subscribe to events
        SuperMarioManager.instance.gameStart.AddListener(GameStart);
        SuperMarioManager.instance.gamePause.AddListener(GamePause);
        SuperMarioManager.instance.gameResume.AddListener(GameResume);
        SuperMarioManager.instance.gameRestart.AddListener(GameRestart);
        SuperMarioManager.instance.marioDeath.AddListener(MarioDeath);
        SuperMarioManager.instance.gameOver.AddListener(GameOver);
    }

    void GameStart()
    {
        backgroundMusicAudio = this.transform.Find("BackgroundMusicAudio").gameObject.GetComponent<AudioSource>();
        pauseAudio = this.transform.Find("PauseAudio").gameObject.GetComponent<AudioSource>();
        marioDeathAudio = this.transform.Find("MarioDeathAudio").gameObject.GetComponent<AudioSource>();
        gameOverAudio = this.transform.Find("GameOverAudio").gameObject.GetComponent<AudioSource>();

        audioSources = new List<AudioSource>
        {
            backgroundMusicAudio,
            pauseAudio,
            gameOverAudio
        };

        backgroundMusicAudio.Play();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GamePause()
    {
        backgroundMusicAudio.Pause();
        pauseAudio.PlayOneShot(pauseAudio.clip);
    }

    void GameResume()
    {
        backgroundMusicAudio.UnPause();
    }

    void GameRestart()
    {
        isRestartButtonPressed = true;
        StopAllAudio();
        GameStart();
    }

    void MarioDeath()
    {
        marioDeathAudio.Play();
    }

    void GameOver()
    {
        isRestartButtonPressed = false;
        backgroundMusicAudio.Stop();
        StartCoroutine(GameOverCoroutine());
    }

    void StopAllAudio()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
        }
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(2.5f);

        if (!isRestartButtonPressed)
        {
            gameOverAudio.Play();
        }
    }
}
