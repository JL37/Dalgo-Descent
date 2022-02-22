using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashVFXScript : MonoBehaviour
{

    private void OnParticleSystemStopped()
    {
        Destroy(transform.parent.gameObject);
    }

}
