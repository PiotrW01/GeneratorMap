using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{
    public static float[,] GenerateNoiseMap(int chunkSize, float scale, List<Layer> layers, Vector2 chunkStartPos, int seed)
    {
        float[,] noiseMap = new float[chunkSize, chunkSize];
        if (layers.Count == 0) return noiseMap;

        Random.InitState(seed);

        int offsetX = Random.Range(0, 99999);
        int offsetY = Random.Range(0, 99999);

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                float samplePosX = (chunkStartPos.x + x) * scale + offsetX;
                float samplePosY = (chunkStartPos.y + y) * scale + offsetY;

                float normalization = 0.0f;
                foreach (Layer layer in layers)
                {
                    Random.InitState(seed);
                    int layerSeed = Random.Range(0, 9999);
                    noiseMap[x, y] += layer.amplitude * Mathf.PerlinNoise(samplePosX * layer.frequency
                                    + layerSeed, samplePosY * layer.frequency + layerSeed);
                    normalization += layer.amplitude;
                }
                noiseMap[x, y] /= normalization;
            }
        }
        return noiseMap;
    }
}
