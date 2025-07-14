using Newtonsoft.Json;
using System;
using UnityEngine;

public enum HeroActionType : byte
{ 

    Training = 0,
    Fighting = 1,
    Recovering = 3,
    Tavern = 4,



}


[Serializable]
public class HeroAction
{
    public event Action<HeroAction> OnActionFinished;
    [field: SerializeField] public int currentTick { get; private set; }
    [field: SerializeField] public int durationInTicks { get; private set; }
    [field: SerializeField] public HeroActionType type { get; private set; }
    [JsonIgnore] public int ticksRemaining => durationInTicks - currentTick;

    [JsonConstructor]
    public HeroAction(int currentTick, int durationInTicks, HeroActionType type)
    {
        this.currentTick = currentTick;
        this.durationInTicks = durationInTicks;
        this.type = type;
        TickManager.Instance.OnTick += Tick;
    }

    public HeroAction(int durationInTicks, HeroActionType type)
    {
        this.durationInTicks = durationInTicks;
        this.currentTick = 0;
        this.type = type;
        TickManager.Instance.OnTick += Tick;
    }

    ~HeroAction()
    {
        TickManager.Instance.OnTick -= Tick;
    }

    private void Tick()
    {
        currentTick++;
        if (currentTick >= durationInTicks) Finish();
    }
    private void Finish()
    {
        TickManager.Instance.OnTick -= Tick;
        OnActionFinished?.Invoke(this);
    }

}


