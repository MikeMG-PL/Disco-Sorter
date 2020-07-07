using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public List<ArrowLights> redLights, greenLights, yellowLights;
    enum Hand { Left, Right };
    [Header("-----------")]
    public LevelManager levelManager;
    public HandEvents leftHand, rightHand;
    
    ObjectParameters leftParameters, rightParameters;

    [HideInInspector()]
    public bool isDone;
    public enum Light { None, Red, Green, Yellow };
    ///////////////////////////////////////////////////////////////////////

    void Check()
    {
        if (leftHand.parameters != null)
            leftParameters = leftHand.parameters;
        else leftParameters = null;

        if (rightHand.parameters != null)
            rightParameters = rightHand.parameters;
        else rightParameters = null;
    }

    void HandCheck(Hand h)
    {
        ObjectParameters o;

        switch (h)
        {
            case Hand.Left:
                o = leftParameters;
                break;
            case Hand.Right:
                o = rightParameters;
                break;
            default:
                o = leftParameters;
                break;
        }

        if (o != null && levelManager.spawnPipeline[o.linkedReleaseId].gameObject != null &&
                !levelManager.spawnPipeline[o.linkedReleaseId].GetComponent<ObjectParameters>().wasReleased
                && LevelManager.timer >= (o.linkedReleaseTimeStart + o.linkedReleaseTimeEnd) / 2 - 1.11f &&
                levelManager.spawnPipeline[o.linkedReleaseId].transform.childCount > 0 && !isDone)
        {
            isDone = true;

            if (o.type == EntityType.RottenApple)
            {
                Enlighten(Light.Yellow);
            }
            if (o.type == EntityType.Apple)
            {
                switch (o.color)
                {
                    case EntityColour.Green:
                        Enlighten(Light.Green);
                        break;

                    case EntityColour.Red:
                        Enlighten(Light.Red);
                        break;

                    default:
                        break;
                }
            }
        }
    }

    void Enlighten(Light l)
    {
        List<ArrowLights> list;

        switch (l)
        {
            case Light.Red:
                list = redLights;
                break;

            case Light.Green:
                list = greenLights;
                break;

            case Light.Yellow:
                list = yellowLights;
                break;

            default:
                list = null;
                break;
        }

        for (int i = 0; i < list.Count; i++)
        {
            StartCoroutine(list[i].fixedBlinkBloom(l));
            StartCoroutine(list[i].fixedBlinkColor(l));
        }
    }

    private void FixedUpdate()
    {
        Check();
        HandCheck(Hand.Left);
        HandCheck(Hand.Right);
    }
}
