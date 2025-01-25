using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public enum Theme
    {
        Modern,
        Futuristic,
        Medieval
    }

    public Theme currentTheme = Theme.Medieval;

    void Start()
    {
        ChangeTheme(currentTheme);
    }

    public void ApplyTheme(Theme theme)
    {
        foreach (Theme t in System.Enum.GetValues(typeof(Theme)))
        {
            if (t != theme)
            {
                string tag = t.ToString();
                GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag(tag);
                Debug.Log("Disabling objects with tag: " + tag);
                foreach (GameObject obj in objectsToDisable)
                {
                    Debug.Log("Disabling " + obj.name);
                    obj.SetActive(false);
                }
            }
        }

        string currentTag = theme.ToString();
        GameObject[] objectsToEnable = GameObject.FindGameObjectsWithTag(currentTag);
        Debug.Log("Enabling objects with tag: " + currentTag);
        foreach (GameObject obj in objectsToEnable)
        {
            Debug.Log("Enabling " + obj.name);
            obj.SetActive(true);
        }
    }

    public void ChangeTheme(Theme newTheme)
    {
        currentTheme = newTheme;
        ApplyTheme(currentTheme);
    }
}
