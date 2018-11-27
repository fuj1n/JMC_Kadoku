using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public float timePerTurn = 1F;
    public float fastForwardMultiplier = 4;

    public bool IsRunning { get; private set; }

    private float currentSpeed;

    private void Update()
    {
        if (!IsRunning)
            return;
    }

    public void Run()
    {
        IsRunning = true;
        currentSpeed = timePerTurn;
    }

    public void FastForward()
    {
        Run();
        currentSpeed = timePerTurn / fastForwardMultiplier;
    }

    public void Pause()
    {
        currentSpeed = 0F;
    }

    public void Step()
    {
        if (!IsRunning)
            return;
        Pause();

        Next();
    }

    private void Next()
    {

    }
}
