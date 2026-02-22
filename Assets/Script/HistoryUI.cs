using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryUI : MonoBehaviour
{
    public Transform content;
    public GameObject rowPrefab;

    void Start()
    {
        StartCoroutine(ApiManager.GetHistory((historyList) =>
        {
            foreach (var h in historyList)
            {
                GameObject row = Instantiate(rowPrefab, content);

                var t = row.transform;
                t.Find("Name").GetComponent<TMP_Text>().text = h.name;
                t.Find("Score").GetComponent<TMP_Text>().text = h.score.ToString();
                t.Find("Level").GetComponent<TMP_Text>().text = h.level_name;
                t.Find("Date").GetComponent<TMP_Text>().text = h.created_at;
            }
        }));
    }
}


