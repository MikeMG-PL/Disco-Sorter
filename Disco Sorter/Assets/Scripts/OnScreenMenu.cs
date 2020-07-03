using OVR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenMenu : MonoBehaviour
{
    public MenuSide screenType;
    public MeshRenderer defaultText, programmers, graphic, musician;
    public SpriteRenderer logo;
    public float fadeSpeed = 5;

    private bool showDefaultText;
    // MainScreen
    private bool showLogo;
    // CreditsScreen
    private bool showProgrammers, showGraphic, showMusician;

    private void Awake()
    {
        if (screenType == MenuSide.Main) StartCoroutine(MainScreen());
        else if (screenType == MenuSide.Credits) StartCoroutine(CreditsScreen());
        else if (screenType == MenuSide.Settings) StartCoroutine(SettingsScreen());
    }

    private void Update()
    {
        if (screenType == MenuSide.Main)
        {
            if (showLogo) ShowElement(logo.material);
            else HideElement(logo.material);
        }

        else if (screenType == MenuSide.Credits)
        {
            if (showProgrammers) ShowElement(programmers.material);
            else HideElement(programmers.material);

            if (showGraphic) ShowElement(graphic.material);
            else HideElement(graphic.material);

            if (showMusician) ShowElement(musician.material);
            else HideElement(musician.material);
        }

        else if (screenType == MenuSide.Settings)
        {

        }

        if (showDefaultText) ShowElement(defaultText.material);
        else HideElement(defaultText.material);
    }

    private void ShowElement(Material material)
    {
        if (material.color.a < 1) SetAlpha(material, material.color.a + (fadeSpeed * Time.deltaTime));
    }

    private void HideElement(Material material)
    {
        if (material.color.a > 0) SetAlpha(material, material.color.a - fadeSpeed * Time.deltaTime);
    }

    IEnumerator MainScreen()
    {
        logo.gameObject.SetActive(true);
        SetAlpha(logo.material, 0);

        defaultText.gameObject.SetActive(true);
        SetAlpha(defaultText.material, 0);

        yield return new WaitForSeconds(1);

        showLogo = true;
        yield return new WaitForSeconds(2);
        showLogo = false;
        yield return new WaitForSeconds(1);

        showDefaultText = true;
    }

    IEnumerator CreditsScreen()
    {
        yield return new WaitForSeconds(3);
        defaultText.gameObject.SetActive(true);
        SetAlpha(defaultText.material, 0);

        yield return new WaitForSeconds(1);

        showDefaultText = true;
    }

    IEnumerator CreditsTexts()
    {
        programmers.gameObject.SetActive(true);
        SetAlpha(programmers.material, 0);

        graphic.gameObject.SetActive(true);
        SetAlpha(graphic.material, 0);

        musician.gameObject.SetActive(true);
        SetAlpha(musician.material, 0);

        showDefaultText = false;

        yield return new WaitForSeconds(1);

        showProgrammers = true;
        yield return new WaitForSeconds(3);
        showProgrammers = false;
        yield return new WaitForSeconds(1);

        showGraphic = true;
        yield return new WaitForSeconds(3);
        showGraphic = false;
        yield return new WaitForSeconds(1);

        showMusician = true;
        yield return new WaitForSeconds(3);
        showMusician = false;
        yield return new WaitForSeconds(1);

        showDefaultText = true;
    }

    IEnumerator SettingsScreen()
    {
        yield return new WaitForSeconds(3);
        defaultText.gameObject.SetActive(true);
        SetAlpha(defaultText.material, 0);

        yield return new WaitForSeconds(1);

        showDefaultText = true;
    }

    public void StartCredits()
    {
        StartCoroutine(CreditsTexts());
    }

    private void SetAlpha(Material material, float value)
    {
        Color color = material.color;
        color.a = value;
        material.color = color;
    }
}
