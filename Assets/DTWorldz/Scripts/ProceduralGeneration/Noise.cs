using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ProceduralGeneration
{

    public static class Noise
    {
        public static float[,] GenerateNoiseMap(System.Random prng, int width, int height, float scale, int octaves, float persistance, float lacunarity, Vector2 offSet, bool isIsland, float landIntensisty, float islandGradientMiddleIntensity)
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

            // Assuming noiseMap is already populated with noise values
            if(isIsland)
            {
                noiseMap = ApplyRadialGradient(noiseMap, landIntensisty, islandGradientMiddleIntensity, width, height);
            }

            return noiseMap;
        }

        private static float[,] ApplyRadialGradient(float[,] noiseMap,float landIntensisty, float islandGradientMiddleIntensity, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // Calculate the distance from the center of the map
                    float distanceX = x - width / 2f;
                    float distanceY = y - height / 2f;
                    float distance = Mathf.Sqrt(distanceX * distanceX + distanceY * distanceY);

                    // Normalize the distance so it ranges from 0 (center) to 1 (edge)
                    float maxDistance = Mathf.Sqrt((width / 2f) * (width / 2f) + (height / 2f) * (height / 2f));
                    float gradient = Mathf.InverseLerp(0, maxDistance, distance);

                    // Invert the gradient to make the center high and the edges low
                    gradient = 1 - gradient;

                    // Skew the gradient to make the center more intense
                    // Raise the gradient to a power less than 1 to make the middle more intense
                    gradient = Mathf.Pow(gradient, islandGradientMiddleIntensity);

                    // Optionally, adjust the gradient curve
                    gradient = Mathf.Pow(gradient, 1/landIntensisty);

                    // Combine the noise map and the radial gradient
                    noiseMap[x, y] = noiseMap[x, y] * gradient;
                }
            }

            return noiseMap;
        }
    }
}