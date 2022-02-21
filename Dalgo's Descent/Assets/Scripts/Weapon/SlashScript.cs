using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    ParticleSystem SlashParticle;
    public Collider SlashCollider;

    private void Awake()
    {
        SlashParticle = GetComponent<ParticleSystem>();
        SlashCollider = GetComponent<Collider>();
    }
}
