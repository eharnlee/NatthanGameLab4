using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerUp : BasePowerUp
{
    private AudioSource magicMushroomAudio;

    private Vector2 velocity;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerUpType.MagicMushroom;

        // magicMushroomBody = this.gameObject.transform.Find("MagicMushroomBody").gameObject;
        magicMushroomAudio = this.transform.Find("MagicMushroomAppearAudio").GetComponent<AudioSource>();

        // for the mushroom to remain stationary while BoxCollider2D is inactive
        powerUpRigidBody.bodyType = RigidbodyType2D.Static;
        powerUpCollider.enabled = false;

        velocity = new Vector2(3.0f, 0f);
    }

    void Update()
    {
        if (spawned)
        {
            MoveMushroom();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && spawned)
        {
            // TODO: do something when colliding with Player
            SuperMarioManager.instance.SmallMarioPowerUp();

            // then destroy powerup (optional)
            DestroyPowerup();
        }
        else if (col.gameObject.layer == 7) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                moveRight *= -1;
            }
        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        magicMushroomAudio.PlayOneShot(magicMushroomAudio.clip);
        StartCoroutine(WaitForSpawn());
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
    }

    IEnumerator WaitForSpawn()
    {
        yield return new WaitForSecondsRealtime(1f);

        spawned = true;
        powerUpRigidBody.bodyType = RigidbodyType2D.Dynamic;
        powerUpCollider.enabled = true;
    }

    public void MoveMushroom()
    {
        powerUpRigidBody.MovePosition(powerUpRigidBody.position + velocity * moveRight * Time.fixedDeltaTime);
    }
}