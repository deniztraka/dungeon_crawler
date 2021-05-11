using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Utils;
using DTWorldz.ProceduralGeneration;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.ProceduralMapGenerators.NoiseMap
{

    public class CellSet
    {
        public Vector3Int Pos;
        public Vector3 WorldPos;
        public bool BushesSet;
        public bool TreesSet;
        public bool SpawnersSet;
        public bool IsProcessed;
        public CellSet(Vector3Int pos, Vector3 worldPos)
        {
            this.Pos = pos;
            this.WorldPos = worldPos;
            this.BushesSet = false;
            this.TreesSet = false;
            this.SpawnersSet = false;
            this.IsProcessed = false;
        }

    }
    public class NoiseMapGenerator : MonoBehaviour
    {
        public enum DrawMode { NoiseMap, ColorMap, TileMap }
        public DrawMode DrawingMode;
        public int Seed = 1;
        public int Width = 100;
        public int Height = 100;
        public float Scale = 0.3f;
        public int Octaves = 4;
        [Range(0, 1)]
        public float Persistance = 0.5f;
        public float Lacunarity = 2;
        public bool IsIsland = true;
        public Texture2D IslandHeightMapTexture;
        public float LandIntensisty = 4;
        //public TerrainType[] Regions;
        public TileMapTerrain[] Terrains;
        public Vector2 OffSet = new Vector2(0, 0);

        public Transform TreesParentObject;
        public Transform BushesParentObject;
        public Transform SpawnersParentObject;

        public bool autoUpdate;
        public bool placeTrees;
        public bool placeBushes;
        public bool placeSpawners;

        public bool GenerateOnStart;

        System.Random prng;

        public Dictionary<string, CellSet> TerrrainTiles;

        void Awake()
        {
            prng = new System.Random(Seed);
        }

        void Start()
        {
            if (GenerateOnStart)
            {
                GenerateMap();
            }
            
            TerrrainTiles = new Dictionary<string, CellSet>();

            FillTerrainTiles();

            var player = GameObject.FindGameObjectWithTag("Player");
            StartCoroutine(ProcessCellsAroundPlayer(player, 10));
        }

        private List<CellSet> GetCellSetsAround(GameObject playerObject, TileMapTerrain terrain, int distance)
        {
            var terrrainTilesAround = new List<CellSet>();
            var cellPos = terrain.Tilemap.WorldToCell(playerObject.transform.position);
            for (int x = cellPos.x - distance; x < cellPos.x + distance; x++)
            {
                for (int y = cellPos.y - distance; y < cellPos.y + distance; y++)
                {
                    var tilePosAround = new Vector3Int(x, y, 0);
                    if (terrain.Tilemap.HasTile(tilePosAround))
                    {
                        Vector3 worldPos = terrain.Tilemap.CellToWorld(tilePosAround);
                        var key = String.Format("{0}_{1}-{2}", terrain.Template.Name, tilePosAround.x, tilePosAround.y);
                        CellSet cellSet = null;
                        var found = TerrrainTiles.TryGetValue(key, out cellSet);
                        if (found)
                        {
                            terrrainTilesAround.Add(cellSet);
                        }
                    }
                }
            }

            return terrrainTilesAround;
        }

        private IEnumerator ProcessCellsAroundPlayer(GameObject playerObject, int distance)
        {
            LevelBehaviour levelBehaviour = null;
            while (true)
            {
                Dictionary<TileMapTerrain, List<CellSet>> TerrrainTiles = new Dictionary<TileMapTerrain, List<CellSet>>();
                foreach (var terrain in Terrains)
                {
                    if (levelBehaviour == null)
                    {
                        levelBehaviour = terrain.Tilemap.transform.GetComponentInParent<LevelBehaviour>();
                    }
                    var aroundCellSets = GetCellSetsAround(playerObject, terrain, distance);
                    foreach (var cellSet in aroundCellSets)
                    {
                        if (!cellSet.IsProcessed && terrain.Tilemap.HasTile(cellSet.Pos))
                        {
                            if (!cellSet.BushesSet)
                            {
                                var chance = prng.NextDouble();
                                if (chance < terrain.Template.BushFrequency)
                                {
                                    var objPos = new Vector3(cellSet.WorldPos.x + 0.5f, cellSet.WorldPos.y + 0.5f, 0);
                                    var bush = Instantiate(terrain.Template.BushTypes[UnityEngine.Random.Range(0, terrain.Template.BushTypes.Count)], objPos, Quaternion.identity, BushesParentObject);
                                    bush.transform.localScale = new Vector3(UnityEngine.Random.Range(0.9f, 1.1f), UnityEngine.Random.Range(0.75f, 1.25f), 1f);
                                    var disabler = bush.GetComponent<DisableIfFarAway>();
                                    disabler.Init();
                                    cellSet.BushesSet = true;
                                }
                            }

                            if (!cellSet.BushesSet && !cellSet.TreesSet)
                            {
                                var chance = prng.NextDouble();
                                if (chance < terrain.Template.TreeFrequency)
                                {
                                    var objPosition = new Vector3(cellSet.WorldPos.x + 0.5f, cellSet.WorldPos.y + 0.5f, 0);
                                    var tree = Instantiate(terrain.Template.TreeTypes[prng.Next(0, terrain.Template.TreeTypes.Count)], objPosition, Quaternion.identity, TreesParentObject);
                                    UnityEngine.Random.InitState(prng.Next());
                                    tree.transform.localScale = new Vector3(UnityEngine.Random.Range(1f, 1.5f), UnityEngine.Random.Range(1f, 1.5f), 1f);
                                    var disabler = tree.GetComponent<DisableIfFarAway>();
                                    disabler.Init();
                                    cellSet.TreesSet = true;
                                }
                            }

                            if (!cellSet.SpawnersSet)
                            {
                                var chance = prng.NextDouble();
                                if (chance < terrain.Template.SpawnerFrequency)
                                {
                                    var objPos = new Vector3(cellSet.WorldPos.x + 0.5f, cellSet.WorldPos.y + 0.5f, 0);
                                    var spawnerObject = Instantiate(terrain.Template.Spawners[prng.Next(0, terrain.Template.Spawners.Count)], objPos, Quaternion.identity, SpawnersParentObject);
                                    var objectSpawnerBehaviour = spawnerObject.GetComponent<ObjectSpawnerBehaviour>();
                                    objectSpawnerBehaviour.CurrentLevel = levelBehaviour;
                                    cellSet.SpawnersSet = true;
                                }
                            }
                            cellSet.IsProcessed = true;
                        }
                    }
                }

                yield return new WaitForSeconds(1f);
            }
        }


        public void FillTerrainTiles()
        {
            foreach (var terrain in Terrains)
            {
                // terrain.ilemap.GetTilesBlock(area);
                foreach (var pos in terrain.Tilemap.cellBounds.allPositionsWithin)
                {
                    Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                    //Vector3 place = terrain.Tilemap.CellToWorld(localPlace);
                    if (terrain.Tilemap.HasTile(localPlace))
                    {
                        Vector3 worldPos = terrain.Tilemap.CellToWorld(localPlace);

                        TerrrainTiles.Add(String.Format("{0}_{1}-{2}", terrain.Template.Name, localPlace.x, localPlace.y), new CellSet(localPlace, worldPos));
                    }
                }
            }
        }


        public System.Random GenerateMap()
        {
            ClearTileMap();

            var prng = new System.Random(Seed);

            float[,] noiseMap = Noise.GenerateNoiseMap(prng, Width, Height, Scale, Octaves, Persistance, Lacunarity, OffSet, IsIsland, IslandHeightMapTexture, LandIntensisty);

            var mapDisplay = MapDisplay.FindObjectOfType<MapDisplay>();
            if (DrawingMode == DrawMode.NoiseMap)
            {
                mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
            }
            else if (DrawingMode == DrawMode.ColorMap)
            {
                mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(GetColorMap(noiseMap), Width, Height));
            }
            else if (DrawingMode == DrawMode.TileMap)
            {
                mapDisplay.DrawTileMap(Terrains, noiseMap, Width, Height);
            }

            if (placeTrees)
            {
                PlaceTrees(prng);
            }

            if (placeBushes)
            {
                PlaceBushes(prng);
            }

            if (placeSpawners)
            {
                PlaceSpawners(prng);
            }
            return prng;
        }

        public void PlaceBushes(System.Random prng)
        {
            ClearBushes();

            if (BushesParentObject == null)
            {
                return;
            }

            foreach (var terrain in Terrains)
            {
                if (terrain.Template.BushFrequency == 0f || terrain.Template.BushTypes.Count == 0)
                {
                    continue;
                }

                var gridLayout = terrain.Tilemap.transform.GetComponentInParent<GridLayout>();

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var chance = prng.NextDouble();
                        if (chance < terrain.Template.BushFrequency)
                        {
                            var regionTile = terrain.Tilemap.GetTile(new Vector3Int(x, y, 0));
                            if (regionTile != null)
                            {
                                var cellPosition = gridLayout.CellToWorld(new Vector3Int(x, y, 0));
                                cellPosition = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
                                var bush = Instantiate(terrain.Template.BushTypes[prng.Next(0, terrain.Template.BushTypes.Count)], cellPosition, Quaternion.identity, BushesParentObject);
                                bush.transform.localScale = new Vector3(UnityEngine.Random.Range(0.9f, 1.1f), UnityEngine.Random.Range(0.75f, 1.25f), 1f);
                            }
                        }
                    }
                }
            }
        }
        public void ClearBushes()
        {
            if (BushesParentObject == null)
            {
                return;
            }

            for (int i = BushesParentObject.childCount - 1; i >= 0; i--)
            {
                Transform child = BushesParentObject.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        public void PlaceTrees(System.Random prng)
        {
            ClearTrees();

            if (TreesParentObject == null)
            {
                return;
            }

            foreach (var terrain in Terrains)
            {
                if (terrain.Template.TreeFrequency == 0f || terrain.Template.TreeTypes.Count == 0)
                {
                    continue;
                }
                var gridLayout = terrain.Tilemap.transform.GetComponentInParent<GridLayout>();

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var chance = prng.NextDouble();
                        if (chance < terrain.Template.TreeFrequency)
                        {
                            var terrainTile = terrain.Tilemap.GetTile(new Vector3Int(x, y, 0));
                            if (terrainTile != null)
                            {
                                var cellPosition = gridLayout.CellToWorld(new Vector3Int(x, y, 0));
                                cellPosition = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
                                var tree = Instantiate(terrain.Template.TreeTypes[prng.Next(0, terrain.Template.TreeTypes.Count)], cellPosition, Quaternion.identity, TreesParentObject);
                                UnityEngine.Random.InitState(prng.Next());
                                tree.transform.localScale = new Vector3(UnityEngine.Random.Range(1f, 1.5f), UnityEngine.Random.Range(1f, 1.5f), 1f);
                            }
                        }
                    }
                }
            }
        }

        public void ClearTrees()
        {
            if (TreesParentObject == null)
            {
                return;
            }

            for (int i = TreesParentObject.childCount - 1; i >= 0; i--)
            {
                Transform child = TreesParentObject.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        public void PlaceSpawners(System.Random prng)
        {
            ClearSpawners();

            if (SpawnersParentObject == null)
            {
                return;
            }

            foreach (var terrain in Terrains)
            {
                if (terrain.Template.SpawnerFrequency == 0f || terrain.Template.Spawners.Count == 0)
                {
                    continue;
                }
                var gridLayout = terrain.Tilemap.transform.GetComponentInParent<GridLayout>();

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        var chance = prng.NextDouble();
                        if (chance < terrain.Template.SpawnerFrequency)
                        {
                            var terrainTile = terrain.Tilemap.GetTile(new Vector3Int(x, y, 0));
                            if (terrainTile != null)
                            {
                                var cellPosition = gridLayout.CellToWorld(new Vector3Int(x, y, 0));
                                cellPosition = new Vector3(cellPosition.x + 0.5f, cellPosition.y + 0.5f, 0);
                                var spawnerObject = Instantiate(terrain.Template.Spawners[prng.Next(0, terrain.Template.Spawners.Count)], cellPosition, Quaternion.identity, SpawnersParentObject);
                                var objectSpawnerBehaviour = spawnerObject.GetComponent<ObjectSpawnerBehaviour>();
                                objectSpawnerBehaviour.CurrentLevel = gridLayout.GetComponentInParent<LevelBehaviour>();
                            }
                        }
                    }
                }
            }
        }

        public void ClearSpawners()
        {
            if (SpawnersParentObject == null)
            {
                return;
            }

            for (int i = SpawnersParentObject.childCount - 1; i >= 0; i--)
            {
                Transform child = SpawnersParentObject.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        public void ClearTileMap()
        {
            var mapDisplay = MapDisplay.FindObjectOfType<MapDisplay>();
            mapDisplay.ClearTileMap(Terrains);
        }

        private Color[] GetColorMap(float[,] noiseMap)
        {
            var colorMap = new Color[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int r = 0; r < Terrains.Length; r++)
                    {
                        if (currentHeight <= Terrains[r].Template.Height)
                        {
                            colorMap[y * Width + x] = Terrains[r].Template.Color;
                            break;
                        }
                    }
                }
            }

            return colorMap;
        }

        private Tile[] GetTileMap(float[,] noiseMap)
        {
            var tileMap = new Tile[Width * Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    float currentHeight = noiseMap[x, y];
                    for (int r = 0; r < Terrains.Length; r++)
                    {
                        if (currentHeight <= Terrains[r].Template.Height)
                        {
                            tileMap[y * Width + x] = Terrains[r].Template.Tile;
                            break;
                        }
                    }
                }
            }

            return tileMap;
        }

        void OnValidate()
        {
            if (Width < 1)
            {
                Width = 1;
            }
            if (Height < 1)
            {
                Height = 1;
            }
            if (Lacunarity < 1)
            {
                Lacunarity = 1;
            }
            if (Octaves < 0)
            {
                Octaves = 0;
            }
        }
    }
}