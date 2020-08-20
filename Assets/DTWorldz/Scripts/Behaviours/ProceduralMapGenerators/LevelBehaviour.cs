using System;
using System.Collections;
using System.Collections.Generic;
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

        private int levelNumber;
        private BinaryTree tree;
        private System.Random random;
        private BinaryTreeNode exitNode;
        private BinaryTreeNode levelDownNode;

        private GameObject ladderDown;
        private GameObject exit;

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
                var position = FloorMap.GetCellCenterWorld(new Vector3Int((int)exitNode.Room.InnerRect.center.x, (int)exitNode.Room.InnerRect.center.y, 0));
                player.transform.position = position;
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
                //var obj = Instantiate(dungeonTemplate.RoomTemplate.Treasures[random.Next(0, dungeonTemplate.RoomTemplate.Treasures.Length)], objectPosition, Quaternion.identity, EnvironmentParent);
                var obj = Instantiate(dungeonTemplate.TreasurePrefab, objectPosition, Quaternion.identity, EnvironmentParent);
                smallestNode.Room.Objects.Add(obj);

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

        internal void AddLevelExit(GameObject exitPrefab, Boolean isDungeonExit)
        {
            var ladderPosition = Vector3.zero;

            //if its dungeon exit, make it center of the room
            if (isDungeonExit)
            {
                ladderPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)exitNode.Room.InnerRect.center.x, (int)exitNode.Room.InnerRect.center.y, 0));
            }
            else
            {
                //try to find a good place on left wall
                for (int x = (int)exitNode.Room.InnerRect.xMin; x < (int)exitNode.Room.InnerRect.xMax; x++)
                {
                    for (int y = (int)exitNode.Room.InnerRect.yMin; y < (int)exitNode.Room.InnerRect.yMax; y++)
                    {
                        //left lights
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
            var overlappedItems = Physics2D.OverlapBoxAll(ladderPosition, new Vector2(2, 1), 0f, CleaningLayer);
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