using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypingGame : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public TMP_Text scoreText;
    public TMP_Text resultText;
    public Slider lifeBar;

    [Header("Game")]
    public MeteorSpawner spawner; // assign di inspector kalau perlu
    public int lives = 3;
    public int score = 0;
    public int pointsPerMeteor = 5;

    [Header("Game Complete UI")]
    public GameObject gameCompletePanel;
    public TMP_Text gameCompleteScoreText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TMP_Text gameOverScoreText;

    [Header("Hide When Game Over")]
    public GameObject[] hideOnGameOver;

    void Start()
    {
        UpdateUI();
        inputField.onSubmit.AddListener(OnSubmit);
        inputField.ActivateInputField();
        inputField.Select();

        lifeBar.maxValue = lives;
        lifeBar.value = lives;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

    }

    void OnSubmit(string text)
    {
        string t = text.Trim();
        if (string.IsNullOrEmpty(t))
        {
            resultText.text = "Ketik dulu dong!";
            inputField.text = "";
            inputField.ActivateInputField();
            return;
        }

        // cari meteor yang cocok
        Meteor[] metas = FindObjectsOfType<Meteor>();
        List<Meteor> matches = new List<Meteor>();
        foreach (var m in metas)
        {
            if (BruteForceMatch(t, m.word))
            {
                matches.Add(m);
            }
        }

        if (matches.Count == 0)
        {
            resultText.text = " Salah! Coba lagi.";
        }
        else
        {
            Meteor target = matches.OrderBy(m => m.transform.position.x).First();

            // ?? Jangan langsung hancurin meteor di sini
            // target.Explode();  <-- HAPUS / KOMENTARIN INI

            resultText.text = $"Menembak '{target.word}'!";
            score += pointsPerMeteor;
            UpdateUI();

            // ?? Pesawat nembak sekali ke depan
            if (PlayerShooting.instance != null)
            {
                PlayerShooting.instance.ShootOnce();
            }
        }



        inputField.text = "";
        inputField.ActivateInputField();
    }

    bool BruteForceMatch(string input, string pattern)
    {
        if (input == null || pattern == null) return false;
        if (input.Length != pattern.Length) return false;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (input[i] != pattern[i]) return false;
        }
        return true;
    }

    public void UpdateUI()
    {
        scoreText.text = $"{score}";
        lifeBar.value = lives;
    }

    // dipanggil dari Meteor.SendMessageUpwards("MeteorReachedLeft", this)
    public void MeteorReachedLeft(Meteor m)
    {
        lives--;
 
        UpdateUI();
        if (lives > 0)
        {
            // respawn pesawat kalau masih punya nyawa
            if (Player.instance != null)
            {
                // kasih sedikit delay biar efek ledakan sempat tampil
                Invoke(nameof(RespawnPlayer), 1f);
            }
        }
        else
        {
            GameOver();
        }
    }

     public void OnMeteorDestroyed(Meteor m)
    {
        // optional: additional effects saat meteor dihancurkan
    }

    void RespawnPlayer()
    {
        if (Player.instance != null)
            Player.instance.Respawn();
    }

    public void LevelComplete()
    {
        inputField.interactable = false;
        if (spawner != null) spawner.StopAllCoroutines();

        // === SIMPAN DATA KE DATABASE ===
        StartCoroutine(SaveScoreToDB());

        ShowGameCompletePanel();
    }


    public void GameOver()
    {
        // Stop meteor spawn
        if (spawner != null)
            spawner.StopAllCoroutines();

        StartCoroutine(SaveScoreToDB());

        // Disable input
        inputField.onSubmit.RemoveListener(OnSubmit);
        inputField.interactable = false;

        ShowGameOverPanel();
    }

    void ShowGameCompletePanel()
    {
        // Matikan semua UI / object yang ingin disembunyikan
        foreach (var obj in hideOnGameOver)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Tampilkan panel game over
        if (gameCompletePanel != null)
        {
            gameCompletePanel.SetActive(true);

            if (gameCompleteScoreText != null)
                gameCompleteScoreText.text = $"{score}";
        }
    }

        void ShowGameOverPanel()
    {
        // Matikan semua UI / object yang ingin disembunyikan
        foreach (var obj in hideOnGameOver)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Tampilkan panel game over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverScoreText != null)
                gameOverScoreText.text = $"{score}";
        }
    }

    IEnumerator SaveScoreToDB()
    {
        int userID = PlayerPrefs.GetInt("id_user", -1);
        if (userID == -1)
        {
            Debug.LogError("id_user tidak ditemukan di PlayerPrefs!");
            yield break;
        }

        int levelID = GetLevelID();
        int finalScore = score;

        // kirim score
        yield return StartCoroutine(ApiManager.SaveScore(userID, levelID, finalScore));

        Debug.Log("Score berhasil dikirim ke server dengan userID: " + userID);
    }



    int GetLevelID()
    {
        string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (scene.Contains("1")) return 1;
        if (scene.Contains("2")) return 2;
        if (scene.Contains("3")) return 3;

        return 1; // default
    }

    public void OnClickHome()
    {
        StartCoroutine(ApiManager.GetUser(PlayerPrefs.GetInt("id_user"), (user) =>
        {
            PlayerPrefs.SetInt("level_unlocked", user.level_unlocked);
            PlayerPrefs.Save();
            SceneManager.LoadScene("SelectLevel");
        }));
    }
}
