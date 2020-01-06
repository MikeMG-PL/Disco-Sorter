using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWaveForm : MonoBehaviour
{
    private Color32 black = new Color32(0, 0, 0, 255);
    private Color32 red = new Color32(255, 0, 0, 255);

    public Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[width];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / width) + 1;
        int s = 0;
        for (int i = 0; i < audio.samples; i += packSize)
        {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }

        Color32[] blackColors = new Color32[width * height];
        for (int i = 0; i < blackColors.Length; i++)
        {
            blackColors[i] = black;
        }

        tex.SetPixels32(blackColors);

        /*for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                tex.SetPixel(x, y, Color.black);
            }
        }*/

        for (int x = 0; x < waveform.Length; x++)
        {

            Color32[] colors = new Color32[2 * (int)(waveform[x] * ((float)height * 0.5f))];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = red;
            }

            int minHight = (height / 2) - (int)(waveform[x] * ((float)height * 0.5f));

            tex.SetPixels32(x, minHight,
                1, 2 * (int)(waveform[x] * ((float)height * 0.5f)), colors);

            /*
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++)
            {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
            */
        }
        tex.Apply();

        return tex;
    }
}
