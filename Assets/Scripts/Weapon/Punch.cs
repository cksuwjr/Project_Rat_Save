using System.Collections;

public class Punch : Skill
{
    public float damage = 40;
    public int count = 1;

    protected override IEnumerator Cast_()
    {
        animator?.SetTrigger("Fire1");

        yield return null;
    }
}
