using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public int onTimePoints, punishPoints, comboMultiplier, comboAfterStreak, rottenDistanceMultiplier, wrongBoxPunishment, noBoxPunishment, wrongActionsNeededToFail, correctBoxPoints;
    public OnScreen onScreen;

    int points, beforeComboCounter, combo, rottenDistance; Queue<bool> actionQueue; public enum AppleState { IncorrectBox, CorrectBox, NoBox, RottenThrow };

    void Start()
    {
        actionQueue = new Queue<bool>();
    }

    void Update()
    {
        DisplayPoints();
        PointListener();
    }

    public void OnTime()
    {
        points += onTimePoints;
        Counting(true, true);
    }

    public void Punish()
    {
        points += punishPoints;
        Counting(false, false);
    }

    public void ThrowPoints(AppleState state, float throwDistance)
    {
        switch (state)
        {
            case AppleState.NoBox:
                points += noBoxPunishment;
                ComboCounter(false);
                break;
            case AppleState.IncorrectBox:
                points += wrongBoxPunishment;
                ComboCounter(false);
                break;
            case AppleState.CorrectBox:
                points += correctBoxPoints;
                break;
            case AppleState.RottenThrow:
                points += (int)throwDistance * rottenDistanceMultiplier;
                break;
        }
    }

    public void ComboCounter(bool addCombo)
    {
        if (addCombo)
        {
            if (beforeComboCounter < comboAfterStreak)
                beforeComboCounter++;
            else
            {
                combo++;
                points += combo * comboMultiplier;
            }
        }
        else
        {
            combo = 0;
            beforeComboCounter = 0;
        }
    }

    public void ActionCounter(bool onTime)
    {
        Queue<bool> wrongNumber = new Queue<bool>();
        actionQueue.Enqueue(onTime);

        if (actionQueue.Count >= 11)
        {
            if (actionQueue.ToArray()[0] == false && wrongNumber.Count > 0)
                wrongNumber.Dequeue();

            actionQueue.Dequeue();
        }
        for (int i = 0; i < actionQueue.Count; i++)
        {
            if (actionQueue.ToArray()[i] == false)
                wrongNumber.Enqueue(actionQueue.ToArray()[i]);
        }

        if (wrongNumber.Count >= wrongActionsNeededToFail)
            FailLevel();
    }

    public void Counting(bool combo, bool action)
    {
        ComboCounter(combo);
        ActionCounter(action);
    }

    public void FailLevel()
    {
        Debug.LogError("LEVEL FAILED!");
    }

    public void DisplayPoints()
    {
        Debug.Log("Points: " + points + ", Combo: " + combo);
    }

    public void PointListener()
    {
        if (points < 0 && actionQueue.Count >= 5)
            FailLevel();
    }


}
