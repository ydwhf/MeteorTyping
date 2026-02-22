using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    public void Logout()
    {
        // Hapus semua session lokal
        PlayerPrefs.DeleteKey("id_user");
        PlayerPrefs.DeleteKey("level_unlocked");
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.Save();

        Debug.Log("<color=red>LOGOUT: semua data session dihapus.</color>");

        // Balik ke halaman login
        SceneManager.LoadScene("Home");
    }
}