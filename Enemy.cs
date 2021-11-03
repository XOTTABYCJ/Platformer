using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour // Script for common enemy
{
    bool isHit = false;
    public GameObject drop;
    public SoundEffector soundEffector;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isHit)
        {
            collision.gameObject.GetComponent<Player>().RecountHp(-1);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 8f, ForceMode2D.Impulse); // When colliding with the "Player" object, the player is thrown back a certain distance
        }
    }

    public IEnumerator Death()
    {
        if (drop != null){ 
            Instantiate(drop, transform.position, Quaternion.identity); // Drop some item. Choose in inspector
        }
        isHit = true;
        GetComponent<Animator>().SetBool("Dead", true);
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; 
        GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).GetComponent<Collider2D>().enabled = false; 
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    public void StartDeath()
    {
        StartCoroutine(Death());
    }
}
