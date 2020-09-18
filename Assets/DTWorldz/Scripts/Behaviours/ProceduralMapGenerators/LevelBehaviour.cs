using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Behaviours.Items.Utils;
using DTWorldz.ProceduralGeneration;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.ProceduralMapGenerators
{
    public class LevelBehaviour : MonoBehaviour
    {
        // Tilemaps
        public Tilemap FloorMap;
        public Tilemap FloorDecorationsMap;
        public Tilemap WallMap;
        public Tilemap WallDecorationsMap;
        public Transform EnvironmentParent;
        public LayerMask CleaningLayer;
        public TileBase TestTile;

        private int levelNumber;
        private BinaryTree tree;
        private System.Random random;
        private BinaryTreeNode exitNode;
        private BinaryTreeNode levelDownNode;

        private GameObject ladderDown;
        private GameObject exit;

        internal void SetRandom(System.Random random)
        {
            this.random = random;
        }

        internal void SetLevelNumber(int levelNumber)
        {
            this.levelNumber = levelNumber;
        }

        internal void SetTree(BinaryTree tree)
        {
            this.tree = tree;
        }

        public void MovePlayer(GameObject player)
        {
            if (player != null && FloorMap != null && exitNode != null)
            {
                //move player in the center of the room            
                //var position = FloorMap.GetCellCenterWorld(new Vector3Int((int)exitNode.Room.InnerRect.center.x, (int)exitNode.Room.InnerRect.center.y, 0));
                player.transform.position = exit.transform.position;
                var movementBehaviour = player.GetComponent<PlayerMovementBehaviour>();
                movementBehaviour.SetMovementGrid(WallMap);
            }
        }

        internal void BuildTreaseRoom(DungeonTemplate dungeonTemplate)
        {
            var smallestNode = tree.FindMin(tree.Root);
            //Fail if the node was null
            if (smallestNode == null)
            {
                return;
            }

            if (dungeonTemplate.RoomTemplate.Treasures.Length > 0)
            {
                var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)smallestNode.Room.InnerRect.center.x, (int)smallestNode.Room.InnerRect.center.y, 0));
                var obj = Instantiate(dungeonTemplate.TreasurePrefab, objectPosition, Quaternion.identity, EnvironmentParent);
                smallestNode.Room.Objects.Add(obj);

                var spawner = smallestNode.Room.GetSpawner();
                if (spawner != null)
                {
                    spawner.SpawnBoss = true;
                }
            }
        }

        internal void GenerateTraps(DungeonTemplate dungeonTemplate)
        {
            GenerateTrap(tree.Root, dungeonTemplate);
        }

        private void GenerateTrap(BinaryTreeNode node, DungeonTemplate dungeonTemplate)
        {
            // Fail if the node was null
            if (node == null)
            {
                return;
            }

            // Iterate if the room is a leaf, otherwise recurse
            if (node.LeftNode == null && node.RightNode == null)
            {
                var room = node.Room;
                for (int x = (int)room.InnerRect.xMin; x < (int)room.InnerRect.xMax; x++)
                {
                    for (int y = (int)room.InnerRect.yMin; y < (int)room.InnerRect.yMax; y++)
                    {
                        //floor tiles
                        if (dungeonTemplate.RoomTemplate.TrapTemplate.FloorTrapPrefabs != null && dungeonTemplate.RoomTemplate.TrapTemplate.FloorTrapPrefabs.Count > 0 && dungeonTemplate.RoomTemplate.TrapChance > random.NextDouble())
                        {
                            if (y != (int)room.InnerRect.yMax - 1 && y != (int)room.InnerRect.yMin && x != (int)room.InnerRect.xMax - 1 && x != (int)room.InnerRect.xMin)
                            {
                                var chosenTrapPrefab = dungeonTemplate.RoomTemplate.TrapTemplate.FloorTrapPrefabs[random.Next(0, dungeonTemplate.RoomTemplate.TrapTemplate.FloorTrapPrefabs.Count)];
                                var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                                var instantiatedTrapObject = Instantiate(chosenTrapPrefab, objectPosition, Quaternion.identity, EnvironmentParent);
                            }
                        }

                        //top wall
                        if (dungeonTemplate.RoomTemplate.TrapTemplate.TopWallTrapPrefabs != null && dungeonTemplate.RoomTemplate.TrapTemplate.TopWallTrapPrefabs.Count > 0 && dungeonTemplate.RoomTemplate.TrapChance > random.NextDouble())
                        {
                            if (y == room.InnerRect.yMax - 1 && WallMap.GetTile(new Vector3Int(x, y + 1, 0)) != null)
                            {
                                var chosenTrapPrefab = dungeonTemplate.RoomTemplate.TrapTemplate.TopWallTrapPrefabs[random.Next(0, dungeonTemplate.RoomTemplate.TrapTemplate.TopWallTrapPrefabs.Count)];
                                var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                                var instantiatedTrapObject = Instantiate(chosenTrapPrefab, new Vector3(objectPosition.x, objectPosition.y, 0), Quaternion.identity, EnvironmentParent);
                            }
                        }

                        //right wall
                        if (dungeonTemplate.RoomTemplate.TrapTemplate.RightWallTrapPrefabs != null && dungeonTemplate.RoomTemplate.TrapTemplate.RightWallTrapPrefabs.Count > 0 && dungeonTemplate.RoomTemplate.TrapChance > random.NextDouble())
                        {
                            if (x == room.InnerRect.xMax - 1 && WallMap.GetTile(new Vector3Int(x + 1, y, 0)) != null)
                            {
                                //var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                                //positions.Right.Add(new Vector3(objectPosition.x + 0.6f, objectPosition.y, objectPosition.z));
                            }
                        }

                        //left wall
                        if (dungeonTemplate.RoomTemplate.TrapTemplate.LeftWallTrapPrefabs != null && dungeonTemplate.RoomTemplate.TrapTemplate.LeftWallTrapPrefabs.Count > 0 && dungeonTemplate.RoomTemplate.TrapChance > random.NextDouble())
                        {
                            if (x == room.InnerRect.xMin && WallMap.GetTile(new Vector3Int(x - 1, y, 0)) != null)
                            {
                                //var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                                //positions.Left.Add(new Vector3(objectPosition.x - 0.6f, objectPosition.y, objectPosition.z));
                            }
                        }
                    }
                }
            }
            else
            {
                GenerateTrap(node.LeftNode, dungeonTemplate);
                GenerateTrap(node.RightNode, dungeonTemplate);
            }
        }

        internal void GenerateSpawners(GameObject correspondingSpawnerPrefab)
        {
            GenerateSpawner(tree.Root, correspondingSpawnerPrefab);
        }

        private void GenerateSpawner(BinaryTreeNode node, GameObject correspondingSpawnerPrefab)
        {
            // Fail if the node was null
            if (node == null)
            {
                return;
            }

            // Iterate if the room is a leaf, otherwise recurse
            if (node.LeftNode == null && node.RightNode == null)
            {
                var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)node.Room.InnerRect.center.x, (int)node.Room.InnerRect.center.y, 0));
                var spawner = Instantiate(correspondingSpawnerPrefab, objectPosition, Quaternion.identity, EnvironmentParent);
                var spawnerBehaviour = spawner.GetComponent<ObjectSpawnerBehaviour>();
                spawnerBehaviour.RangeY = node.Room.InnerRect.height - 1;
                spawnerBehaviour.RangeX = node.Room.InnerRect.width - 1;
                spawnerBehaviour.CurrentLevel = this;
                var aliveCount = (int)Math.Floor((decimal)(node.Room.GetSurcafeArea() / 10));
                spawnerBehaviour.MaxAliveCount = (int)Math.Ceiling((decimal)(aliveCount / 3));
                node.Room.SetSpawner(spawnerBehaviour);
            }
            else
            {
                GenerateSpawner(node.LeftNode, correspondingSpawnerPrefab);
                GenerateSpawner(node.RightNode, correspondingSpawnerPrefab);
            }
        }

        internal void AddDungeonExit(DungeonTemplate dungeonTemplate)
        {
            if (exitNode == null)
            {
                exitNode = tree.FindMax(tree.Root);
            }

            AddLevelExit(dungeonTemplate.ExitDoorPrefab, true);
        }

        internal void AddLadderDown(DungeonTemplate dungeonTemplate)
        {
            if (levelDownNode == null)
            {
                levelDownNode = tree.FindMin(tree.Root);
            }

            var ladderPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)levelDownNode.Room.InnerRect.center.x, (int)levelDownNode.Room.InnerRect.center.y, 0));

            //clean objects surraounding
            var overlappedItems = Physics2D.OverlapBoxAll(ladderPosition, new Vector2(2, 2), 0f, CleaningLayer);
            foreach (var item in overlappedItems)
            {
                if (item.tag != "Player")
                {
                    //Debug.Log("down" + item.name);
                    DestroyImmediate(item.gameObject);
                }
            }

            ladderDown = Instantiate(dungeonTemplate.LadderDownPrefab, ladderPosition, Quaternion.identity, EnvironmentParent);
        }

        internal void AddLadderUp(DungeonTemplate dungeonTemplate)
        {
            if (exitNode == null)
            {
                exitNode = tree.FindMax(tree.Root);
            }

            AddLevelExit(dungeonTemplate.LadderUpPrefab, false);
        }

        internal void AddChest(DungeonTemplate dungeonTemplate)
        {
            BuildTreaseRoom(dungeonTemplate);
        }

        internal void AddLevelExit(GameObject exitPrefab, Boolean isDungeonExit)
        {
            var ladderPosition = Vector3.zero;

            //if its dungeon exit, make it center of the room
            if (isDungeonExit)
            {
                //Debug.Log(WallMap.GetTile(new Vector3Int(11, 27, 0)));
                //try to find a good place top wall
                for (int x = (int)exitNode.Room.InnerRect.xMin; x < (int)exitNode.Room.InnerRect.xMax; x++)
                {
                    for (int y = (int)exitNode.Room.InnerRect.yMin; y < (int)exitNode.Room.InnerRect.yMax; y++)
                    {
                        //Debug.Log(y + "-" + (exitNode.Room.InnerRect.yMax - 1) + "  :  " + (WallMap.GetTile(new Vector3Int(x, y, 0)) != null).ToString());
                        //top wall
                        if (ladderPosition == Vector3.zero &&
                         y == exitNode.Room.InnerRect.yMax - 1 &&
                          WallMap.GetTile(new Vector3Int(x, y + 1, 0)) != null &&
                          WallMap.GetTile(new Vector3Int(x + 1, y + 1, 0)) != null &&
                          WallMap.GetTile(new Vector3Int(x - 1, y + 1, 0)) != null &&
                          WallMap.GetTile(new Vector3Int(x + 2, y + 1, 0)) != null &&
                          WallMap.GetTile(new Vector3Int(x - 2, y + 1, 0)) != null)
                        {
                            // Debug.Log(x + "-" + y);
                            // Debug.Log(WallMap.GetTile(new Vector3Int(x - 2, y + 1, 0)));                            
                            ladderPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        }
                    }
                }

                // if there is no place to put ladder on top wall, put it on center
                if (ladderPosition == Vector3.zero)
                {
                    ladderPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)exitNode.Room.InnerRect.center.x, (int)exitNode.Room.InnerRect.center.y, 0));
                }
            }
            else
            {
                //try to find a good place on left wall
                for (int x = (int)exitNode.Room.InnerRect.xMin; x < (int)exitNode.Room.InnerRect.xMax; x++)
                {
                    for (int y = (int)exitNode.Room.InnerRect.yMin; y < (int)exitNode.Room.InnerRect.yMax; y++)
                    {
                        //left wall
                        if (x == exitNode.Room.InnerRect.xMin && WallMap.GetTile(new Vector3Int(x - 1, y, 0)) != null)
                        {
                            ladderPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        }
                    }
                }

                // if there is no place to put ladder on left wall, put it on center
                if (ladderPosition == Vector3.zero)
                {
                    ladderPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)exitNode.Room.InnerRect.center.x, (int)exitNode.Room.InnerRect.center.y, 0));
                }
            }

            //clean objects in front of the ladder
            var overlappedItems = Physics2D.OverlapBoxAll(ladderPosition, isDungeonExit ? new Vector2(3, 3) : new Vector2(2, 1), 0f, CleaningLayer);
            foreach (var item in overlappedItems)
            {
                if (item.tag != "Player")
                {
                    //Debug.Log("exit" + item.name);
                    DestroyImmediate(item.gameObject);
                }
            }
            exit = Instantiate(exitPrefab, ladderPosition, Quaternion.identity, EnvironmentParent);

        }

        public GameObject GetExitObject()
        {
            return exit;
        }

        public GameObject GetLadderDownObject()
        {
            return ladderDown;
        }
    }
}