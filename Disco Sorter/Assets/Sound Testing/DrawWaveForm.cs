using UnityEngine;

public class DrawWaveForm : MonoBehaviour
{
    [SerializeField]
    private GameObject waveformObject;

    private Color32 black = new Color32(0, 0, 0, 255);
    private Color32 red = new Color32(255, 0, 0, 255);

    /// Funkcja odpowiedzialna za poprawne renderowanie i synchronizację waveformu ///
    public void Waveform()
    {
        AudioClip audioClip = GetComponent<AudioSource>().clip;
        MeshRenderer renderer = waveformObject.GetComponent<MeshRenderer>();
        GameObject[] entityArray = GetComponent<EditorNet>().entityArray;
        EditorNet editorNet = GetComponent<EditorNet>();
        float sceneSongLength = editorNet.entitiesAmount * 0.1f + editorNet.entitiesAmount * 0.005f;
        Vector3 scaleVector = new Vector3(sceneSongLength, 1, 0.1f);

        // obliczenie długosci piosenki na scenie
        // utworzenie Vectora3 skali określającego porządaną długość 
        // przypisanie quadowi nowego vectora3 skali

        renderer.gameObject.transform.localScale = scaleVector;

        //Mechanizm skalowania tekstury w zależności od długości piosenki i gęstości siatki
        if ((int)sceneSongLength * 150 >= 60000)
            renderer.material.mainTexture = PaintWaveformSpectrum(audioClip, 1f, 16000, 1000, Color.red);
        else if ((int)sceneSongLength * 150 >= 48000 && (int)sceneSongLength * 150 < 60000)
            renderer.material.mainTexture = PaintWaveformSpectrum(audioClip, 1f, (int)sceneSongLength * 40, 1000, Color.red);
        else if ((int)sceneSongLength * 150 >= 36000 && (int)sceneSongLength * 150 < 48000)
            renderer.material.mainTexture = PaintWaveformSpectrum(audioClip, 1f, (int)sceneSongLength * 50, 1000, Color.red); // liczba 150 - czułość wyświetlania waveformu
        else if ((int)sceneSongLength * 150 >= 24000 && (int)sceneSongLength * 150 < 36000)
            renderer.material.mainTexture = PaintWaveformSpectrum(audioClip, 1f, (int)sceneSongLength * 65, 1000, Color.red);
        else if ((int)sceneSongLength * 150 >= 16000 && (int)sceneSongLength * 150 < 24000)
            renderer.material.mainTexture = PaintWaveformSpectrum(audioClip, 1f, (int)sceneSongLength * 100, 1000, Color.red);
        else if ((int)sceneSongLength * 150 < 16000)
            renderer.material.mainTexture = PaintWaveformSpectrum(audioClip, 1f, (int)sceneSongLength * 150, 1000, Color.red);

        renderer.gameObject.transform.position = new Vector3(entityArray[0].transform.position.x - 0.05f, entityArray[0].transform.position.y, entityArray[0].transform.position.z + 0.55f);
        renderer.material.mainTexture.filterMode = FilterMode.Point;
    }

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
