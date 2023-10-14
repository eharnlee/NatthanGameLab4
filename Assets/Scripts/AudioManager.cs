using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource backgroundMusicAudio;
    private AudioSource pauseAudio;
    private AudioSource gameOverAudio;

    void Awake()
    {
        // subscribe to events
        SuperMarioManager.instance.gameStart.AddListener(GameStart);
        SuperMarioManager.instance.gamePause.AddListener(GamePause);
        SuperMarioManager.instance.gameResume.AddListener(GameResume);
        SuperMarioManager.instance.gameRestart.AddListener(GameRestart);
        SuperMarioManager.instance.gameOver.AddListener(GameOver);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GameStart()
    {
        backgroundMusicAudio = this.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        // backgroundMusicAudio = this.transform.Find("BackgroundMusicAudio").gameObject.GetComponent<AudioSource>();
        pauseAudio = this.transform.Find("PauseAudio").gameObject.GetComponent<AudioSource>();
        gameOverAudio = this.transform.Find("GameOverAudio").gameObject.GetComponent<AudioSource>();

        backgroundMusicAudio.Play();
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
        backgroundMusicAudio.Stop();
        GameStart();
    }

    void GameOver()
    {
        backgroundMusicAudio.Stop();
        StartCoroutine(GameOverCoroutine());
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        gameOverAudio.Play();
    }
}
