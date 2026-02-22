using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public string levelSceneName;
    public GameObject inputPanel;
    public GameObject selectLevelPanel;

    public void OnClickLevel()
    {
        // Kirim nama level yang mau dibuka nanti
        inputPanel.GetComponent<InputNamePanel>().pendingLevel = levelSceneName;
    }
}