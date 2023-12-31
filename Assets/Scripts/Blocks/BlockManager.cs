using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    void Awake()
    {
        // subscribe to Game Restart event
        SuperMarioManager.instance.gameRestart.AddListener(GameRestart);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void GameRestart()
    {
        foreach (Transform child in transform)
        {
            BlockPowerUpController childPowerUpController = child.gameObject.GetComponent<BlockPowerUpController>();

            if (childPowerUpController != null)
            {
                childPowerUpController.GameRestart();
            }
        }
    }
}
