using Persistence;
using System;
using UnityEngine;
using UnityUtils;

public class TickManager : Singleton<TickManager>
{
    public static event Action OnTick;

    [SerializeField] private int ticksPerSecond = 1;

    private float tickInterval;
    private float tickTimer;

    public int ticksToSeconds(int nTicks) => nTicks * ticksPerSecond;
    protected override void Awake()
    {
        base.Awake();
        tickInterval = 1f / ticksPerSecond;
    }

    public void Start()
    {

        // 2. Get the current UNIX timestamp (in seconds)
        long currentUnixTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // 3. Calculate how many seconds passed
        long secondsPassed = currentUnixTime - SaveLoadSystem.Instance.data.lastTickTime;

        // 4. Convert to tick count
        int offlineTicks = (int)(secondsPassed * ticksPerSecond);

        // 5. Simulate the missed ticks
        for (int i = 0; i < offlineTicks; i++)
        {
            OnTick?.Invoke();
        }

        Debug.Log($"Simulated {offlineTicks} offline ticks.");
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;

        while (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;
            SaveLoadSystem.Instance.data.lastTickTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            OnTick?.Invoke();
        }
    }
}


