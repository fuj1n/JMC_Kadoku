using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public float timePerTurn = 1F;
    public float fastForwardMultiplier = 4;

    public Color runningHighlight;

    public bool IsRunning { get; private set; }

    public StartAction playerStart;
    public StartAction enemyStart;

    private float currentSpeed;

    private float timer;

    private ActionBase playerAction;
    private ActionBase enemyAction;

    private void Update()
    {
        if (!IsRunning)
            return;

        timer += Time.deltaTime;
        if (timer > currentSpeed)
        {
            timer = 0;
            Next();
        }
    }

    public void Run()
    {
        if (!IsRunning)
        {
            playerAction = playerStart;
            enemyAction = enemyStart;

            if (playerAction)
                playerAction.GetManager().SetOutline(runningHighlight, 0F);
            if (enemyAction)
                enemyAction.GetManager().SetOutline(runningHighlight, 0F);
        }

        IsRunning = true;
        currentSpeed = timePerTurn;
    }

    public void FastForward()
    {
        if (!IsRunning)
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
            Run();
        Pause();

        Next();
    }

    private void Next()
    {
        if (playerAction)
        {
            playerAction.Execute();

            playerAction.GetManager().SetOutline(new Color(), currentSpeed * .5F);
            playerAction = playerAction.GetNextAction();
            if (playerAction)
                playerAction.GetManager().SetOutline(runningHighlight, currentSpeed * .5F);
        }

        if (enemyAction)
        {
            enemyAction.Execute();
            enemyAction.GetManager().SetOutline(new Color(), currentSpeed * .5F);
            enemyAction = enemyAction.GetNextAction();
            if (enemyAction)
                enemyAction.GetManager().SetOutline(runningHighlight, currentSpeed * .5F);
        }
    }
}
