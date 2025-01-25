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
                foreach (GameObject obj in objectsToDisable)
                {
                    obj.SetActive(false);
                }
            }
        }

        string currentTag = theme.ToString();
        GameObject[] objectsToEnable = GameObject.FindGameObjectsWithTag(currentTag);
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    public void ChangeTheme(Theme newTheme)
    {
        currentTheme = newTheme;
        ApplyTheme(currentTheme);
    }

    public int getTheme()
    {
        if (currentTheme == Theme.Modern) return 0;
        if (currentTheme == Theme.Medieval) return 1;
        if (currentTheme == Theme.Futuristic) return 2;
        return -1;
    }
}
