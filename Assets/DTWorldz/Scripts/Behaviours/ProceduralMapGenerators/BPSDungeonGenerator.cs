using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.ProceduralMapGenerators
{
    public class BPSDungeonGenerator : MonoBehaviour
    {
        public int Seed = 0;
        public int Width = 20;
        public int Height = 20;

        public int MinRoomSize = 4;
        public int MaxRoomSize = 10;

        // Tilemaps
        public Tilemap FloorMap;
        public Tilemap WallMap;

        // Tiles
        public TileBase FloorTile;
        public TileBase WallTile;
        public TileBase BackgroundTile;
        public TileBase CorridorTile;
        // Start is called before the first frame update



        void Start()
        {
            long hash = Seed;
            hash = (hash + 0xabcd1234) + (hash << 15);
            hash = (hash + 0x0987efab) ^ (hash >> 11);
            hash ^= Seed;
            hash = (hash + 0x46ac12fd) + (hash << 7);
            hash = (hash + 0xbe9730af) ^ (hash << 11);
            UnityEngine.Random.InitState(Seed);

            // Set background color
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    FloorMap.SetTile(new Vector3Int(i, j, 0), BackgroundTile);
                    WallMap.SetTile(new Vector3Int(i, j, 0), WallTile);
                }
            }

            var roomTree = new BinaryTree();
            roomTree.Add(new Room(new Rect(0, 0, Width, Height)));
            recurseBSP(roomTree.Root);
            roomTree.Root.CreateRooms();
            drawLeafsFloor(roomTree.Root, roomTree);
            //drawLeafWalls(roomTree.Root, roomTree);
            drawCorridors(roomTree.Root, roomTree);
            //drawCorridorsWalls(roomTree.Root, roomTree);
            clearCorridorRoomIntersections(roomTree.Root, roomTree);
        }

        private void drawCorridorsWalls(BinaryTreeNode root, BinaryTree tree)
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


                        //Top Wall
                        if (y == (int)corridor.y + (int)corridor.height - 1)
                        {
                            WallMap.SetTile(new Vector3Int(x, y, 0), WallTile);
                        }
                        else

                        //Bottom Wall
                        if (y == (int)corridor.y)
                        {
                            WallMap.SetTile(new Vector3Int(x, y - 1, 0), WallTile);
                        }
                        else

                        //Left Wall
                        if (x == (int)corridor.x)
                        {
                            WallMap.SetTile(new Vector3Int(x - 1, y, 0), WallTile);
                        }
                        else

                        //Right Wall
                        if (x == (int)corridor.x + (int)corridor.width - 1)
                        {
                            WallMap.SetTile(new Vector3Int(x + 1, y, 0), WallTile);
                        }

                        // //Bottom Left Corner
                        // if (x == (int)node.Room.InnerRect.xMin && y == (int)node.Room.InnerRect.yMin)
                        // {
                        //     WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.x - 1, (int)node.Room.InnerRect.yMin - 1, 0), WallTile);
                        // }

                        // //Bottom Right Corner
                        // if (x == (int)node.Room.InnerRect.xMax - 1 && y == (int)node.Room.InnerRect.yMin)
                        // {
                        //     WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMax, (int)node.Room.InnerRect.yMin - 1, 0), WallTile);
                        // }

                        // //Top Right Corner
                        // if (x == (int)node.Room.InnerRect.xMax - 1 && y == (int)node.Room.InnerRect.yMax - 1)
                        // {
                        //     WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMax, (int)node.Room.InnerRect.yMax, 0), WallTile);
                        // }

                        // //Top Left Corner
                        // if (x == (int)node.Room.InnerRect.xMin && y == (int)node.Room.InnerRect.yMax - 1)
                        // {
                        //     WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMin - 1, (int)node.Room.InnerRect.yMax, 0), WallTile);
                        // }



                        WallMap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
            drawCorridorsWalls(root.LeftNode, tree);
            drawCorridorsWalls(root.RightNode, tree);
        }

        private void drawLeafWalls(BinaryTreeNode node, BinaryTree tree)
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

                        //clear intersecting corridor walls 
                        WallMap.SetTile(new Vector3Int(x, y, 0), null);

                        //Bottom Left Corner
                        if (x == (int)node.Room.InnerRect.xMin && y == (int)node.Room.InnerRect.yMin)
                        {
                            WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.x - 1, (int)node.Room.InnerRect.yMin - 1, 0), WallTile);
                        }

                        //Bottom Right Corner
                        if (x == (int)node.Room.InnerRect.xMax - 1 && y == (int)node.Room.InnerRect.yMin)
                        {
                            WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMax, (int)node.Room.InnerRect.yMin - 1, 0), WallTile);
                        }

                        //Top Right Corner
                        if (x == (int)node.Room.InnerRect.xMax - 1 && y == (int)node.Room.InnerRect.yMax - 1)
                        {
                            WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMax, (int)node.Room.InnerRect.yMax, 0), WallTile);
                        }

                        //Top Left Corner
                        if (x == (int)node.Room.InnerRect.xMin && y == (int)node.Room.InnerRect.yMax - 1)
                        {
                            WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMin - 1, (int)node.Room.InnerRect.yMax, 0), WallTile);
                        }

                        //Top Wall
                        if (y == (int)node.Room.InnerRect.yMax - 1)
                        {
                            WallMap.SetTile(new Vector3Int(x, (int)node.Room.InnerRect.yMax, 0), WallTile);
                        }

                        //Bottom Wall
                        if (y == (int)node.Room.InnerRect.yMin)
                        {
                            WallMap.SetTile(new Vector3Int(x, (int)node.Room.InnerRect.yMin - 1, 0), WallTile);
                        }

                        //Left Wall
                        if (x == (int)node.Room.InnerRect.xMin)
                        {
                            WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMin - 1, y, 0), WallTile);
                        }

                        //Right Wall
                        if (x == (int)node.Room.InnerRect.xMax - 1)
                        {
                            WallMap.SetTile(new Vector3Int((int)node.Room.InnerRect.xMax, y, 0), WallTile);
                        }
                    }
                }



            }
            else
            {
                drawLeafWalls(node.LeftNode, tree);
                drawLeafWalls(node.RightNode, tree);
            }
        }

        private void clearCorridorRoomIntersections(BinaryTreeNode node, BinaryTree tree)
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
                        //FloorMap.SetTile(new Vector3Int(x, y, 0), FloorTile);

                        //clear intersecting corridor walls 
                        WallMap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }

            else
            {
                clearCorridorRoomIntersections(node.LeftNode, tree);
                clearCorridorRoomIntersections(node.RightNode, tree);
            }
        }

        private void drawCorridors(BinaryTreeNode root, BinaryTree tree)
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


                        FloorMap.SetTile(new Vector3Int(x, y, 0), CorridorTile);
                        WallMap.SetTile(new Vector3Int(x, y, 0), null);




                    }
                }
            }
            drawCorridors(root.LeftNode, tree);
            drawCorridors(root.RightNode, tree);
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
            if (node.Room.Rect.width > MaxRoomSize || node.Room.Rect.height > MaxRoomSize)
            {
                // Only recurse if the split was successful
                if (node.Split(MinRoomSize))
                {
                    recurseBSP(node.LeftNode);
                    recurseBSP(node.RightNode);
                }
            }
        }

        public void drawLeafsFloor(BinaryTreeNode node, BinaryTree tree)
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
                        FloorMap.SetTile(new Vector3Int(x, y, 0), FloorTile);
                        WallMap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }



            }
            else
            {
                drawLeafsFloor(node.LeftNode, tree);
                drawLeafsFloor(node.RightNode, tree);
            }
        }
    }
}