using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour // Script for a button that removes certain blocks 
{
    public GameObject[] block;
    public Sprite buttonDown;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "MarkBox"){
            GetComponent<SpriteRenderer>().sprite = buttonDown;
            GetComponent<BoxCollider2D>().enabled = false;
            foreach (GameObject obj in block)
            {
                Destroy(obj);
            }
        }
    }
}
