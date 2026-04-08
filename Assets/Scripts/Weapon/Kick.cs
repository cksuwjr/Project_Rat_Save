using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : Skill
{
    public float damage = 40;
    public int count = 1;

    protected override IEnumerator Cast_()
    {
        animator?.SetTrigger("Fire2");

        yield return null;
    }
}
