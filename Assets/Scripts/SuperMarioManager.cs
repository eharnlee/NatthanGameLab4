using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SuperMarioManager : Singleton<SuperMarioManager>
{
    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    public static Vector3 marioPosition;
    private GameObject marioBody;
    private int score = 0;
    public AudioMixer audioMixer;
    private AudioMixerSnapshot audioMixerDefaultSnapshot;
    private float specialEventsPitch = 0.95f;

    public IntVariable gameScore;

    void Start()
    {
        gameStart.Invoke();
        Time.timeScale = 1.0f;
        audioMixerDefaultSnapshot = audioMixer.FindSnapshot("Default");
        audioMixerDefaultSnapshot.TransitionTo(0.1f);

        // subscribe to scene manager scene change
        SceneManager.activeSceneChanged += SceneSetup;

        marioBody = GameObject.Find("Mario");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SceneSetup(Scene current, Scene next)
    {
        gameStart.Invoke();
        SetScore(score);
    }

    public void GameRestart()
    {
        // reset score
        score = 0;
        SetScore(score);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        ResetAudioMixerSpecialEventsPitch();
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        SetScore(score);
        IncreaseAudioMixerSpecialEventsPitch();
    }

    public void IncreaseAudioMixerSpecialEventsPitch()
    {
        specialEventsPitch += 0.025f;
        audioMixer.SetFloat("SpecialEventsPitch", specialEventsPitch);
    }

    public void SmallMarioPowerUp()
    {
        marioBody.GetComponent<PlayerMovement>().SmallMarioPowerUp();
    }


    public void ResetAudioMixerSpecialEventsPitch()
    {
        specialEventsPitch = 0.975f;
        audioMixer.SetFloat("SpecialEventsPitch", 1f);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
    }
}