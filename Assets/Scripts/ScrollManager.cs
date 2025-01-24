using UnityEngine;

[CreateAssetMenu(fileName = "SpeedManager", menuName = "Scriptable Objects/SpeedManager")]
public class SpeedManager : MonoBehaviour
{
    public static SpeedManager instance;
    [SerializeField]
    public float speed = 1.0f;
    private float audioLength = 28.44f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public float GetSpeed()
    {
        return speed;
    }
}
