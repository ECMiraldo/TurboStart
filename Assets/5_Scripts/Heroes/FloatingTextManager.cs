using UnityUtils;

public class FloatingTextManager : Singleton<FloatingTextManager>
{





    private void OnEnable()
    {
        Damage.OnDamageResolved += OnDamage;
    }

    private void OnDisable()
    {
        Damage.OnDamageResolved -= OnDamage;
    }

    private void OnDamage(Damage dmg)
    {

    }

}


