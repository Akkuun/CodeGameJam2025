using System.Threading.Tasks;
using UnityEngine;

public class InteractableObstacleFX : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem m_particleSystem;
    
    [SerializeField]
    private ParticleSystem m_playingParticleSystem;
    [SerializeField]
    private bool m_isEasilyTriggerable = true;

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
    [SerializeField]
    public async Task Play()
    {
        if(m_particleSystem == null)
        {
            return;
        }
        m_particleSystem.Play();
        if(m_playingParticleSystem != null)
        {
            //! Do not work
            ParticleSystem.VelocityOverLifetimeModule velocityModule = m_playingParticleSystem.velocityOverLifetime;
            velocityModule.enabled = true;

            velocityModule.xMultiplier *= 8;

            await Task.Delay(1000);

            velocityModule.xMultiplier /= 8;
        }
    
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
