using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickPowerUpController : MonoBehaviour, IPowerUpController
{
    public GameObject brickBlock;
    private Animator blockAnimator;
    private AudioSource blockBumpAudio;

    public GameObject powerUpObject;
    public BasePowerUp powerUp;
    private Animator powerUpAnimator;

    void Start()
    {
        blockAnimator = brickBlock.GetComponent<Animator>();
        blockBumpAudio = brickBlock.GetComponent<AudioSource>();

        powerUpAnimator = powerUpObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && !powerUp.hasSpawned)
        {
            // show disabled sprite
            blockAnimator.SetTrigger("spawned");
            // spawn the powerup
            powerUpAnimator.SetTrigger("spawned");

            powerUp.SpawnPowerup();
        }
        else if (other.gameObject.tag == "Player")
        {
            blockBumpAudio.PlayOneShot(blockBumpAudio.clip);
        }
    }

    // used by animator
    public void Disable()
    {

    }
}