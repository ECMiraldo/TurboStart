using System;
using UnityEngine;
using UnityUtils;
using Persistence;

public class TickManager : Singleton<TickManager>
{
    public static event Action OnTick;
    public static event Action OnNewDay;

    [SerializeField] private int ticksPerSecond = 1;
    [SerializeField] private int ticksPerDay = 60;

    private float tickInterval;
    private float tickTimer;

    public int TotalTicks { get; private set; }

    public int CurrentDay => TotalTicks / ticksPerDay;
    public int TickOfDay => TotalTicks % ticksPerDay;

    public bool IsPaused { get; private set; }

    private const long MAX_OFFLINE_SECONDS = 60 * 60 * 24 * 7; // 7 days cap

    protected override void Awake()
    {
        base.Awake();
        tickInterval = 1f / ticksPerSecond;
    }

    private void Start()
    {
        SimulateOfflineProgress();
    }

    // ---------------------------
    // PAUSE SYSTEM
    // ---------------------------

    public void Pause() => IsPaused = true;
    public void Resume() => IsPaused = false;

    // ---------------------------
    // OFFLINE SIMULATION
    // ---------------------------

    private void SimulateOfflineProgress()
    {
        long last = SaveLoadSystem.Instance.data.lastTickTime;
        long now = GetSafeCurrentUnixTime(last);

        long secondsPassed = now - last;

        if (secondsPassed <= 0)
            return;

        secondsPassed = Math.Min(secondsPassed, MAX_OFFLINE_SECONDS);

        int offlineTicks = (int)(secondsPassed * ticksPerSecond);

        AdvanceTicks(offlineTicks);

        Debug.Log($"Simulated {offlineTicks} offline ticks. Day {CurrentDay}");
    }

    // ---------------------------
    // MAIN LOOP
    // ---------------------------

    private void Update()
    {
        if (IsPaused)
            return;

        tickTimer += Time.deltaTime;

        while (tickTimer >= tickInterval)
        {
            tickTimer -= tickInterval;

            AdvanceTicks(1);

            SaveLoadSystem.Instance.data.lastTickTime =
                DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
    }

    // ---------------------------
    // CORE SIMULATION
    // ---------------------------

    private void AdvanceTicks(int ticks)
    {
        for (int i = 0; i < ticks; i++)
        {
            TotalTicks++;

            OnTick?.Invoke();

            if (TotalTicks % ticksPerDay == 0)
            {
                OnNewDay?.Invoke();
            }
        }
    }

    // ---------------------------
    // TIME SAFETY (ANTI-CHEAT / CLOCK FIX)
    // ---------------------------

    private long GetSafeCurrentUnixTime(long lastValidTime)
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // If system clock went backwards → ignore it
        if (now < lastValidTime)
        {
            Debug.LogWarning($"System time rollback detected. Using last valid time.");
            return lastValidTime;
        }

        return now;
    }

    // ---------------------------
    // UTIL
    // ---------------------------

    public int TicksToSeconds(int ticks)
    {
        return ticks / ticksPerSecond;
    }
}