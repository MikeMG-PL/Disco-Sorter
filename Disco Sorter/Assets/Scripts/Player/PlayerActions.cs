using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    // Do obsługi soczystości - potwierdzania trafienia w rytm na ekranie
    [Header("Vignette")]
    public OnScreen onScreen;
    public LevelManager levelManager;

    public void CheckActionTime(ObjectParameters parameters, bool thisIsGrabbingOrDisco)
    {
        float timer = LevelManager.timer;
        float actionStart, actionEnd;

        switch (thisIsGrabbingOrDisco)
        {
            case true:
                actionStart = parameters.actionStartTime; actionEnd = parameters.actionEndTime;

                if (timer >= actionStart && timer <= actionEnd && !parameters.wasCatchedOnTime)
                {
                    onScreen.HighlightVignette(ActionHighlight.Success);
                    parameters.wasCatchedOnTime = true;
                }

                else if((timer < actionStart || timer > actionEnd))
                    onScreen.HighlightVignette(ActionHighlight.Fail);
                break;

            case false:
                if (parameters.action != EntityAction.CatchAndRelease) return;
                actionStart = parameters.linkedReleaseTimeStart; actionEnd = parameters.linkedReleaseTimeEnd;

                if (timer >= actionStart - 0.16f && timer <= actionEnd + 0.4f)
                {
                    onScreen.HighlightVignette(ActionHighlight.Success);
                    parameters.wasReleasedOnTime = true;
                }
                else if(levelManager.spawnPipeline[parameters.linkedReleaseId] != null)
                {
                    onScreen.HighlightVignette(ActionHighlight.Fail);
                }
                break;
        }
    }
}
