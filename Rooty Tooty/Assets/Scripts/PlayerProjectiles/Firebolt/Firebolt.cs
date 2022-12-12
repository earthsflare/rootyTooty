using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Firebolt : PlayerProjectile
{
    [SerializeField] private AudioSource impactSound;
    // Coroutine for impact animation
    protected override IEnumerator ImpactAnimation()
    {
        projectileAnimator.Play("Firebolt_Impact");
        impactSound.Play();
        return base.ImpactAnimation();
    }
}
