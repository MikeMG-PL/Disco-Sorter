using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public List<ArrowLights> redLights, greenLights, yellowLights;
    public enum Hand { Left, Right };
    [Header("-----------")]
    public LevelManager levelManager;
    public HandEvents leftHand, rightHand;
    
    ObjectParameters leftParameters, rightParameters;

    [HideInInspector()]
    public bool isDoneLeft, isDoneRight;
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

        switch (h)
        {
            case Hand.Left:
                if (leftParameters != null && levelManager.spawnPipeline[leftParameters.linkedReleaseId].gameObject != null &&
                !levelManager.spawnPipeline[leftParameters.linkedReleaseId].GetComponent<ObjectParameters>().wasReleased
                && LevelManager.timer >= (leftParameters.linkedReleaseTimeStart + leftParameters.linkedReleaseTimeEnd) / 2 - 1.11f &&
                levelManager.spawnPipeline[leftParameters.linkedReleaseId].transform.childCount > 0 && !isDoneLeft)
                {
                    isDoneLeft = true;
                    Proceed(leftParameters, Hand.Left);
                }
                break;
            case Hand.Right:
                if (rightParameters != null && levelManager.spawnPipeline[rightParameters.linkedReleaseId].gameObject != null &&
                !levelManager.spawnPipeline[rightParameters.linkedReleaseId].GetComponent<ObjectParameters>().wasReleased
                && LevelManager.timer >= (rightParameters.linkedReleaseTimeStart + rightParameters.linkedReleaseTimeEnd) / 2 - 1.11f &&
                levelManager.spawnPipeline[rightParameters.linkedReleaseId].transform.childCount > 0 && !isDoneRight)
                {
                    isDoneRight = true;
                    Proceed(rightParameters, Hand.Right);
                }
                break;
            default:
                break;
        }
    }

    void Proceed(ObjectParameters o, Hand hand)
    {
        if (o.type == EntityType.RottenApple)
        {
            Enlighten(Light.Yellow, hand);
        }
        if (o.type == EntityType.Apple)
        {
            switch (o.color)
            {
                case EntityColour.Green:
                    Enlighten(Light.Green, hand);
                    break;

                case EntityColour.Red:
                    Enlighten(Light.Red, hand);
                    break;

                default:
                    break;
            }
        }
    }

    void Enlighten(Light l, Hand hand)
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
            StartCoroutine(list[i].fixedBlinkBloom(l, hand));
            StartCoroutine(list[i].fixedBlinkColor(l, hand));
        }
    }

    private void FixedUpdate()
    {
        Check();
        HandCheck(Hand.Left);
        HandCheck(Hand.Right);
    }
}
