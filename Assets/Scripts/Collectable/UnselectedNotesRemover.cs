using UnityEngine;

public class UnselectedNotesRemover : MonoBehaviour
{
    public GameObject note0;
    public GameObject note1;
    public GameObject note2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (note0 == null || note1 == null || note2 == null)
        {
            try {
                Destroy(note0);
            } catch (System.Exception) {}
            try {
                Destroy(note1);
            } catch (System.Exception) {}
            try {
                Destroy(note2);
            } catch (System.Exception) {}
        }
    }
}
