using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public int damage = 20;
    public float speed = 1000.0f;

    private new Rigidbody rigidbody;
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward * speed);
    }
}
