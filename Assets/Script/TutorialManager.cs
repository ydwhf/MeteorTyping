using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [Header("Isi data di sini (tanpa drag referensi UI)")]
    public Sprite gambar;
    public string judul;

    private Image stepImage;
    private TMP_Text stepTitle;

    void Awake()
    {
        // otomatis cari komponen berdasarkan nama object di child
        stepImage = transform.Find("StepImage").GetComponent<Image>();
        stepTitle = transform.Find("StepTitle").GetComponent<TMP_Text>();
    }

    void Start()
    {
        // isi UI dari data
        if (stepImage != null) stepImage.sprite = gambar;
        if (stepTitle != null) stepTitle.text = judul;
    }
}
