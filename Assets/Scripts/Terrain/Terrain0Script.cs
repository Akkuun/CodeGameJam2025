using Unity.VisualScripting;
using UnityEngine;

public class Terrain0Script : MonoBehaviour
{
    public float speed = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        // Call ScrollableObject's Update method
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
