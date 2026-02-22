using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggleButtons : MonoBehaviour
{
    public Image iconImage;
    public Sprite iconOn;
    public Sprite iconOff;

    void Start()
    {
        UpdateIcon();
    }

    public void Toggle()
    {
        BGMManager bgm = FindObjectOfType<BGMManager>();
        if (bgm == null) return;

        bgm.ToggleMusic();
        UpdateIcon();
    }

    void UpdateIcon()
    {
        BGMManager bgm = FindObjectOfType<BGMManager>();
        if (bgm == null) return;

        iconImage.sprite = bgm.isMusicOn ? iconOn : iconOff;
    }
}
