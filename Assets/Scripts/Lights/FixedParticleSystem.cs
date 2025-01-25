using UnityEngine;

public class FixedParticleSystem : MonoBehaviour
{
    private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.simulationSpace = ParticleSystemSimulationSpace.Local;
    }

    void Update()
    {
        // Vous pouvez ajouter ici des logiques supplémentaires si nécessaire
    }
}
