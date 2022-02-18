using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public List<BoxCollider> Colliders;
    public float BaseDamage;

    public delegate void OnWeaponHitDelegate(float basedamage, Vector3 position);
    public event OnWeaponHitDelegate OnWeaponHitListeners;

    // Start is called before the first frame update
    void Start()
    {
        foreach(var collider in GetComponents<BoxCollider>())
        {
            Colliders.Add(collider);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
         if (OnWeaponHitListeners != null) OnWeaponHitListeners.Invoke(BaseDamage, transform.position);
    }
}
