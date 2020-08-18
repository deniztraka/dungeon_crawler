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

        internal void SetLevelNumber(int levelNumber)
        {
            this.levelNumber = levelNumber;
        }

        internal void SetTree(BinaryTree tree)
        {
            this.tree = tree;
        }

        public void BuildStartRoom(GameObject player)
        {
            var largestNode = tree.FindMax(tree.Root);
            if (player != null && FloorMap != null)
            {
                //move player in the center of the room            
                var position = FloorMap.GetCellCenterWorld(new Vector3Int((int)largestNode.Room.InnerRect.center.x, (int)largestNode.Room.InnerRect.center.y, 0));
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
    }
}