using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public GameObject selectLevelPanel;
    public GameObject howToPlayPanel;
    public GameObject aboutPanel;

    private GameObject activePanel = null;

    public void ToggleHowToPlay()
    {
        // Kalau sedang aktif -> matikan dan tampilkan SelectLevel
        if (activePanel == howToPlayPanel)
        {
            howToPlayPanel.SetActive(false);
            activePanel = null;
            selectLevelPanel.SetActive(true);
            return;
        }

        // Kalau panel about aktif -> matikan
        if (activePanel == aboutPanel)
            aboutPanel.SetActive(false);

        // Buka How To Play
        howToPlayPanel.SetActive(true);
        selectLevelPanel.SetActive(false);
        activePanel = howToPlayPanel;
    }

    public void ToggleAbout()
    {
        // Kalau About sedang aktif -> matikan dan tampilkan SelectLevel
        if (activePanel == aboutPanel)
        {
            aboutPanel.SetActive(false);
            activePanel = null;
            selectLevelPanel.SetActive(true);
            return;
        }

        // Kalau panel HowToPlay aktif -> matikan
        if (activePanel == howToPlayPanel)
            howToPlayPanel.SetActive(false);

        // Buka Panel About
        aboutPanel.SetActive(true);
        selectLevelPanel.SetActive(false);
        activePanel = aboutPanel;
    }
}
