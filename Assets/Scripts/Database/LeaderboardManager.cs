using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    private DatabaseManager m_databaseManager;
    private List<User> m_users;
    private TextMeshProUGUI[] m_textComponents;

    async void Start ()
    {
        m_databaseManager = FindAnyObjectByType<DatabaseManager>();
        if(m_databaseManager == null)
        {
            Debug.LogError("Database Manager is null.");
            return;
        }
        m_users = await m_databaseManager.GetAllUsersAsync();
        if(m_users.Count == 0)
        {
            Debug.LogError("No users found.");
            return;
        }
        ReloadLeaderboard();
    }

    void ReloadLeaderboard()
    {
        m_textComponents = GetComponentsInChildren<TextMeshProUGUI>();

        // Sort users by score in descending order
        m_users.Sort((user1, user2) => user2.Score.CompareTo(user1.Score)); 

        for (int i = 0; i < m_textComponents.Length && i < m_users.Count; i++)
        {
            m_textComponents[i].text = $"{i + 1}- {m_users[i].Name} - {m_users[i].Score}";
        }
        for (int j = m_users.Count; j < m_textComponents.Length; j++)
        {
            m_textComponents[j].text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
