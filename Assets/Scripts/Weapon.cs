using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Weapon : MonoBehaviour
{
    public bool singleFire = false;  //Disparo unico o automatica
    public float cadenciaTiro = 0.1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int cargadorSize = 30;
    public float timeToReload = 1.5f;
    public float weaponDamage = 15;
    public AudioClip fireAudio;
    public AudioClip reloadAudio;

    [HideInInspector]
    public WeaponManager manager;

    float nextFireTime = 0;
    bool canFire = true;
    int cargadorSizeDefault = 0;
    AudioSource audioSource;

    void Start()
    {
        cargadorSizeDefault = cargadorSize;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        //Sonido 3D
        audioSource.spatialBlend = 1f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && singleFire)
        {
            Fire();
        }
        if (Input.GetMouseButton(0) && !singleFire)
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R) && canFire)
        {
            StartCoroutine(Reload());
        }
    }

    void Fire()
    {
        if (canFire)
        {
            if (Time.time > nextFireTime)
            {
                nextFireTime = Time.time + cadenciaTiro;

                if (cargadorSize > 0)
                {
                    //FirePoint
                    Vector3 firePointPointerPosition = manager.playerCamera.transform.position + manager.playerCamera.transform.forward * 100;
                    RaycastHit hit;
                    if (Physics.Raycast(manager.playerCamera.transform.position, manager.playerCamera.transform.forward, out hit, 100))
                    {
                        firePointPointerPosition = hit.point;
                    }
                    firePoint.LookAt(firePointPointerPosition);
                    //Fire
                    GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                    Bullet bullet = bulletObject.GetComponent<Bullet>();
                    //Set daño
                    bullet.SetDamage(weaponDamage);

                    cargadorSize--;
                    audioSource.clip = fireAudio;
                    audioSource.Play();
                }
                else
                {
                    StartCoroutine(Reload());
                }
            }
        }
    }

    IEnumerator Reload()
    {
        canFire = false;

        audioSource.clip = reloadAudio;
        audioSource.Play();

        yield return new WaitForSeconds(timeToReload);

        cargadorSize = cargadorSizeDefault;

        canFire = true;
    }

    public void ActivateWeapon(bool activate)
    {
        StopAllCoroutines();
        canFire = true;
        gameObject.SetActive(activate);
    }
}