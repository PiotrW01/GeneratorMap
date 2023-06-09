using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public TMP_InputField seed;
    public Slider renderDistanceSlider;
    public TextMeshProUGUI renderDistanceText;
    public TMP_Dropdown dropDown;
    public TMP_Dropdown resDropDown;

    public TextMeshProUGUI CText;
    public TextMeshProUGUI HText;
    public TextMeshProUGUI TText;
    public TextMeshProUGUI HMText;
    public TextMeshProUGUI terrainText;
    public TextMeshProUGUI chunkPosText;
    public TextMeshProUGUI gridPosText;

    public GameObject EditWindow;
    public GameObject EditWindowContent;

    private void Start()
    {
        EditWindow.SetActive(false);
        seed.text = ChunkLoader.Instance.seed.ToString();
        renderDistanceText.text = ChunkLoader.renderDistance.ToString();
        renderDistanceSlider.value = ChunkLoader.renderDistance;

        renderDistanceSlider.onValueChanged.AddListener((v) =>
        {
            renderDistanceText.text = v.ToString();
            ChunkLoader.renderDistance = (int)v;
            ChunkLoader.Instance.ReloadChunks();
        });

        seed.onEndEdit.AddListener((v) =>
        {
            try
            {
                int newSeed = int.Parse(v);
                ChunkLoader.Instance.seed = newSeed;
            }
            catch {
                ChunkLoader.Instance.seed = int.MaxValue;
                seed.text = int.MaxValue.ToString();
            }
        });
    }

    private void FixedUpdate()
    {
        // Gets the tile at current coordinates and displays its information
        var tilemap = ChunkLoader.Instance.tilemap;
        var pos = tilemap.WorldToCell(Camera.main.transform.position);
        try
        {
        CustomTile tile = tilemap.GetTile<CustomTile>(pos);
        CText.text = "C: " + tile.continentalityValue.ToString();
        HText.text = "H: " + tile.heightValue.ToString();
        TText.text = "T: " + tile.temperatureValue.ToString();
        HMText.text = "HM: " + tile.humidityValue.ToString();
        terrainText.text = "Terrain: " + tile.terrainType.ToString();
        } catch { }
        Vector2Int chunkPos = ChunkLoader.Instance.GridToChunkCoords(pos.x, pos.y);
        chunkPosText.text = "Current chunk x: " + chunkPos.x.ToString() + " y: " + chunkPos.y.ToString();
        gridPosText.text = "Current grid pos x: " + pos.x.ToString() + " y: " + pos.y.ToString();
}
    public void ChangeDisplayedNoiseMap()
    {
        switch (dropDown.value)
        {
            case 0:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Continentality;
                break;
            case 1:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Height;
                break;
            case 2:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Temperature;
                break;
            case 3:
                ChunkLoader.Instance.selectedNoiseMap = NoiseMap.Humidity;
                break;
        }
    }

    public void ChangeResolution()
    {
        ResolutionManager.Instance.SetResolution(resDropDown.value);
    }

    public void ToggleFullscreen()
    {
        ResolutionManager.Instance.ToggleFullscreen();
    }

    public void DisplayNoise()
    {
        ChunkLoader.Instance.SwitchNoise();
    }

    // Activates editor window
    public void EditPreset()
    {
        EditWindow.SetActive(!EditWindow.activeSelf);
        if (EditWindow.activeSelf)
        {
            for (int i = 0; i < EditWindowContent.transform.childCount; i++)
            {
                EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().LoadLayers();
            }
        } else
        {
            for (int i = 0; i < EditWindowContent.transform.childCount; i++)
            {
                EditWindowContent.transform.GetChild(i).GetComponent<NoiseMapEditor>().UnloadLayers();
            }
        }
    }

    public void RandomizeSeed()
    {
        var newSeed = Random.Range(-int.MaxValue + 1, int.MaxValue - 1);
        ChunkLoader.Instance.seed = newSeed;
        seed.text = newSeed.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}



