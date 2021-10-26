using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    private float moveSpeed;
    private float resetPosZ;
    private float spawnPosZ;

    private float playAreaWidthX = 20f;
    private float playAreaWidthXinteractables = 6f;
    private float sideDecoEdgeX = 12f;

    [SerializeField] private bool sideDecorationLeft;
    [SerializeField] private bool sideDecorationRight;
    
    void Start()
    {
        moveSpeed = LevelManager.levelManager.runningSpeed;
        spawnPosZ = LevelManager.levelManager.objectSpawnPosZ;
        resetPosZ = LevelManager.levelManager.objectResetPosZ;

        // Make sure player can reach the score and enemy objects
        if(GetComponentInChildren<ScoreObject>() != null || GetComponentInChildren<Enemy>() != null)
        {
            playAreaWidthX = playAreaWidthXinteractables;
        }
    }

    
    void Update()
    {
        // Move the object based on running speed
        transform.Translate(Vector3.back * Time.deltaTime * LevelManager.levelManager.runningSpeed);        

        // Reset object position when it's behind the player
        if(transform.position.z < resetPosZ)
        {
            // Randomize x position to create variety
            float randomX;
            if (sideDecorationLeft)
            {
                randomX = Random.Range(-playAreaWidthX, -sideDecoEdgeX);
                transform.position = new Vector3(randomX, transform.position.y, spawnPosZ);
            }
            else if (sideDecorationRight)
            {
                randomX = Random.Range(sideDecoEdgeX, playAreaWidthX);
            }
            else
            {
                randomX = Random.Range(-playAreaWidthX, playAreaWidthX);
            }

            transform.position = new Vector3(randomX, transform.position.y, spawnPosZ);
        }
    }
}
