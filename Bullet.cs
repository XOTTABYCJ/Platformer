using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour // Script for ammo shooting enemy
{
    float speed = 4f;
    float TimeToDisable = 1f;

    void Start()
    {
        StartCoroutine(SetDisable());
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }


    IEnumerator SetDisable()
    {
        yield return new WaitForSeconds(TimeToDisable);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopCoroutine(SetDisable());
        gameObject.SetActive(false);
    }
}
