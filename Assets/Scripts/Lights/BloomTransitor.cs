using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomTransitor : MonoBehaviour
{
    private float m_defaultIntensity;
    private float m_targetIntensity = 100.0f;
    private float m_transitionSpeed = 3.0f;

    private float m_defaultTreshold;
    private float m_targetTreshold = 0.0f;

    private Bloom m_bloom;

    public bool m_shouldGlow;

    void Start()
    {
        if (GetComponent<Volume>().profile.TryGet<Bloom>(out m_bloom))
        {
            m_defaultIntensity = m_bloom.intensity.value;
            m_defaultTreshold = m_bloom.threshold.value;
        }
        else
        {
            Debug.LogError("Bloom effect not found in the Volume profile.");
        }
    }

    void Update()
    {
        if(m_shouldGlow)
        {
            m_bloom.intensity.value = Mathf.Lerp(m_bloom.intensity.value, m_targetIntensity, m_transitionSpeed * Time.deltaTime);
            m_bloom.threshold.value = Mathf.Lerp(m_bloom.threshold.value, m_targetTreshold, m_transitionSpeed * Time.deltaTime);
        }
        else
        {
            m_bloom.intensity.value = Mathf.Lerp(m_bloom.intensity.value, m_defaultIntensity, m_transitionSpeed * Time.deltaTime / 2);
            m_bloom.threshold.value = Mathf.Lerp(m_bloom.threshold.value, m_defaultTreshold, m_transitionSpeed * Time.deltaTime / 2);
        }
    }
}