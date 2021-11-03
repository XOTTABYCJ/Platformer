using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCarPet : MonoBehaviour // Script for a platform moving in contact with a player
{
    public Transform left, right;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            RaycastHit2D leftWall = Physics2D.Raycast(left.position, Vector2.left, 0.3f);
            RaycastHit2D rightWall = Physics2D.Raycast(right.position, Vector2.right, 0.3f);

            if ( ((Input.GetAxis("Horizontal") > 0) && !rightWall.collider && (collision.transform.position.x > transform.position.x)) || //Checking that the platform stops as soon as it comes into contact with a collision of another object
                 ((Input.GetAxis("Horizontal") < 0) && !leftWall.collider && (collision.transform.position.x < transform.position.x)))
                transform.position = new Vector3(collision.transform.position.x, transform.position.y, transform.position.z);

        }
    }
}
