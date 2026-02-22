using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPlayerName : MonoBehaviour
{
    public TMP_Text playerNameText;

    void Start()
    {
        string name = PlayerPrefs.GetString("name", "Player");
        playerNameText.text = name;
    }
}
