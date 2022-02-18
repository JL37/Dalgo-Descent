using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashScript : MonoBehaviour
{
    ParticleSystem SlashParticle;

    private void Awake()
    {
        SlashParticle = GetComponent<ParticleSystem>();
    }
}
