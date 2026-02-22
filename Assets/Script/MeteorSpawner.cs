using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public Transform spawnPoint;
    public Transform meteorRoot;
    public float spawnInterval = 5f;
    public float spawnYMin = -2f;
    public float spawnYMax = 2f;
    public float meteorTravelTime = 20f;
    public float leftBoundaryX = -12f;
    public TMP_FontAsset meteorFont;

    public List<string> words = new List<string> { "earth", "moon" };


    void Start()
    {
        StartCoroutine(SpawnSequence());
    }

    IEnumerator SpawnSequence()
    {
        for (int i = 0; i < words.Count; i++)
        {
            SpawnOne(words[i]);
            yield return new WaitForSeconds(spawnInterval);
        }

        // ?? Tunggu sampai semua meteor dihapus dari scene
        yield return new WaitUntil(() => GameObject.FindObjectsOfType<Meteor>().Length == 0);

        // ?? Saat tidak ada meteor tersisa, akhiri game
        TypingGame typingGame = FindObjectOfType<TypingGame>();
        if (typingGame != null)
        {
            typingGame.LevelComplete();
        }
    }

    void SpawnOne(string word)
    {
        if (meteorPrefab == null || spawnPoint == null) return;

        // ?? Biar lurus: Y tetap
        Vector3 pos = spawnPoint.position;
        pos.y = 0f; // semua meteor sejajar

        GameObject go = Instantiate(meteorPrefab, pos, Quaternion.identity, meteorRoot);
        Meteor m = go.GetComponent<Meteor>();
        if (m != null)
        {
            m.word = word;
            m.travelTime = meteorTravelTime;
            m.leftBoundaryX = leftBoundaryX;

            var tmp = go.GetComponentInChildren<TextMeshPro>();
            if (tmp)
            {
                tmp.text = word;

                if (meteorFont != null)
                    tmp.font = meteorFont;   // <- ganti font di sini
            }

        }
    }
}
