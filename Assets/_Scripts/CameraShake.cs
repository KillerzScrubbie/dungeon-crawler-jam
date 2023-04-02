using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;
    CinemachineBasicMultiChannelPerlin perlin;
    float shakeTimer;
    float shakeTotal;
    float startIntensity;

    [SerializeField] float _intensity;
    [SerializeField] float _duration;

    private void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
        perlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        perlin.m_AmplitudeGain = intensity;
        startIntensity = intensity;
        shakeTimer = time;
        shakeTotal = time;
    }

    private void Update()
    {
        if (shakeTimer <= 0) return;
        shakeTimer -= Time.deltaTime;
        perlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - (shakeTimer / shakeTotal));
    }

    void ShakeOnDamage(int dmg)
    {
        ShakeCamera(_intensity * dmg, _duration);
    }

    void OnEnable()
    {
        PlayerHp.OnPlayerTakeDamage += ShakeOnDamage;
    }
    void OnDisable()
    {
        PlayerHp.OnPlayerTakeDamage -= ShakeOnDamage;
    }
}
