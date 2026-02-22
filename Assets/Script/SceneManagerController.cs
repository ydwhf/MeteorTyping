using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SceneManagerController : MonoBehaviour
{
    [System.Serializable]
    public class SceneButton
    {
        public Button button;
        public string sceneName;
    }

    [Header("Daftar tombol dan scene tujuan")]
    public SceneButton[] sceneButtons;

    void Start()
    {
        foreach (var sb in sceneButtons)
        {
            if (sb.button == null)
            {
                Debug.LogWarning("[SceneManagerController] Ada button yang belum di-assign!");
                continue;
            }

            string target = sb.sceneName;

            sb.button.onClick.AddListener(() => GoToScene(target));
        }
    }

    private void GoToScene(string sceneName)
    {
        if (sceneName == "exit")
        {
            Debug.Log("[SceneManagerController] Keluar dari game...");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // buat testing di editor
#endif
            return;
        }

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log($"[SceneManagerController] Pindah ke scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"[SceneManagerController] Scene '{sceneName}' tidak ditemukan di Build Settings!");
        }
    }
}
