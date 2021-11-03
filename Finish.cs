using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour // A script that allows the player to complete a level
{
    public Main main;
    public Sprite finSprite;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = finSprite;
            main.Win();
        }
    }
}