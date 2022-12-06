using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebolt : PlayerProjectile
{
    // Coroutine for impact animation
    protected override IEnumerator ImpactAnimation()
    {
        projectileAnimator.Play("Firebolt_Impact");
        return base.ImpactAnimation();
    }
}
