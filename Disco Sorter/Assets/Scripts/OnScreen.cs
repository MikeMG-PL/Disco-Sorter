﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreen : MonoBehaviour
{
    public Material vignette; public Color r, g; float alpha; bool highlighted; public float vignetteFadeSpeed = 10;

    SFXManager sfx;
    [Header("-------------------")]
    public TextMesh songTitle;
    public TextMesh songArtist;
    public MeshRenderer titleMesh;
    public MeshRenderer artistMesh;

    [Header("-------------------")]
    public SpriteRenderer logo;
    public TextMesh scoreText;
    public MeshRenderer scoreMesh;

    [Header("-------------------")]
    public LoadToScene loadToScene;
    //////////////////////////////

    public float fadeSpeed = 10;
    float alphaLogo = 0, alphaText = 0;
    [HideInInspector()]
    public bool showLogo, showPoints;

    PointManager pointManager;

    private void Awake()
    {
        pointManager = GameObject.FindGameObjectWithTag("PointManager").GetComponent<PointManager>();
        StartCoroutine(Cor());
        sfx = GetComponent<SFXManager>();
    }

    void Start()
    {
        vignette.color = new Color(0, 0, 0, 0);
        //g = new Color(0, 1, 0, alpha);
        //r = new Color(1, 0, 0, alpha);

        songTitle.text = loadToScene.level.name;
        songArtist.text = loadToScene.level.artist;
    }

    IEnumerator Cor()
    {
        songTitle.gameObject.SetActive(true);
        songTitle.color = new Color(songTitle.color.r, songTitle.color.g, songTitle.color.b, alphaLogo);
        songArtist.gameObject.SetActive(true);
        songArtist.color = new Color(songArtist.color.r, songArtist.color.g, songArtist.color.b, alphaLogo);
        scoreMesh.gameObject.SetActive(true);
        scoreMesh.material.color = new Color(scoreMesh.material.color.r, scoreMesh.material.color.g, scoreMesh.material.color.b, alphaText);

        yield return new WaitForSeconds(2);

        showLogo = true;

        yield return new WaitForSeconds(3);

        showLogo = false;

        yield return new WaitForSeconds(2);

        showPoints = true;
    }

    void Update()
    {
        if (showLogo)
        {
            titleMesh.material.color = new Color(titleMesh.material.color.r, titleMesh.material.color.g, titleMesh.material.color.b, alphaLogo);
            artistMesh.material.color = new Color(artistMesh.material.color.r, artistMesh.material.color.g, artistMesh.material.color.b, alphaLogo);
            if (alphaLogo < 1)
            {
                alphaLogo += fadeSpeed * Time.deltaTime;
            }
        }
        else
        {
            titleMesh.material.color = new Color(titleMesh.material.color.r, titleMesh.material.color.g, titleMesh.material.color.b, alphaLogo);
            artistMesh.material.color = new Color(artistMesh.material.color.r, artistMesh.material.color.g, artistMesh.material.color.b, alphaLogo);
            if (alphaLogo > 0)
            {
                alphaLogo -= fadeSpeed * Time.deltaTime;
            }
        }

        if (showPoints)
        {
            scoreMesh.material.color = new Color(scoreMesh.material.color.r, scoreMesh.material.color.g, scoreMesh.material.color.b, alphaText);
            if (alphaText < 1)
            {
                alphaText += fadeSpeed * Time.deltaTime;
            }
        }
        else
        {
            scoreMesh.material.color = new Color(scoreMesh.material.color.r, scoreMesh.material.color.g, scoreMesh.material.color.b, alphaText);
            if (alphaText > 0)
            {
                alphaText -= fadeSpeed * Time.deltaTime;
            }
        }
        //////////////////////////////////////////////////
        if (loadToScene == null)
            Debug.LogError("There is no LoadToScene assigned in OnScreen gameobject.");
    }

    public void HighlightVignette(ActionHighlight h)
    {
        switch (h)
        {
            case ActionHighlight.Success:

                sfx.PlaySound(sfx.onTime);
                vignette.color = g;
                pointManager.OnTime();

                StartCoroutine(VignetteAnim(true));
                break;

            case ActionHighlight.Fail:

                sfx.PlaySound(sfx.wrong);
                vignette.color = r;
                pointManager.Punish();

                StartCoroutine(VignetteAnim(false));
                break;

            default:
                vignette.color = new Color(0, 0, 0, 0);
                break;
        }

        
    }

    public IEnumerator VignetteAnim(bool withEnviroLight)
    {
        if (!highlighted)
        {
            highlighted = true;
            float maxAlpha = 1f, maxLight = 1f, minLight = 0.3f;

            while (alpha <= maxAlpha)
            {
                vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
                vignette.SetColor("_EmissionColor", vignette.color);
                alpha += vignetteFadeSpeed * Time.fixedDeltaTime;

                if (withEnviroLight && RenderSettings.reflectionIntensity < maxLight)
                    RenderSettings.reflectionIntensity += vignetteFadeSpeed * Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            while (alpha > 0)
            {
                vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, alpha);
                vignette.SetColor("_EmissionColor", vignette.color);
                alpha -= vignetteFadeSpeed * Time.fixedDeltaTime;
                if (withEnviroLight && RenderSettings.reflectionIntensity > minLight)
                    RenderSettings.reflectionIntensity -= vignetteFadeSpeed * Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            highlighted = false;
        }
    }
}