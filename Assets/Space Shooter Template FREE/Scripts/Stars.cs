using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public float speed = 5f; // Kecepatan gerak
    public float leftLimit = -10f; // Batas kiri
    public float rightStart = 10f; // Posisi awal di kanan

    void Start()
    {
        // Mulai dari kanan
        transform.position = new Vector3(rightStart, transform.position.y, transform.position.z);
    }

    void Update()
    {
        // Gerak ke kiri dengan world space
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        // Kalau sudah lewat batas kiri, balikin lagi ke kanan
        if (transform.position.x < leftLimit)
        {
            transform.position = new Vector3(rightStart, transform.position.y, transform.position.z);
        }
    }

}
