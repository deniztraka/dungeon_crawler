﻿using System;
using System.Collections;
using System.Collections.Generic;
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

        private GameObject player;

        private List<Vector3> testPaths;


        // Tilemaps
        public Tilemap FloorMap;
        public Tilemap FloorDecorationsMap;
        public Tilemap WallMap;
        public Tilemap WallDecorationsMap;

        public Transform EnvironmentParent;

        private Random random;

        public void ClearMap()
        {
            FloorMap.ClearAllTiles();
            FloorDecorationsMap.ClearAllTiles();
            WallMap.ClearAllTiles();
            WallDecorationsMap.ClearAllTiles();

            int childs = EnvironmentParent.transform.childCount;
            for (int i = childs - 1; i > 0; i--)
            {
                DestroyImmediate(EnvironmentParent.transform.GetChild(i).gameObject);
            }
        }

        public void BuildMap()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            random = new System.Random(DungeonTemplate.Seed != -1 ? DungeonTemplate.Seed : DateTime.Now.Millisecond);

            // Set Default Tiles
            for (int i = 0; i < DungeonTemplate.Width; i++)
            {
                for (int j = 0; j < DungeonTemplate.Height; j++)
                {
                    FloorMap.SetTile(new Vector3Int(i, j, 0), DungeonTemplate.BackgroundTile);
                    WallMap.SetTile(new Vector3Int(i, j, 0), DungeonTemplate.WallTile);
                }
            }

            var roomTree = new BinaryTree(random);
            roomTree.Add(new Room(new Rect(0, 0, DungeonTemplate.Width, DungeonTemplate.Height)));
            recurseBSP(roomTree.Root);
            roomTree.Root.CreateRooms();
            BuildLeafs(roomTree.Root, roomTree);
            BuildCorridors(roomTree.Root, roomTree);
            BuildDecorations(roomTree.Root, roomTree);
            BuildLights(roomTree.Root, roomTree);

            //Debug.Log(roomTree.GetTreeDepth());
            //put treasure into smallest room
            var smallestNode = roomTree.FindMin(roomTree.Root);
            BuildTreasure(smallestNode);

            //move player into biggest room
            var largestNode = roomTree.FindMax(roomTree.Root);
            BuildStartRoom(largestNode);
        }

        private void BuildLights(BinaryTreeNode node, BinaryTree tree)
        {
            // Fail if the node was null
            if (node == null)
            {
                return;
            }

            // Iterate if the room is a leaf, otherwise recurse
            if (node.LeftNode == null && node.RightNode == null)
            {
                var lightPositions = GetRoomLightPositions(node.Room);
                foreach (var pos in lightPositions.Top)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.TopLight, pos, DungeonTemplate.RoomTemplate.TopLight.transform.rotation, EnvironmentParent);
                }
                foreach (var pos in lightPositions.Bottom)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.BottomLight, pos, DungeonTemplate.RoomTemplate.BottomLight.transform.rotation, EnvironmentParent);
                }
                foreach (var pos in lightPositions.Left)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.LeftLight, pos, DungeonTemplate.RoomTemplate.LeftLight.transform.rotation, EnvironmentParent);
                }
                foreach (var pos in lightPositions.Right)
                {
                    var light = Instantiate(DungeonTemplate.RoomTemplate.RightLight, pos, DungeonTemplate.RoomTemplate.RightLight.transform.rotation, EnvironmentParent);
                }
            }
            else
            {
                BuildLights(node.LeftNode, tree);
                BuildLights(node.RightNode, tree);
            }
        }

        private LightPositions GetRoomLightPositions(Room room)
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
                    if (y == room.InnerRect.yMax - 1 && WallMap.GetTile(new Vector3Int(x, y + 1, 0)) != null && x == room.InnerRect.xMax - (Math.Ceiling(width / 2)))
                    {
                        var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        positions.Top.Add(new Vector3(objectPosition.x, objectPosition.y - 0.01f, objectPosition.z));
                    }

                    //bottom lights
                    if (y == room.InnerRect.yMin && WallMap.GetTile(new Vector3Int(x, y - 1, 0)) != null && x == room.InnerRect.xMax - (Math.Ceiling(width / 2)))
                    {
                        var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y - 1, 0));
                        positions.Bottom.Add(new Vector3(objectPosition.x, objectPosition.y - 0.01f, objectPosition.z));
                    }

                    //left lights
                    if (x == room.InnerRect.xMin && WallMap.GetTile(new Vector3Int(x - 1, y, 0)) != null && y == room.InnerRect.yMin + (Math.Floor(height / 2) - 1))
                    {
                        var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                        positions.Left.Add(new Vector3(objectPosition.x - 0.6f, objectPosition.y, objectPosition.z));
                    }


                    // && WallMap.GetTile(new Vector3Int(x + 1, y, 0)) != null && y == room.InnerRect.yMin + (Math.Floor(height / 2)-1)
                    //right lights
                    if (x == room.InnerRect.xMax - 1 && WallMap.GetTile(new Vector3Int(x + 1, y, 0)) != null && y == room.InnerRect.yMin + (Math.Floor(height / 2) - 1))
                    {
                        var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
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

        private void BuildStartRoom(BinaryTreeNode node)
        {
            if (player != null)
            {
                //move player in the center of the room            
                var position = FloorMap.GetCellCenterWorld(new Vector3Int((int)node.Room.InnerRect.center.x, (int)node.Room.InnerRect.center.y, 0));
                player.transform.position = position;
            }
        }

        private void BuildTreasure(BinaryTreeNode node)
        {
            // Fail if the node was null
            if (node == null)
            {
                return;
            }

            if (DungeonTemplate.RoomTemplate.Treasures.Length > 0)
            {
                var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int((int)node.Room.InnerRect.center.x, (int)node.Room.InnerRect.center.y, 0));


                var obj = Instantiate(DungeonTemplate.RoomTemplate.Treasures[random.Next(0, DungeonTemplate.RoomTemplate.Treasures.Length)], objectPosition, Quaternion.identity, EnvironmentParent);
                node.Room.Objects.Add(obj);

            }
        }

        private void BuildDecorations(BinaryTreeNode node, BinaryTree tree)
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
                            var wallTile = WallMap.GetTile(new Vector3Int(x, y + 1, 0));
                            if (wallTile != null)
                            {
                                if (DungeonTemplate.RoomTemplate.DecorationChance > random.NextDouble())
                                {
                                    WallDecorationsMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.RoomTemplate.UpperWallDecorations[random.Next(0, DungeonTemplate.RoomTemplate.UpperWallDecorations.Length)]);
                                }
                            }
                        }

                        //Room Floor Decorations
                        if (DungeonTemplate.RoomTemplate.FloorDecorations.Length > 0 && y != (int)node.Room.InnerRect.yMax - 1 && y != (int)node.Room.InnerRect.yMin && x != (int)node.Room.InnerRect.xMax - 1 && x != (int)node.Room.InnerRect.xMin)
                        {

                            if (DungeonTemplate.RoomTemplate.DecorationChance > random.NextDouble())
                            {
                                FloorDecorationsMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.RoomTemplate.FloorDecorations[random.Next(0, DungeonTemplate.RoomTemplate.FloorDecorations.Length)]);
                            }

                        }

                        //Room Object Decorations
                        if (DungeonTemplate.RoomTemplate.Containers.Length > 0)
                        {
                            //if there is no any decoration && chance factor ofcourse && make sure containers locations will be near wall
                            if (node.Room.Objects.Count < node.Room.MaxNumberOfObjects && //there shouldn't be too much objects
                                FloorDecorationsMap.GetTile(new Vector3Int(x, y, 0)) == null && //there should be no floor decoration on that tile
                                DungeonTemplate.RoomTemplate.ContainerChance > random.NextDouble() && //chance factor
                                (y == (int)node.Room.InnerRect.yMax - 1 || y == (int)node.Room.InnerRect.yMin + 1 || x == (int)node.Room.InnerRect.xMax - 1 || x == (int)node.Room.InnerRect.xMin)
                            )
                            {
                                var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                                //to centralize the object within cell
                                var newPos = new Vector3(objectPosition.x, objectPosition.y, objectPosition.z);
                                var obj = Instantiate(DungeonTemplate.RoomTemplate.Containers[random.Next(0, DungeonTemplate.RoomTemplate.Containers.Length)], newPos, Quaternion.identity, EnvironmentParent);
                                node.Room.Objects.Add(obj);
                            }
                        }

                    }
                }
            }
            else
            {
                BuildDecorations(node.LeftNode, tree);
                BuildDecorations(node.RightNode, tree);
            }
        }

        private void BuildCorridors(BinaryTreeNode root, BinaryTree tree)
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


                        FloorMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.FloorTile);
                        WallMap.SetTile(new Vector3Int(x, y, 0), null);




                    }
                }
            }
            BuildCorridors(root.LeftNode, tree);
            BuildCorridors(root.RightNode, tree);
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

        public void BuildLeafs(BinaryTreeNode node, BinaryTree tree)
        {
            //Debug.Log("Call of draw");

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
                        FloorMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.FloorTile);
                        WallMap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }



            }
            else
            {
                BuildLeafs(node.LeftNode, tree);
                BuildLeafs(node.RightNode, tree);
            }
        }

        void Update()
        {
            //to test astar alghoritm
            if (player != null && Input.GetMouseButtonDown(0))
            {
                testPaths = AStar.FindPath(WallMap, player.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
        void OnDrawGizmosSelected()
        {
            if (testPaths != null && testPaths.Count > 0)
            {
                Gizmos.color = Color.blue;
                foreach (var point in testPaths)
                {
                    Gizmos.DrawWireSphere(point, 0.5f);
                }

                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(testPaths[0], 0.5f);
            }
        }
    }
}