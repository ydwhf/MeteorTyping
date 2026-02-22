using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsMove : MonoBehaviour
{
    public float speed = 1f; // sudah dipakai LevelController buat atur kecepatan

    void Update()
    {
        // Gerak dari kanan ke kiri
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Optional: Hapus planet kalau udah keluar dari layar (biar ga numpuk)
        if (transform.position.x < -17f)
        {
            Destroy(gameObject);
        }
    }
}
