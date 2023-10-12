using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMushroomPowerUp : BasePowerUp
{
    private AudioSource magicMushroomAudio;
    private GameObject magicMushroomBody;

    // setup this object's type
    // instantiate variables
    protected override void Start()
    {
        base.Start(); // call base class Start()
        this.type = PowerUpType.MagicMushroom;

        magicMushroomBody = this.gameObject.transform.Find("MagicMushroomBody").gameObject;
        magicMushroomAudio = magicMushroomBody.GetComponent<AudioSource>();

        // workaround so that the question block and mushroom's colliders do not force
        // the mushroom to be pushed outside of the question block
        this.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
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
        else if (col.gameObject.layer == 10) // else if hitting Pipe, flip travel direction
        {
            if (spawned)
            {
                goRight = !goRight;
                rigidBody.bodyType = RigidbodyType2D.Dynamic;
                rigidBody.AddForce(Vector2.right * 3 * (goRight ? 1 : -1), ForceMode2D.Impulse);
            }
        }
    }

    // interface implementation
    public override void SpawnPowerup()
    {
        spawned = true;
        rigidBody.bodyType = RigidbodyType2D.Dynamic;
        rigidBody.AddForce(Vector2.right * 3, ForceMode2D.Impulse); // move to the right
        magicMushroomAudio.PlayOneShot(magicMushroomAudio.clip);
    }


    // interface implementation
    public override void ApplyPowerup(MonoBehaviour i)
    {
        // TODO: do something with the object
    }
}