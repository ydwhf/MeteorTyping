using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text statusText;

    public void OnClickLogin()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (username == "" || password == "")
        {
            statusText.text = "Isi username & password!";
            return;
        }

        StartCoroutine(ApiManager.Login(username, password, (resp) =>
        {
            HandleLoginResponse(resp);
        }));
    }

    private void HandleLoginResponse(ApiManager.LoginResponse resp)
    {
        if (resp == null)
        {
            statusText.text = "Gagal konek server!";
            return;
        }

        if (resp.status)
        {
            PlayerPrefs.SetInt("id_user", resp.data.id_user);
            PlayerPrefs.SetInt("level_unlocked", resp.data.level_unlocked);
            PlayerPrefs.SetString("username", resp.data.username);
            PlayerPrefs.SetString("name", resp.data.name);
            PlayerPrefs.Save();

            Debug.Log("LOGIN SAVED id_user = " + resp.data.id_user);

            // Pindah ke scene dengan coroutine
            StartCoroutine(DelayedSceneLoad());
        }
        else
        {
            statusText.text = resp.message != null ? resp.message : "Username atau password salah!";
        }
    }

    private IEnumerator DelayedSceneLoad()
    {
        yield return new WaitForSeconds(0.2f); // biar PlayerPrefs sempat tersimpan
        SceneManager.LoadScene("SelectLevel");
    }
}
