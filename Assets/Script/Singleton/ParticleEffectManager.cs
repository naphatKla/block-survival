using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using Unity.Mathematics;
using UnityEngine;

public class ParticleEffectManager : MonoSingleton<ParticleEffectManager>
{

    void Awake()
    {
        Instance.enabled = true;
    }
    
    public void PlayParticleEffect(ParticleSystem particleEffect, Vector3 position)
    {
        Destroy(Instantiate(particleEffect, position, Quaternion.identity).gameObject, particleEffect.main.startLifetime.constantMax);
    }
    
    public void PlayParticleEffect(ParticleSystem particleEffect, Vector3 position, Transform parent)
    {
        Destroy(Instantiate(particleEffect, position, Quaternion.identity,parent).gameObject, particleEffect.main.startLifetime.constantMax);
    }
}
