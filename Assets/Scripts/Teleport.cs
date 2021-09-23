using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] Player player = null;
    [SerializeField] float teleportX = 0f;
    [SerializeField] float teleportY = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player.transform.position = new Vector2(teleportX, teleportY);
    }
}
