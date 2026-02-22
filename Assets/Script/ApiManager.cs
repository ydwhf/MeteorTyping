using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiManager
{
    // private static string BASE_URL = "http://localhost:8080/api";
    private static string BASE_URL = "https://admintypinggame.my.id/api";
    // Ganti nanti ke IP LAN kalau build Android

    // --- MODEL DATA ---
    [System.Serializable]
    private class CreateUserRequest
    {
        public string name;
    }

    [System.Serializable]
    private class SaveScoreRequest
    {
        public int idUser;
        public int idLevel;
        public int score;
    }

    // --- RESPONSE ---
    [System.Serializable]
    public class UserResponse
    {
        public string status;
        public int idUser;
    }

    // === LOGIN ===
    [System.Serializable]
    public class LoginRequest
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public bool status;
        public string message;
        public UserData data;
    }

    [System.Serializable]
    public class UserData
    {
        public int id_user;
        public string username;
        public string name;
        public int level_unlocked;
    }

    // === GET USER ===
    [System.Serializable]
    public class GetUserResponse
    {
        public bool status;
        public string message;
        public UserData data;
    }

    public static IEnumerator Login(string username, string password, System.Action<LoginResponse> callback)
    {
        string url = BASE_URL + "/login";

        LoginRequest data = new LoginRequest()
        {
            username = username,
            password = password
        };

        string json = JsonUtility.ToJson(data);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
         Debug.Log("JSON SEND: " + json);

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            LoginResponse resp = JsonUtility.FromJson<LoginResponse>(req.downloadHandler.text);
            callback(resp);
        }
        else
        {
            Debug.LogError("LOGIN ERROR: " + req.error);
            callback(null);
        }
        Debug.Log("RESP RAW: " + req.downloadHandler.text);

    }

    // === GET OR CREATE USER ===
    public static IEnumerator GetOrCreateUser(string playerName, System.Action<int> callback)
    {
        string url = BASE_URL + "/users/getOrCreate";

        CreateUserRequest data = new CreateUserRequest()
        {
            name = playerName
        };

        string json = JsonUtility.ToJson(data);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            UserResponse resp = JsonUtility.FromJson<UserResponse>(req.downloadHandler.text);
            callback(resp.idUser);
        }
        else
        {
            Debug.LogError("Error GetOrCreateUser: " + req.error);
            callback(-1);
        }
    }

    // === SAVE SCORE ===
    public static IEnumerator SaveScore(int idUser, int idLevel, int score)
    {
        string url = BASE_URL + "/scores/save";

        SaveScoreRequest data = new SaveScoreRequest()
        {
            idUser = idUser,
            idLevel = idLevel,
            score = score
        };
        Debug.Log("<color=cyan>[DEBUG USER ID]</color> " + idUser);
        Debug.Log("<color=green>[DEBUG LEVEL]</color> " + idLevel);
        Debug.Log("<color=magenta>[DEBUG SCORE]</color> " + score);
        Debug.Log("<color=orange>[DEBUG URL]</color> " + url);

        string json = JsonUtility.ToJson(data);

        UnityWebRequest req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
            Debug.LogError("Error SaveScore: " + req.error);
        else
            Debug.Log("Score saved!");
    }
    public static IEnumerator GetHistory(Action<List<HistoryData>> callback)
    {
        string url = BASE_URL + "/scores/history";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;

            // Bungkus json array ke format yang JsonUtility bisa baca
            json = "{\"data\":" + json + "}";

            HistoryListWrapper wrapper = JsonUtility.FromJson<HistoryListWrapper>(json);

            callback(wrapper.data);
        }
        else
        {
            Debug.Log("Error: " + www.error);
            callback(new List<HistoryData>());
        }
    }

    [System.Serializable]
    public class HistoryListWrapper
    {
        public List<HistoryData> data;
    }

    public static IEnumerator GetUser(int idUser, System.Action<UserData> callback)
    {
        string url = BASE_URL + "/getUser/" + idUser;

        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("RAW GET USER: " + req.downloadHandler.text);

            GetUserResponse resp = JsonUtility.FromJson<GetUserResponse>(req.downloadHandler.text);

            if (resp.status)
                callback(resp.data);
            else
                callback(null);
        }
        else
        {
            Debug.LogError("ERROR GET USER: " + req.error);
            callback(null);
        }
    }
}
