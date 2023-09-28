using System.Collections;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect;
    private Transform tr;

    private int hitCount = 0;
    private new Rigidbody rigidbody = default;
    void Start()
    {
        tr = GetComponent<Transform>();
        
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.tag == "BULLET")
        {
            Destroy(coll.gameObject);

            if(++hitCount >= 3)
            {
                StartCoroutine(ExplosionBarrel());
            }
        }
    }

    IEnumerator ExplosionBarrel()
    {
        Instantiate(expEffect, tr.position, Quaternion.identity);

        Collider[] colls = Physics.OverlapSphere(tr.position, 10.0f);

        foreach(Collider coll in colls)
        {
            rigidbody = coll.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.mass = 1.0f;
                rigidbody.AddExplosionForce(100.0f, tr.position, 10.0f, 600.0f);
            }
        }
        Destroy(gameObject, 5.0f);
        yield return null;
    }
}
