using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [HideInInspector] public string word;
    public float travelTime = 20f;
    public float leftBoundaryX = -12f;
    public int scoreValue = 5;

    private Vector3 startPos;
    private Vector3 endPos;
    private float t = 0f;
    private TextMeshPro tmpText;
    private SpriteRenderer sr;
    private bool isExploded = false;
    public GameObject explosionFX;


    void Awake()
    {
        tmpText = GetComponentInChildren<TextMeshPro>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(leftBoundaryX, transform.position.y, transform.position.z);
        t = 0f;
        if (tmpText != null) tmpText.text = word ?? "";
    }

    void Update()
    {
        if (isExploded) return;

        if (travelTime <= 0f)
        {
            transform.position = endPos;
            ReachedLeft();
            return;
        }

        t += Time.deltaTime / travelTime;
        transform.position = Vector3.Lerp(startPos, endPos, t);

        if (transform.position.x <= leftBoundaryX)
        {
            ReachedLeft();
        }
    }

    void ReachedLeft()
    {
        SendMessageUpwards("MeteorReachedLeft", this, SendMessageOptions.DontRequireReceiver);
        Destroy(gameObject);
    }

    public void Explode()
    {
        if (isExploded) return;
        isExploded = true;

        if (sr) sr.enabled = false;

        SendMessageUpwards("OnMeteorDestroyed", this, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject, 0.4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 💥 Kalau kena peluru dari pesawat
        if (other.CompareTag("Projectile"))
        {
            // efek ledakan kalau ada
            if (explosionFX != null)
            {
                Instantiate(explosionFX, transform.position, Quaternion.identity);
            }

            // tambahin skor lewat TypingGame
            TypingGame typeGame = FindObjectOfType<TypingGame>();
            if (typeGame != null)
            {
                typeGame.OnMeteorDestroyed(this);
            }

            // hancurin peluru & meteor
            Destroy(other.gameObject);
            Explode();

            if (PlayerShooting.instance != null)
                PlayerShooting.instance.PlayExplosionSound();

            return; // biar gak lanjut ke bawah
        }

        // 🚀 Kalau nabrak pesawat
        if (!other.CompareTag("Player")) return;

        TypingGame typingGame = FindObjectOfType<TypingGame>();
        if (typingGame != null)
        {
            typingGame.MeteorReachedLeft(this);
        }
        else
        {
            Debug.LogWarning("TypingGame tidak ditemukan! Pastikan ada di scene dan aktif.");
        }

        if (Player.instance != null)
        {
            Player.instance.GetDamage(1);
        }
        else
        {
            Debug.LogWarning("Player.instance null — pastikan Player aktif di scene.");
        }
    }

}
