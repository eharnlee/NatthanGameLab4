using System.Collections;
using System.Collections.Generic;
using SuperMarioBros;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 25;
    public float maxSpeed = 20;
    public float upSpeed = 15;
    public float deathImpulse = 10;
    public float stompImpulse = 10;
    private bool moving = false;
    private bool jumpedState = false;
    private bool onGroundState = true;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public GameObject enemies;

    public GameManager gameManager;

    // for animation
    public Animator marioAnimator;

    // for audio
    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;

    // state
    [System.NonSerialized]
    public bool alive = true;

    public Transform gameCamera;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.velocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set gameover scene
        gameManager.GameOver(); // replace this with whichever way you triggered the game over screen for Checkoff 1
    }

    // Start is called before the first frame update
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();

        // Set to be 30 FPS
        Application.targetFrameRate = 30;

        marioBody = GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
    }

    // Update is called once per frame
    void Update()
    {
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            // if above goomba, stomp on goomba
            if (marioBody.transform.position.y > other.gameObject.transform.position.y + 0.1)
            {
                // alive
                gameManager.IncreaseScore(1);
                other.gameObject.GetComponent<EnemyMovement>().stomped();

                marioBody.velocity = new Vector2(marioBody.velocity.x, 0);
                marioBody.AddForce(Vector2.up * stompImpulse, ForceMode2D.Impulse);
            }
            // else if not above goomba, die
            else
            {
                marioAnimator.Play("Mario Die");
                marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
                alive = false;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        GameManager.marioPosition = marioBody.position;

        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.velocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.velocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }

    void Move(int value)
    {
        Vector2 movement = new Vector2(value, 0);

        // check if it doesn't go beyond maxSpeed
        if (marioBody.velocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;

        }
    }

    public void GameRestart()
    {
        // reset position
        marioBody.transform.position = new Vector3(-6.5f, 2, 0);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        gameCamera.position = new Vector3(0, 6.5f, -10);
    }
}
