using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script defines which sprite the 'Player" uses and its health.
/// </summary>

public class Player : MonoBehaviour
{
    public GameObject destructionFX;
    public static Player instance;

    private Vector3 startPosition;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        startPosition = transform.position;
    }

    public void GetDamage(int damage)
    {
        // kasih delay sebelum hancur supaya TypingGame sempat update
        Invoke(nameof(Destruction), 0.2f);
    }

    void Destruction()
    {
        if (CameraShake.instance != null)
            CameraShake.instance.Shake(0.3f, 0.2f);

        Instantiate(destructionFX, transform.position, Quaternion.identity);

        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
    }
}