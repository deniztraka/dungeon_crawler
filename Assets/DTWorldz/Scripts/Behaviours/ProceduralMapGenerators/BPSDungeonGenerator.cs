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
            UnityEngine.Random.InitState((int)hash);

            // Set background color
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    FloorMap.SetTile(new Vector3Int(i, j, 0), BackgroundTile);
                }
            }

            var roomTree = new BinaryTree();
            roomTree.Add(new Room(new Rect(0, 0, Width, Height)));
            recurseBSP(roomTree.Root);
            roomTree.Root.CreateRooms();
            drawLeafs(roomTree.Root, roomTree);
            drawCorridors(roomTree.Root, roomTree);
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
                        FloorMap.SetTile(new Vector3Int(x, y, 0), FloorTile);
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

        public void drawLeafs(BinaryTreeNode node, BinaryTree tree)
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
                    }
                }


            }
            else
            {
                drawLeafs(node.LeftNode, tree);
                drawLeafs(node.RightNode, tree);
            }
        }
    }
}