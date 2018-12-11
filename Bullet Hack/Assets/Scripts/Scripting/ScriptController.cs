﻿using UnityEngine;

public class ScriptController : MonoBehaviour
{
    public float timePerTurn = 1F;
    public float fastForwardMultiplier = 4;

    public Color runningHighlight;

    public bool IsRunning { get; private set; }

    public StartAction playerStart;
    public StartAction enemyStart;

    public ScriptableCharacter playerAvatar;
    public ScriptableCharacter enemyAvatar;

    [HideInInspector]
    public ScriptableCharacter currentAvatar;

    private float currentSpeed;

    private float timer;

    private ActionBase playerAction;
    private ActionBase enemyAction;

    private void Update()
    {
        if (!IsRunning)
            return;

        timer -= Time.deltaTime;
        if (float.IsInfinity(currentSpeed))
            return;

        if (timer <= 0F)
        {
            timer = currentSpeed;
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
        currentSpeed = float.PositiveInfinity;
    }

    public void Step()
    {
        if (!IsRunning)
            Run();
        Pause();

        if (timer <= 0F)
            Next();
    }

    public float GetTweenSpeed()
    {
        float tweenSpeed = currentSpeed;
        if (float.IsInfinity(tweenSpeed))
            tweenSpeed = .02F;

        return tweenSpeed;
    }

    private void Next()
    {
        float tweenSpeed = GetTweenSpeed();

        playerAvatar.tweenSpeed = tweenSpeed;
        enemyAvatar.tweenSpeed = tweenSpeed;

        if (playerAction)
        {
            currentAvatar = playerAvatar;
            playerAction.Execute();

            playerAction.GetManager().FadeOutline(0F, tweenSpeed * .5F);
            playerAction = playerAction.GetNextAction();
            if (playerAction)
                playerAction.GetManager().SetOutline(runningHighlight, tweenSpeed * .5F);
        }

        if (enemyAction)
        {
            currentAvatar = enemyAvatar;
            enemyAction.Execute();
            enemyAction.GetManager().FadeOutline(0F, tweenSpeed * .5F);
            enemyAction = enemyAction.GetNextAction();
            if (enemyAction)
                enemyAction.GetManager().SetOutline(runningHighlight, tweenSpeed * .5F);
        }
    }
}
