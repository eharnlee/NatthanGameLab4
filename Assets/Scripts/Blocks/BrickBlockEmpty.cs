using System.Collections;
using System.Collections.Generic;
using SuperMarioBros;
using UnityEngine;

public class BrickBlockEmpty : MonoBehaviour
{
    public Animator blockAnimator;
    public GameObject brickBlock;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (GameManager.marioPosition.y < brickBlock.transform.position.y)
        {
            blockAnimator.SetTrigger("hitBlock");
        }
    }
}
