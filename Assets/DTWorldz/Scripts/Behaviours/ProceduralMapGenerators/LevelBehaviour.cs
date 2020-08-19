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

            AddLevelExit(dungeonTemplate.ExitDoorPrefab);
        }

        internal void AddLadderDown(DungeonTemplate dungeonTemplate)
        {
            if (levelDownNode == null)
            {
                levelDownNode = tree.FindMin(tree.Root);
            }

            var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)levelDownNode.Room.InnerRect.center.x, (int)levelDownNode.Room.InnerRect.center.y, 0));
            //var obj = Instantiate(dungeonTemplate.RoomTemplate.Treasures[random.Next(0, dungeonTemplate.RoomTemplate.Treasures.Length)], objectPosition, Quaternion.identity, EnvironmentParent);
            ladderDown = Instantiate(dungeonTemplate.LadderDownPrefab, objectPosition, Quaternion.identity, EnvironmentParent);
        }

        internal void AddLadderUp(DungeonTemplate dungeonTemplate)
        {
            if (exitNode == null)
            {
                exitNode = tree.FindMax(tree.Root);
            }

            AddLevelExit(dungeonTemplate.LadderUpPrefab);                    
        }

        internal void AddLevelExit(GameObject exitPrefab)
        {            
            var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)exitNode.Room.InnerRect.center.x, (int)exitNode.Room.InnerRect.center.y, 0));
            //var obj = Instantiate(dungeonTemplate.RoomTemplate.Treasures[random.Next(0, dungeonTemplate.RoomTemplate.Treasures.Length)], objectPosition, Quaternion.identity, EnvironmentParent);
            exit = Instantiate(exitPrefab, objectPosition, Quaternion.identity, EnvironmentParent);
        }

        public GameObject GetExitObject(){
            return exit;
        }

        public GameObject GetLadderDownObject(){
            return ladderDown;
        }
    }
}