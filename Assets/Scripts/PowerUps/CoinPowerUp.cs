using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPowerUp : BasePowerUp
{
    private AudioSource coinAudio;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerUpType.Coin;

        coinAudio = this.gameObject.GetComponent<AudioSource>();
    }

    void playCoinSound()
    {
        coinAudio.PlayOneShot(coinAudio.clip);
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;
        playCoinSound();
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
    }
}