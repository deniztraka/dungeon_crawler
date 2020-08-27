using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Items.Utils;
using DTWorldz.Models;
using DTWorldz.ProceduralGeneration;
using DTWorldz.ScriptableObjects;
using Toolbox;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace DTWorldz.Behaviours.ProceduralMapGenerators
{
    public class BPSDungeonGenerator : MonoBehaviour
    {
        public DungeonTemplate DungeonTemplate;
        public int Seed = -1;

        public GameObject Dungeon;
        public GameObject LevelPrefab;

        public GameObject TestPrefab;

        private GameObject player;
        private Random random;
        private List<LevelBehaviour> levels;

        public void BuildMap()
        {
            if (Dungeon.transform.childCount > 0)
            {
                ClearMap();
            }

            if (levels == null)
            {
                levels = new List<LevelBehaviour>();
            }

            player = GameObject.FindGameObjectWithTag("Player");
            var seed = DungeonTemplate.Seed != -1 ? DungeonTemplate.Seed : (Seed != -1 ? Seed : DateTime.Now.Millisecond);

            random = new System.Random(seed);

            var levelCount = random.Next(1, DungeonTemplate.MaxLevelCount);
            for (int i = 0; i < levelCount; i++)
            {
                BuildLevel(i);
            }

            for (int i = 0; i < levels.Count; i++)
            {
                BuildTraps(i);
            }

            for (int i = 0; i < levels.Count; i++)
            {
                BuildLevelTransitions(i);
            }

            for (int i = 0; i < levels.Count; i++)
            {
                BuildTeleporters(i);
            }

            for (int i = 0; i < levels.Count; i++)
            {
                BuildSpawners(i);
            }

            //move player into biggest room in first level
            levels[0].MovePlayer(player);

            levels[levels.Count - 1].AddChest(DungeonTemplate);

            TestFeatures();

            Debug.Log("Dungeon is created with seed:" + seed.ToString());
        }

        private void BuildTraps(int levelIndex)
        {
            levels[levelIndex].GenerateTraps(DungeonTemplate);
        }

        private void BuildSpawners(int levelIndex)
        {
            var correspondingSpawner = DungeonTemplate.SpawnerObjects[levelIndex];
            if (correspondingSpawner != null)
            {
                levels[levelIndex].GenerateSpawners(correspondingSpawner);
            }
        }

        public void ClearMap()
        {
            int childsCount = Dungeon.transform.childCount;
            for (int i = childsCount - 1; i >= 0; i--)
            {
                DestroyImmediate(Dungeon.transform.GetChild(i).gameObject);
            }
            levels = new List<LevelBehaviour>();
        }

        private void BuildLevel(int levelNumber)
        {
            //calculate width acording to level number
            var width = Math.Floor(DungeonTemplate.Width * Math.Pow(0.75, levelNumber));
            width = width < DungeonTemplate.MaxRoomSize ? DungeonTemplate.MaxRoomSize : width;

            var levelPositionX = DungeonTemplate.Width * levelNumber + levelNumber * 10;

            var levelGameObject = Instantiate(LevelPrefab, new Vector3(levelPositionX, 0, 0), Quaternion.identity, Dungeon.transform);
            var levelBehaviour = levelGameObject.GetComponent<LevelBehaviour>();
            levels.Add(levelBehaviour);

            // Set Default Tiles
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    levelBehaviour.FloorMap.SetTile(new Vector3Int(i, j, 0), DungeonTemplate.BackgroundTile);
                    levelBehaviour.WallMap.SetTile(new Vector3Int(i, j, 0), DungeonTemplate.WallTile);
                }
            }

            var roomTree = new BinaryTree(random);
            roomTree.Add(new Room(new Rect(0, 0, (float)width, (float)width)));
            recurseBSP(roomTree.Root);
            roomTree.Root.CreateRooms();
            levelBehaviour.SetRandom(random);
            levelBehaviour.SetLevelNumber(levelNumber);
            levelBehaviour.SetTree(roomTree);

            BuildLeafs(roomTree.Root, levelBehaviour);
            BuildCorridors(roomTree.Root, levelBehaviour);
            BuildDecorations(roomTree.Root, levelBehaviour);
            BuildLights(roomTree.Root, levelBehaviour);
        }

        private void TestFeatures()
        {
            // var obj = Instantiate(TestPrefab, new Vector3(23.5f, 15.5f, 0), Quaternion.identity);
            // var movementBehaviour = obj.GetComponent<MovementBehaviour>();
            // movementBehaviour.SetMovementGrid(levels[0].WallMap);
        }

        private void BuildTeleporters(int levelNumber)
        {
            //if there is another level down below
            if (levelNumber + 1 < levels.Count)
            {
                //get ladder down for this level
                var thisLadderDownObject = levels[levelNumber].GetLadderDownObject();
                var ladderDownTeleporter = thisLadderDownObject.GetComponentInChildren<TeleporterBehaviour>();

                //get exit for one level below
                var belowLevelExit = levels[levelNumber + 1].GetExitObject();
                var belowLevelTeleporter = belowLevelExit.GetComponentInChildren<TeleporterBehaviour>();

                ladderDownTeleporter.TeleportTo = belowLevelTeleporter.transform.position + belowLevelTeleporter.TeleportPosition;
                ladderDownTeleporter.SetTeleportsToLevel(levels[levelNumber + 1]);

                belowLevelTeleporter.TeleportTo = ladderDownTeleporter.transform.position + ladderDownTeleporter.TeleportPosition;
                belowLevelTeleporter.SetTeleportsToLevel(levels[levelNumber]);
            }
        }

        private void BuildLevelTransitions(int levelNumber)
        {
            //Debug.Log("LEVEL:" + levelNumber);
            var levelBehaviour = levels[levelNumber];

            // ilk kat ise
            if (levelNumber == 0)
            {
                //Debug.Log("dungeon Exit for level:" + levelNumber);
                //bu levela dungeondan çıkış kapısı koy 
                levelBehaviour.AddDungeonExit(DungeonTemplate);

            }

            //bir level yukarısı var ise
            if (levelNumber > 0)
            {
                //Debug.Log("ladder Up for level:" + levelNumber);
                //bu levela yukarı çıkış kapısı koy     
                levelBehaviour.AddLadderUp(DungeonTemplate);
            }

            //bir level aşağısı var ise
            if (levelNumber + 1 < levels.Count)
            {
                //Debug.Log("ladder Down for level:" + levelNumber);
                //bu levela aşağı iniş kapısı koy  
                levelBehaviour.AddLadderDown(DungeonTemplate);
            }
        }

        private void BuildLights(BinaryTreeNode node, LevelBehaviour levelBehaviour)
        {
            // Fail if the node was null
            if (node == null)
            {
                return;
            }

            // Iterate if the room is a leaf, otherwise recurse
            if (node.LeftNode == null && node.RightNode == null)
            {
                var lightPositions = GetRoomLightPositions(node.Room, levelBehaviour);
                foreach (var pos in lightPositions.Top)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.TopLight, pos, DungeonTemplate.RoomTemplate.TopLight.transform.rotation, levelBehaviour.EnvironmentParent);
                }
                foreach (var pos in lightPositions.Bottom)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.BottomLight, pos, DungeonTemplate.RoomTemplate.BottomLight.transform.rotation, levelBehaviour.EnvironmentParent);
                }
                foreach (var pos in lightPositions.Left)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.LeftLight, pos, DungeonTemplate.RoomTemplate.LeftLight.transform.rotation, levelBehaviour.EnvironmentParent);
                }
                foreach (var pos in lightPositions.Right)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.RightLight, pos, DungeonTemplate.RoomTemplate.RightLight.transform.rotation, levelBehaviour.EnvironmentParent);
                }
            }
            else
            {
                BuildLights(node.LeftNode, levelBehaviour);
                BuildLights(node.RightNode, levelBehaviour);
            }
        }

        private LightPositions GetRoomLightPositions(Room room, LevelBehaviour levelBehaviour)
        {
            var positions = new LightPositions();
            var height = room.InnerRect.height;
            var width = room.InnerRect.width;

            //Debug.Log(width * height);

            for (int x = (int)room.InnerRect.xMin; x < (int)room.InnerRect.xMax; x++)
            {
                for (int y = (int)room.InnerRect.yMin; y < (int)room.InnerRect.yMax; y++)
                {
                    //top lights
                    if (y == room.InnerRect.yMax - 1 && levelBehaviour.WallMap.GetTile(new Vector3Int(x, y + 1, 0)) != null && x == room.InnerRect.xMax - (Math.Ceiling(width / 2)))
                    {
                        var objectPosition = levelBehaviour.FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        positions.Top.Add(new Vector3(objectPosition.x, objectPosition.y - 0.01f, objectPosition.z));
                    }

                    //bottom lights
                    if (y == room.InnerRect.yMin && levelBehaviour.WallMap.GetTile(new Vector3Int(x, y - 1, 0)) != null && x == room.InnerRect.xMax - (Math.Ceiling(width / 2)))
                    {
                        var objectPosition = levelBehaviour.FloorMap.GetCellCenterWorld(new Vector3Int(x, y - 1, 0));
                        positions.Bottom.Add(new Vector3(objectPosition.x, objectPosition.y - 0.01f, objectPosition.z));
                    }

                    //left lights
                    if (x == room.InnerRect.xMin && levelBehaviour.WallMap.GetTile(new Vector3Int(x - 1, y, 0)) != null && y == room.InnerRect.yMin + (Math.Floor(height / 2) - 1))
                    {
                        var objectPosition = levelBehaviour.FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        positions.Left.Add(new Vector3(objectPosition.x - 0.6f, objectPosition.y, objectPosition.z));
                    }


                    // && WallMap.GetTile(new Vector3Int(x + 1, y, 0)) != null && y == room.InnerRect.yMin + (Math.Floor(height / 2)-1)
                    //right lights
                    if (x == room.InnerRect.xMax - 1 && levelBehaviour.WallMap.GetTile(new Vector3Int(x + 1, y, 0)) != null && y == room.InnerRect.yMin + (Math.Floor(height / 2) - 1))
                    {
                        var objectPosition = levelBehaviour.FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        positions.Right.Add(new Vector3(objectPosition.x + 0.6f, objectPosition.y, objectPosition.z));
                    }

                }
            }

            return positions;
        }

        void Start()
        {
            //BuildMap();
        }

        private void BuildDecorations(BinaryTreeNode node, LevelBehaviour levelBehaviour)
        {
            // Fail if the node was null
            if (node == null)
            {
                return;
            }

            // Iterate if the room is a leaf, otherwise recurse
            if (node.LeftNode == null && node.RightNode == null)
            {
                // iterate room tiles
                for (int x = (int)node.Room.InnerRect.xMin; x < (int)node.Room.InnerRect.xMax; x++)
                {
                    for (int y = (int)node.Room.InnerRect.yMin; y < (int)node.Room.InnerRect.yMax; y++)
                    {

                        //Room Top Wall Decorations
                        if (DungeonTemplate.RoomTemplate.UpperWallDecorations.Length > 0 && y == (int)node.Room.InnerRect.yMax - 1)
                        {
                            var wallTile = levelBehaviour.WallMap.GetTile(new Vector3Int(x, y + 1, 0));
                            if (wallTile != null)
                            {
                                if (DungeonTemplate.RoomTemplate.DecorationChance > random.NextDouble())
                                {
                                    levelBehaviour.WallDecorationsMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.RoomTemplate.UpperWallDecorations[random.Next(0, DungeonTemplate.RoomTemplate.UpperWallDecorations.Length)]);
                                }
                            }
                        }

                        //Room Floor Decorations
                        if (DungeonTemplate.RoomTemplate.FloorDecorations.Length > 0 && y != (int)node.Room.InnerRect.yMax - 1 && y != (int)node.Room.InnerRect.yMin && x != (int)node.Room.InnerRect.xMax - 1 && x != (int)node.Room.InnerRect.xMin)
                        {

                            if (DungeonTemplate.RoomTemplate.DecorationChance > random.NextDouble())
                            {
                                levelBehaviour.FloorDecorationsMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.RoomTemplate.FloorDecorations[random.Next(0, DungeonTemplate.RoomTemplate.FloorDecorations.Length)]);
                            }

                        }

                        //Room Object Decorations
                        if (DungeonTemplate.RoomTemplate.Containers.Length > 0)
                        {
                            //if there is no any decoration && chance factor ofcourse && make sure containers locations will be near wall
                            if (node.Room.Objects.Count < node.Room.MaxNumberOfObjects && //there shouldn't be too much objects
                                levelBehaviour.FloorDecorationsMap.GetTile(new Vector3Int(x, y, 0)) == null && //there should be no floor decoration on that tile
                                DungeonTemplate.RoomTemplate.ContainerChance > random.NextDouble() && //chance factor
                                (y == (int)node.Room.InnerRect.yMax - 1 || y == (int)node.Room.InnerRect.yMin + 1 || x == (int)node.Room.InnerRect.xMax - 1 || x == (int)node.Room.InnerRect.xMin)
                            )
                            {
                                var objectPosition = levelBehaviour.FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                                //to centralize the object within cell
                                var newPos = new Vector3(objectPosition.x, objectPosition.y, objectPosition.z);
                                var obj = Instantiate(DungeonTemplate.RoomTemplate.Containers[random.Next(0, DungeonTemplate.RoomTemplate.Containers.Length)], newPos, Quaternion.identity, levelBehaviour.EnvironmentParent);
                                node.Room.Objects.Add(obj);
                            }
                        }

                    }
                }
            }
            else
            {
                BuildDecorations(node.LeftNode, levelBehaviour);
                BuildDecorations(node.RightNode, levelBehaviour);
            }
        }

        private void BuildCorridors(BinaryTreeNode root, LevelBehaviour levelBehaviour)
        {
            // Fail if the input was null
            if (root == null)
            {
                return;
            }

            foreach (var corridor in root.GetCorridors())
            {
                // Draw the floor of the room
                for (int x = (int)corridor.x; x < (int)corridor.x + (int)corridor.width; x++)
                {
                    for (int y = (int)corridor.y; y < (int)corridor.y + (int)corridor.height; y++)
                    {


                        levelBehaviour.FloorMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.FloorTile);
                        levelBehaviour.WallMap.SetTile(new Vector3Int(x, y, 0), null);




                    }
                }
            }
            BuildCorridors(root.LeftNode, levelBehaviour);
            BuildCorridors(root.RightNode, levelBehaviour);
        }

        public void recurseBSP(BinaryTreeNode node)
        {
            // Fail if the input was null
            if (node == null)
            {
                return;
            }


            // Should not recurse on already split rooms
            if (node.LeftNode != null || node.RightNode != null)
            {
                return;
            }

            // Only attempt a split if the room is large enough
            if (node.Room.Rect.width > DungeonTemplate.MaxRoomSize || node.Room.Rect.height > DungeonTemplate.MaxRoomSize)
            {
                // Only recurse if the split was successful
                if (node.Split(DungeonTemplate))
                {
                    recurseBSP(node.LeftNode);
                    recurseBSP(node.RightNode);
                }
            }
        }

        public void BuildLeafs(BinaryTreeNode node, LevelBehaviour levelBehaviour)
        {
            // Fail if the input was null
            if (node == null)
            {
                return;
            }

            // Draw the room if it is a leaf, otherwise recurse
            if (node.LeftNode == null && node.RightNode == null)
            {
                // Draw the floor of the room
                for (int x = (int)node.Room.InnerRect.xMin; x < (int)node.Room.InnerRect.xMax; x++)
                {
                    for (int y = (int)node.Room.InnerRect.yMin; y < (int)node.Room.InnerRect.yMax; y++)
                    {
                        levelBehaviour.FloorMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.FloorTile);
                        levelBehaviour.WallMap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }



            }
            else
            {
                BuildLeafs(node.LeftNode, levelBehaviour);
                BuildLeafs(node.RightNode, levelBehaviour);
            }
        }

        void Update()
        {
            //to test astar alghoritm
            if (Input.GetMouseButtonDown(0))
            {
                //testPaths = AStar.FindPath(WallMap, player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

            }
        }
        void OnDrawGizmosSelected()
        {
            // if (testPaths != null && testPaths.Count > 0)
            // {
            //     Gizmos.color = Color.blue;
            //     foreach (var point in testPaths)
            //     {
            //         Gizmos.DrawWireSphere(point, 0.5f);
            //     }

            //     Gizmos.color = Color.red;

            //     Gizmos.DrawWireSphere(testPaths[0], 0.5f);
            // }
        }
    }
}