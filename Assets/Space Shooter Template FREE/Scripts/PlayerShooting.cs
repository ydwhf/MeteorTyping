using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// guns objects in 'Player's' hierarchy
[System.Serializable]
public class Guns
{
    public GameObject rightGun, leftGun, centralGun;
    [HideInInspector] public ParticleSystem leftGunVFX, rightGunVFX, centralGunVFX;
}

public class PlayerShooting : MonoBehaviour
{
    [Tooltip("shooting frequency. the higher the more frequent")]
    public float fireRate;

    [Tooltip("projectile prefab")]
    public GameObject projectileObject;

    [HideInInspector] public float nextFire;

    [Tooltip("current weapon power")]
    [Range(1, 4)]
    public int weaponPower = 1;

    public Guns guns;
    [HideInInspector] public int maxweaponPower = 4;
    public static PlayerShooting instance;

    // 🔊 Tambahan: sound FX
    [Header("Sound Effects")]
    public AudioClip shootSound;
    public AudioClip explosionSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        if (guns.centralGun != null)
            guns.centralGunVFX = guns.centralGun.GetComponent<ParticleSystem>();

        // 🔊 Inisialisasi AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        // Tidak ada auto shooting
    }

    void MakeAShot()
    {
        switch (weaponPower)
        {
            case 1:
                CreateLazerShot(projectileObject, guns.centralGun.transform.position, Quaternion.Euler(0, 0, -90));
                if (guns.centralGunVFX != null)
                    guns.centralGunVFX.Play();
                break;
        }

        // 🔊 Bunyi saat tembak
        if (shootSound != null && audioSource != null)
            audioSource.PlayOneShot(shootSound);
    }

    void CreateLazerShot(GameObject lazer, Vector3 pos, Quaternion rot)
    {
        Instantiate(lazer, pos, rot);
    }

    public void ShootOnce()
    {
        if (Time.time > nextFire)
        {
            MakeAShot();
            nextFire = Time.time + 1 / fireRate;
        }
    }

    // 🔊 Method publik untuk bunyi ledakan
    public void PlayExplosionSound()
    {
        Debug.Log("💥 Explosion sound dipanggil!");

        if (explosionSound != null && audioSource != null)
            audioSource.PlayOneShot(explosionSound);
        else
            Debug.LogWarning("❌ explosionSound atau audioSource null!");
    }

}
