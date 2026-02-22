using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public int totalLevel = 3;

    void Start()
    {
        int unlocked = PlayerPrefs.GetInt("level_unlocked", 1);

        Debug.Log("LEVEL TERBUKA: " + unlocked); // Debug biar lu lihat

        for (int i = 1; i <= totalLevel; i++)
        {
            Transform levelObj = transform.Find("Level" + i);

            if (levelObj == null)
            {
                Debug.LogWarning("Tidak menemukan object: Level" + i);
                continue;
            }

            Button btn = levelObj.GetComponent<Button>();
            GameObject lockIcon = levelObj.Find("LockIcon").gameObject;
            TMP_Text numText = levelObj.Find("LevelNumber").GetComponent<TMP_Text>();

            numText.text = "Level " + i;

            if (i <= unlocked)
            {
                lockIcon.SetActive(false);
                btn.interactable = true;

                int levelIndex = i;
                btn.onClick.AddListener(() =>
                {
                    PlayerPrefs.SetInt("selected_level", levelIndex);
                    Debug.Log("Masuk Level: " + levelIndex);
                    SceneManager.LoadScene("Gameplay");
                });
            }
            else
            {
                lockIcon.SetActive(true);
                btn.interactable = false;
            }
        }
    }
}
