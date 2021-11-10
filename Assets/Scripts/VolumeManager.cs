using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class VolumeManager : MonoBehaviour
{
    public Volume volume { get; private set; }

    [ColorUsageAttribute(true, true)]
    public Color takeDamageTint = Color.red;
    public float takeDamageDuration = 1f;
    private float takeDamageTime = 1f;
    private ColorAdjustments colorAdjustments;

    // Start is called before the first frame update
    void Awake()
    {
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet<ColorAdjustments>(out colorAdjustments) == false)
        {
            Debug.LogError("ERROR: couldn't get ColorAdjustments from the Volume");
        }
    }
    private void Update()
    {
        if (takeDamageTime < 1f)
        {
            takeDamageTime += Time.deltaTime / takeDamageDuration;
            colorAdjustments.colorFilter.value = Color.Lerp(takeDamageTint, Color.white, takeDamageTime);
        }
    }

    private void OnEnable()
    {
        PlayerHealth.OnPlayerTakesHit += OnPlayerTakesHit;
    }
    private void OnDestroy()
    {
        PlayerHealth.OnPlayerTakesHit -= OnPlayerTakesHit;
    }

    void OnPlayerTakesHit(float damage = 0)
    {
        colorAdjustments.colorFilter.value = takeDamageTint;
        takeDamageTime = 0f;
    }
}
