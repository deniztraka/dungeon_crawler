using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ProceduralGeneration
{

    public static class Noise
    {
        public static float[,] GenerateNoiseMap(System.Random prng, int width, int height, float scale, int octaves, float persistance, float lacunarity, Vector2 offSet, bool isIsland, Texture2D islandHeightMap, float islandHeightMapIntensity)
        {

            var noiseMap = new float[width, height];

            var octaveOffsets = new Vector2[octaves];
            for (int o = 0; o < octaves; o++)
            {
                float offSetX = prng.Next(-100000, 100000) + offSet.x;
                float offSetY = prng.Next(-100000, 100000) + offSet.y;
                octaveOffsets[o] = new Vector2(offSetX, offSetY);
            }


            if (scale <= 0)
            {
                scale = 0.0001f;
            }



            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            for (int x = 0; x < width; x++)
            {

                for (int y = 0; y < height; y++)
                {

                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int o = 0; o < octaves; o++)
                    {

                        float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[o].x;
                        float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[o].y;

                        var perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // to make samples between -1,1
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;


                        //noiseMap[x, y] = perlinValue;
                    }

                    // for normalization
                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;

                }
            }

            // normalize our noisemap again to between 0,1
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            // make it island
            if (isIsland && islandHeightMap != null)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        var grayScaleSample = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight - 0.1f, islandHeightMap.GetPixel(x, y).grayscale);
                        //Debug.Log(hede);
                        grayScaleSample = Mathf.Pow(grayScaleSample, islandHeightMapIntensity);

                        //noiseMap[x, y] = (1 + noiseMap[x, y] - grayScaleSample) / 2;
                        noiseMap[x, y] = noiseMap[x, y] - grayScaleSample;

                    }
                }
            }

            return noiseMap;
        }
    }
}