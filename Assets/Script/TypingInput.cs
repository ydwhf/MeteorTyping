using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypingInput : MonoBehaviour
{
    public TMP_InputField inputField; // tempat kita ngetik
    public TMP_Text outputText;       // buat nampilin hasil

    void Start()
    {
        // Fokus langsung ke input field waktu game jalan
        inputField.ActivateInputField();

        // Saat tekan Enter (Submit)
        inputField.onSubmit.AddListener(HandleInput);
    }

    void HandleInput(string text)
    {
        // Simpan hasil ke text UI
        outputText.text = "Kamu mengetik: " + text;

        // Reset input biar siap ketik lagi
        inputField.text = "";
        inputField.ActivateInputField();
    }
}
