using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour // Script for door teleportation
{
    public bool isOpen = false;
    public Transform door;
    public Sprite mid, top;

    public void Unlock()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = mid; // Change sprite door top
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = top; // Change sprite door down
    }

    public void Teleport(GameObject player) // Moving the positions of the transmitted object
    {
        player.transform.position = new Vector3(door.position.x, door.position.y, player.transform.position.z);
    }
}
