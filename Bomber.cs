using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour // A script for a shooting enemy
{
    public GameObject bullet;
    public Transform shoot;
    public float timeShoot = 4f;

    void Start()
    {
        shoot.transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z); // The initial position of the ammo
        StartCoroutine(Shooting());
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(timeShoot);
        Instantiate(bullet, shoot.transform.position, transform.rotation);
        StartCoroutine(Shooting());
    }
}
