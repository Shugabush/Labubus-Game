using UnityEngine;

[System.Serializable]
public struct Timer
{
    [field: SerializeField]
    public float TimeLimit { get; private set; }

    public float TimeElapsed { get; private set; }

    public float TimeRemaining => TimeLimit - TimeElapsed;

    public float FractionOfTimeElapsed => TimeElapsed / TimeLimit;
    public float FractionOfTimeRemaining => TimeRemaining / TimeLimit;
    public bool Running => TimeElapsed < TimeLimit;
    public bool OutOfTime => TimeElapsed >= TimeLimit;

    public Timer(float timeLimit)
    {
        TimeLimit = timeLimit;
        TimeElapsed = 0f;
    }

    public void Update(float time)
    {
        TimeElapsed += time;
    }

    public void Reset()
    {
        this = new Timer(TimeLimit);
    }
}
