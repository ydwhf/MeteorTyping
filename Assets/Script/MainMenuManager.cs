using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject loginPanel; // drag LoginPanel ke sini

    public void OnPlayClicked()
    {
        loginPanel.SetActive(true);
    }
}
