using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magicbolt : PlayerProjectile
{
    [SerializeField] private AudioSource impactSound;
    // Coroutine for impact animation
    protected override IEnumerator ImpactAnimation()
    {
        projectileAnimator.Play("Player Shoot Impact Animation");
        impactSound.Play();
        return base.ImpactAnimation();
    }
}
