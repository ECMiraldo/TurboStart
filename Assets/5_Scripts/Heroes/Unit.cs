using AYellowpaper.SerializedCollections;
using System.Collections;
using UnityEngine;

public struct Damage
{
    public int amount;

}


public class DamageCalculator
{

}



public abstract class Unit : MonoBehaviour
{
    [field: SerializeField] public Animator animator { get; private set; }


    public abstract UnitStats stats { get; }
    public abstract UnitVitals unitVitals { get; }

    protected virtual void Awake()
    {

    }

    public void TakeDamage(Damage damage)
    {
        unitVitals.IncrementHealth(-damage.amount);
        //do animator related shit
    }




    public IEnumerator DoMove()
    {
        yield return null;  
    }
}


