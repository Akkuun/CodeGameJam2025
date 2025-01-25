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
        // Désactiver les objets des thèmes qui ne correspondent pas au thème actuel
        foreach (Theme t in System.Enum.GetValues(typeof(Theme)))
        {
            ToggleThemeObjects(t, t == theme);
        }
    }

    public void ChangeTheme(Theme newTheme)
    {
        currentTheme = newTheme;
        ApplyTheme(currentTheme);
    }

    private void ToggleThemeObjects(Theme theme, bool enable)
    {
        string tag = theme.ToString();
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        if (objects.Length > 0)
        {
            foreach (GameObject obj in objects)
            {
                Debug.LogError("Found object with tag: " + tag);
                if (enable)
                {
                    Debug.Log("Enabling " + obj.name + " with tag: " + tag);
                    obj.SetActive(true);
                    obj.active = true;
                }
                else
                {
                    Debug.Log("Disabling " + obj.name + " with tag: " + tag);
                    obj.SetActive(false);
                    obj.active = false;
                }
            }
        }
        else
        {
            Debug.LogWarning("No objects found with tag: " + tag);
        }
    }
}
