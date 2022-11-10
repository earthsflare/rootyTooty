using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterball : PlayerProjectile
{
    // Coroutine for impact animation
    protected override IEnumerator ImpactAnimation()
    {
        projectileAnimator.Play("Waterball_Impact");
        return base.ImpactAnimation();
    }
}
