using UnityEngine;

public class InteractableObstacleFX : MonoBehaviour
{

    public ParticleSystem m_particleSystem;
    public bool m_isEasilyTriggerable = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(m_particleSystem == null)
        {
            Debug.LogError("Particle system is null.");
            return;
        }
        m_particleSystem.Clear();
        m_particleSystem.Stop();
    }

    public void Play()
    {
        if(m_particleSystem == null)
        {
            return;
        }
        m_particleSystem.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!m_isEasilyTriggerable)
        {
            return;
        }
        // Check if the object in collision is the player
        PlayerController player = collision.GetComponent<PlayerController>();

        if (player != null)
        {
            Play();
        }
    }
}
