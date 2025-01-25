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
    [SerializeField] private bool m_showCloseToPlayer = false;

    void Start ()
    {
        m_databaseManager = FindAnyObjectByType<DatabaseManager>();
        if(m_databaseManager == null)
        {
            Debug.LogError("Database Manager is null.");
            return;
        }
        ReloadLeaderboard();
    }

    void OnEnable ()
    {
        if(m_databaseManager == null)
        {
            m_databaseManager = FindAnyObjectByType<DatabaseManager>();
        }

        if(m_databaseManager == null)
        {
            Debug.LogError("Database Manager is null.");
            return;
        }
        ReloadLeaderboard();
    }

    async void ReloadLeaderboard()
    {
        m_users = await m_databaseManager.GetAllUsersAsync();
        m_textComponents = GetComponentsInChildren<TextMeshProUGUI>();
        string userID = SystemInfo.deviceUniqueIdentifier;
        // Sort users by score in descending order
        m_users.Sort((user1, user2) => user2.Score.CompareTo(user1.Score)); 
        Debug.Log(m_users.Count);
        bool isPlayerInLeaderboard = false;
        for (int i = 0; i < m_textComponents.Length && i < m_users.Count; i++)
        {
            if(m_users[i].Id.Equals(userID))
            {
                isPlayerInLeaderboard = true;
                m_textComponents[i].color = Color.green;
            }
            m_textComponents[i].text = $"{i + 1} - {m_users[i].Name} - {m_users[i].Score}";
        }
        if(!isPlayerInLeaderboard)
        {
            int playerIndex = m_users.FindIndex(user => user.Id.Equals(userID))-1;
            for(int i = 7; i < m_textComponents.Length && i < m_users.Count && playerIndex+i-7 < m_users.Count; i++)
            {
                int localIndex = playerIndex+i-7;
                if(m_users[localIndex].Id.Equals(userID))
                {
                    m_textComponents[i].color = Color.green;
                }
                m_textComponents[i].text = $"{localIndex + 1} - {m_users[localIndex].Name} - {m_users[localIndex].Score}";
            }
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
