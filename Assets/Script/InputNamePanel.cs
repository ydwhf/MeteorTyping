using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputNamePanel : MonoBehaviour
{
    public TMP_InputField nameField;
    public GameObject selectLevelPanel;

    [HideInInspector]
    public string pendingLevel;

    public void OnOk()
    {
        string name = nameField.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            Debug.LogWarning("Nama tidak boleh kosong");
            return;
        }

        PlayerPrefs.SetString("player_name", name);

        SceneManager.LoadScene(pendingLevel);
    }

    public void OnCancel()
    {
        // Hide panel input
        gameObject.SetActive(false);

        // Balikin panel select level
        selectLevelPanel.SetActive(true);
    }
}

