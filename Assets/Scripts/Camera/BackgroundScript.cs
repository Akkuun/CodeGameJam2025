using UnityEngine;

[System.Serializable]
public class BackgroundElement
{
    public SpriteRenderer m_backgroundSprite;
    [Range(0,1)] public float m_scrollSpeed;
    [HideInInspector] public Material m_material;
}

public class BackgroundScript : MonoBehaviour
{
    private const float SCROLL_MULTIPLIER = 0.01f;

    [SerializeField] private BackgroundElement[] m_backgroundElements;

    void Start()
    {
        foreach (BackgroundElement element in m_backgroundElements)
        {
            element.m_material = element.m_backgroundSprite.material;
        }
    }

    void Update()
    {
        foreach (BackgroundElement element in m_backgroundElements)
        {
            Vector2 offset = element.m_material.GetVector("_Offset");
            offset.x = transform.position.x * element.m_scrollSpeed * SCROLL_MULTIPLIER;
            offset.y = transform.position.y * element.m_scrollSpeed * SCROLL_MULTIPLIER;
            element.m_material.SetVector("_Offset", offset);
        }
    }
}
