using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseLevel : MonoBehaviour
{
    public MainMenuManager menu;
    public GameObject discoPrefab;
    public FadeScreen fadeScreen;
    public List<string> levels;
    GameObject[] apples;
    GameObject[] rottenApples;

    public void SpawnDiscos()
    {
        menu.blocked = true;
        Transform text;
        Vector3 pos;


        for (int i = 0; i < levels.Count; i++)
        {
            pos = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
            text = Instantiate(transform.GetChild(0));
            GameObject disco = Instantiate(discoPrefab, pos, Quaternion.identity, transform);
            text.parent = disco.transform;
            text.GetComponent<TextMesh>().text = levels[i];
            text.gameObject.SetActive(true);
            text.position = new Vector3(transform.position.x + 0.5f, transform.position.y + 0.25f, transform.position.z);
            disco.AddComponent<MenuDiscoLevel>();
        }

        HideApples();
    }

    void HideApples()
    {
        apples = GameObject.FindGameObjectsWithTag("Apple");
        rottenApples = GameObject.FindGameObjectsWithTag("RottenApple");

        for (int i = 0; i < apples.Length; i++)
        {
            Destroy(apples[i]);
        }

        for (int i = 0; i < rottenApples.Length; i++)
        {
            Destroy(rottenApples[i]);
        }
    }

    public void LaunchLevel()
    {
        fadeScreen.FadeInAndStartGame();
    }
}
