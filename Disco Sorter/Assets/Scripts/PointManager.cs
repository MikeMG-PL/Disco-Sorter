using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    public int onTimePoints, punishPoints, comboMultiplier, comboAfterStreak, rottenDistanceMultiplier, wrongBoxPunishment, noBoxPunishment, wrongActionsNeededToFail, correctBoxPoints;
    public OnScreen onScreen;

    int points, beforeComboCounter, combo, rottenDistance; Queue<bool> actionQueue; public enum AppleState { IncorrectBox, CorrectBox, NoBox, RottenThrow }; public bool levelFailed;
    public LevelManager levelManager; bool stopListening; public bool godMode;

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
                points += correctBoxPoints;
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
        if (godMode == false && levelFailed == false)
        {   
            for(int i = 0; i < levelManager.spawnPipeline.Count; i++)
            {
                if (levelManager.spawnPipeline[i] != null)
                    levelManager.spawnPipeline[i].GetComponent<ObjectMethods>().dissolve = true;
            }
            levelFailed = true;
            StartCoroutine(FailEffect());
        }
    }

    public void DisplayPoints()
    {
        if (combo > 0 && levelFailed == false)
        {
            onScreen.scoreText.text = points.ToString() + "\ncombo: " + combo;
            onScreen.scoreMesh.transform.localScale = new Vector3(0.15f, 0.15f);
        }
        else if (levelFailed == false)
        {
            onScreen.scoreText.text = points.ToString();
            onScreen.scoreMesh.transform.localScale = new Vector3(0.2f, 0.2f);
        }
    }

    public void PointListener()
    {
        if (points < 0 && actionQueue.Count >= 5 && !stopListening)
            FailLevel();
    }

    IEnumerator FailEffect()
    {
        for (int i = 0; i < levelManager.spawnPipeline.Count; i++)
        {
            if (levelManager.spawnPipeline[i] != null && levelFailed)
            {

                if (levelManager.spawnPipeline[i].CompareTag("DiscoBall") && levelManager.spawnPipeline[i].activeSelf)
                    levelManager.spawnPipeline[i].GetComponent<ObjectMethods>().DestroyDisco();
                else if(levelManager.spawnPipeline[i].transform.childCount > 0)
                {
                    levelManager.spawnPipeline[i].transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material =
                        levelManager.spawnPipeline[i].GetComponent<ObjectMethods>().dissolveMaterial;
                    StartCoroutine(levelManager.spawnPipeline[i].GetComponent<ObjectMethods>().Dissolve());
                }

            }
        }

        float timer = 0;
        onScreen.showPoints = false;

        while (Time.timeScale > 0.1)
        {
            timer += Time.fixedDeltaTime;
            if (timer >= 0.5f)
            {
                onScreen.scoreMesh.transform.localScale = new Vector3(0.15f, 0.15f);
                onScreen.scoreText.text = "sorting\nfailed!";
                onScreen.showPoints = true;
                stopListening = true;
            }

            Time.timeScale -= Time.fixedDeltaTime;
            levelManager.GetComponent<AudioSource>().pitch -= Time.fixedDeltaTime;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
        Time.timeScale = 0;
        levelManager.GetComponent<AudioSource>().pitch = 0;
        StopCoroutine(FailEffect());
    }
}
