using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPlayerPrefs : MonoBehaviour
{
    void Start()
    {
        Debug.Log("<color=yellow> ===== CHECKING PLAYER PREFS ===== </color>");
        Debug.Log("id_user: " + PlayerPrefs.GetInt("id_user", -1));
        Debug.Log("username: " + PlayerPrefs.GetString("username", "NULL"));
        Debug.Log("level_unlock: " + PlayerPrefs.GetInt("level_unlock", -1));
        Debug.Log("name: " + PlayerPrefs.GetString("name", "NULL"));
        Debug.Log("<color=yellow> ==================================== </color>");
    }
}
