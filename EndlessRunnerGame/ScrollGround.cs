using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollGround : MonoBehaviour
{
    MeshRenderer mesh;
    private float scrollSpeed;
    private float offset;
    private float speedMultiplier = 0.1f; // To match the speed

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        scrollSpeed = LevelManager.levelManager.runningSpeed * speedMultiplier;
    }

    
    void Update()
    {
        // Set the mesh offset based on running speed so that it appears scrolling
        offset += Time.deltaTime * LevelManager.levelManager.runningSpeed * speedMultiplier;
        mesh.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
    }
}
