using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
// using UnityEngine.SceneManagement;

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

    private SuperMarioManager gameManager;

    // for animation
    private Animator marioAnimator;

    // for audio
    private AudioSource marioJumpAudio;
    private AudioSource marioDeathAudio;
    private AudioSource smallMarioPowerUpAudio;

    // state
    [System.NonSerialized]
    public bool alive = true;

    private Transform gameCamera;

    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);

    void Awake()
    {
        // subscribe to Game Restart event
        SuperMarioManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = SuperMarioManager.instance;
        marioSprite = GetComponent<SpriteRenderer>();

        // Set to be 30 FPS
        Application.targetFrameRate = 30;

        marioBody = GetComponent<Rigidbody2D>();

        // update animator state
        marioAnimator = this.gameObject.GetComponent<Animator>();
        marioAnimator.SetBool("onGround", onGroundState);

        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        marioJumpAudio = this.transform.Find("MarioJumpAudio").GetComponent<AudioSource>();
        marioDeathAudio = this.transform.Find("MarioDeathAudio").GetComponent<AudioSource>();
        smallMarioPowerUpAudio = this.transform.Find("SmallMarioPowerUpAudio").GetComponent<AudioSource>();
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
                marioAnimator.Play("Small Mario Die");
                // marioDeathAudio.PlayOneShot(marioDeathAudio.clip);
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
        SuperMarioManager.marioPosition = marioBody.position;

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
    void PlayJumpSound()
    {
        // play jump sound
        marioJumpAudio.PlayOneShot(marioJumpAudio.clip);
    }

    void PlayDeathImpulse()
    {
        marioBody.velocity = Vector2.zero;
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
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

    void GameOverScene()
    {
        // stop time
        Time.timeScale = 0.0f;
        // set gameover scene
        gameManager.GameOver(); // replace this with whichever way you triggered the game over screen for Checkoff 1
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
        gameCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
        gameCamera.position = new Vector3(0, 6.5f, -10);
    }

    public void SmallMarioPowerUp()
    {
        smallMarioPowerUpAudio.PlayOneShot(smallMarioPowerUpAudio.clip);
        marioAnimator.Play("Small Mario Power Up");
    }
}
