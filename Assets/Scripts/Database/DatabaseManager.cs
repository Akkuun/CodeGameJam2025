using System;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;

public class DatabaseManager : MonoBehaviour
{
    public TMP_InputField m_name;

    private string m_userID;
    private DatabaseReference m_database;

    private bool m_isLoaded = false;

    void Start()
    {
        m_userID = SystemInfo.deviceUniqueIdentifier;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                m_database = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase database reference is set.");

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
            m_isLoaded = true;
        });
    }

    public void SaveScore()
    {
        if (m_database == null)
        {
            Debug.LogError("Firebase database reference is null.");
            return;
        }
        if(m_name.text == "")
        {
            Debug.LogError("Name is empty.");
            return;
        }

        //TODO: Récupérer le score du joueur
        int _score = 0; 

        //TODO: Récupérer les notes du joueur
        bool _note1 = false;
        bool _note2 = false;
        bool _note3 = false;

        //Saving the user data
        User user = new User(m_name.text, m_userID, _score, _note1, _note2, _note3);
        string json = JsonUtility.ToJson(user);
        
        m_database.Child("users").Child(m_name.text).SetRawJsonValueAsync(json).ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to save user data: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("User data saved successfully.");
            }
        });
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        while (!m_isLoaded)
        {
            await Task.Delay(100);
        }
        List<User> users = new List<User>();

        if (m_database == null)
        {
            Debug.LogError("Firebase database reference is null.");
            return users;
        }

        try
        {
            DataSnapshot snapshot = await m_database.Child("users").GetValueAsync();
            foreach (DataSnapshot userSnapshot in snapshot.Children)
            {
                User userData = JsonUtility.FromJson<User>(userSnapshot.GetRawJsonValue());
                users.Add(userData);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to read the database: " + e.Message);
        }

        return users;
    }

    
}
