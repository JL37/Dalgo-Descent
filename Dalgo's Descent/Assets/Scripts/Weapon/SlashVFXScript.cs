using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SLASH_TYPE
{
    SLASH_1,
    SLASH_2,
    SLASH_3,
    CLEAVE
}

public class SlashVFXScript : MonoBehaviour
{
    [SerializeField][Range(0,5)] private double OnHitDelay;
    [SerializeField][Range(0,5)] private double HitDuration;
    public Collider SlashCollider;
    public ParticleSystem SlashParticle;

    public SLASH_TYPE SlashType;

    public List<AI> hitEnemies;

    private double DelayTimer;
    void Start()
    {
        SlashCollider.enabled = false;    
        hitEnemies = new List<AI>();
    }

    void Update()
    {
        DelayTimer += Time.deltaTime;

        if (DelayTimer >= OnHitDelay && DelayTimer < HitDuration)
        {
            transform.parent.parent = null;
            SlashCollider.enabled = true;
        }
        else
            SlashCollider.enabled = false;
    }

    private void OnParticleSystemStopped()
    {
        Destroy(transform.parent.gameObject);
    }

}
