using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMaze : MonoBehaviour
{

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(GameObject.Find("Portal"));
        GameObject.Find("GameController").GetComponent<MazeConstructor>().GenerateNewMaze();
    }
}
