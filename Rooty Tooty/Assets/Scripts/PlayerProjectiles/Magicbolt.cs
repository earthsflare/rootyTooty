using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magicbolt : PlayerProjectile
{
    // Coroutine for impact animation
    protected override IEnumerator ImpactAnimation()
    {
        projectileAnimator.Play("Player Shoot Impact Animation");
        return base.ImpactAnimation();
    }
}
