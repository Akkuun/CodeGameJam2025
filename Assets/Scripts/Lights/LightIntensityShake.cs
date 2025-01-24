using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightIntensityShake : MonoBehaviour
{
    private Light2D m_lightComponent;
    public float m_min;
    public float m_max;
    public float m_fadeSpeed = 0.1f;
    public bool m_isFading;
    public bool m_isShaking;

    private bool m_isLoaded;
    private float m_intensity;
    private float m_targetIntensity;

    void Start()
    {
        m_lightComponent = GetComponent<Light2D>();
        if (m_lightComponent != null)
        {
            m_isLoaded = true;
            m_isShaking = true;
            m_isFading = false;
            m_intensity = m_lightComponent.intensity;
        }
    }

    void Update()
    {
        if (m_isLoaded && m_isShaking)
        {
            if (m_isFading)
            {
                m_intensity = Mathf.Lerp(m_intensity, m_targetIntensity, m_fadeSpeed);
                m_lightComponent.intensity = Random.Range(m_intensity * m_min, m_intensity * m_max);
            }
            else
            {
                m_lightComponent.intensity = Random.Range(m_intensity * m_min, m_intensity * m_max);
            }
        }
    }

    public void StartFading(float _targetIntensity)
    {
        m_targetIntensity = _targetIntensity;
        m_isFading = true;
    }

    public void StopFading()
    {
        m_isFading = false;
    }

    public void SetIntensity(float _intensity)
    {
        m_intensity = _intensity;
    }

    public void SetShake(bool _bool)
    {
        m_isShaking = _bool;
    }
}
