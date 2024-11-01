using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed;
    public float damage;
    private Vector3 dir;
    private Camera cam;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //카메라 방향지정
        cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 10);
        dir = (cam.ScreenToWorldPoint(screenCenter) - cam.transform.position).normalized;
    }

    private void FixedUpdate()
    {
        //카메라 방향으로 전진
        BulletMove();
    }

    private void BulletMove()
    {
        rb.velocity = dir * bulletSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
        }
    }
}
