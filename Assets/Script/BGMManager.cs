using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;
    private AudioSource audioSource;
    public bool isMusicOn = true;

    [Header("Daftar Backsound per Scene")]
    public SceneMusic[] sceneMusics;
    public float fadeDuration = 1f;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    private void Awake()
    {
        // Cegah duplikasi
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = false;

        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void Start()
    {
        // Mulai musik scene pertama
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        PlayMusicForScene(newScene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip newClip = null;

        foreach (var s in sceneMusics)
        {
            if (s.sceneName == sceneName)
            {
                newClip = s.musicClip;
                break;
            }
        }

        // Kalau scene tidak punya musik
        if (newClip == null)
        {
            StartCoroutine(FadeOutAndStop());
            return;
        }

        // Kalau musik sama, jangan restart
        if (audioSource.clip == newClip && audioSource.isPlaying)
            return;

        StartCoroutine(FadeToNewClip(newClip));
    }

    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        // Fade out
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;
    }

    private IEnumerator FadeOutAndStop()
    {
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = null;
    }

    public void ToggleMusic()
    {
        isMusicOn = !isMusicOn;

        if (isMusicOn)
            audioSource.Play();
        else
            audioSource.Pause();
    }


}