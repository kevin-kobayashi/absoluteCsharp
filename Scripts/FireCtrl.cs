using System.Collections;
using UnityEngine;
using System;

public class FireCtrl : MonoBehaviour
{
    [SerializeField] public JellyCtrl playerObj = default;
    //GameCtrl gameCtrl;
    public GameObject bullet;
    public Transform firePos;
    public MeshRenderer _renderer;
    //[SerializeField] public JellyCtrl playerObj = default;
    private float shootInterval = 0;

    [SerializeField] private AudioClip shotSfx;
    AudioSource audioSource;
    public float shotsfxVolumn = 1.0f;


    private void Start()
    {
        _renderer.enabled = false;
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    void Update()
    {
        audioSource.volume = shotsfxVolumn;
        shootInterval -= Time.deltaTime;
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    public void Fire()
    {
        if(shootInterval <= 0)
        {
            // Invoke("playerShot", 0.5f);
            StartCoroutine(this.ShowMuzzleFlash());
            //gameCtrl.PlaySfx(transform.position, shotSfx);
            StartCoroutine(this.CreateBullet());
            shootInterval = 0.5f;
        }
        else
        {
            return;
        }
    }

    IEnumerator ShowMuzzleFlash()
    {
        yield return new WaitForSeconds(0.4f);
        _renderer.enabled = true;
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.01f, 0.1f));
        _renderer.enabled = false;
    }

    IEnumerator CreateBullet()
    {
        yield return new WaitForSeconds(0.5f);
        audioSource.PlayOneShot(shotSfx, 0.2f);
        Instantiate(bullet, firePos.position, firePos.rotation);
        yield return new WaitForSeconds(0.1f);
        audioSource.PlayOneShot(shotSfx, 0.2f);
        Instantiate(bullet, firePos.position, firePos.rotation);
        yield return null;
    }
}
