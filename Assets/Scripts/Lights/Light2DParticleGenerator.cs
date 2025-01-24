using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class Light2DParticleGenerator : MonoBehaviour
{
    public GameObject m_prefab;

    private ParticleSystem m_particleSystem;
    private List<GameObject> m_instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_particles;
    private Dictionary<string, float> m_lightOuterRadius = new Dictionary<string, float>();

    void Start()
    {
        m_prefab.transform.Rotate(90, 0, 0);
        m_particleSystem = GetComponent<ParticleSystem>();
        m_particles = new ParticleSystem.Particle[m_particleSystem.main.maxParticles];
        CalculatePrefabOuterRadius();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_particleSystem.GetParticles(m_particles);
        while (m_instances.Count < count)
        {
            m_instances.Add(Instantiate(m_prefab, m_particleSystem.transform));
            m_instances[m_instances.Count - 1].transform.rotation = GetPrefabRotation();
        }

        bool worldSpace = (m_particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                {
                    m_instances[i].transform.position = m_particles[i].position;
                }
                else
                {
                    m_instances[i].transform.localPosition = m_particles[i].position;
                }
                m_instances[i].transform.rotation = GetPrefabRotationWithRotationYZ(m_particles[i].rotation3D.y, m_particles[i].rotation3D.z);

                float scale = m_particles[i].GetCurrentSize(m_particleSystem);
                m_instances[i].transform.localScale = m_prefab.transform.localScale * scale;

                AdjustLightOuterRadius(m_instances[i], scale);

                m_instances[i].SetActive(true);
            }
            else
            {
                m_instances[i].SetActive(false);
            }
        }
    }

    private Quaternion GetPrefabRotation()
    {
        return Quaternion.Euler(0, m_prefab.transform.rotation.y, m_prefab.transform.rotation.z);
    }

    private Quaternion GetPrefabRotationWithRotationYZ(float _y, float _z)
    {
        return Quaternion.Euler(0, m_prefab.transform.rotation.y - _y, m_prefab.transform.rotation.z - _z);
    }

    void CalculatePrefabOuterRadius()
    {
        Light2D[] prefabLights = m_prefab.GetComponentsInChildren<Light2D>(true);

        foreach (Light2D light in prefabLights)
        {
            m_lightOuterRadius[light.gameObject.name] = light.pointLightOuterRadius;
        }
    }

    void AdjustLightOuterRadius(GameObject obj, float scale)
    {
        Light2D[] lights = obj.GetComponentsInChildren<Light2D>(true);
        foreach (Light2D light in lights)
        {
            string lightName = light.gameObject.name;
            if (m_lightOuterRadius.ContainsKey(lightName))
            {
                light.pointLightOuterRadius = m_lightOuterRadius[lightName] * scale;
            }
        }
    }
}
