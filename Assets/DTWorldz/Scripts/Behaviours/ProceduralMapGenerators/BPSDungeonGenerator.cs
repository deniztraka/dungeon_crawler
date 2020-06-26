using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.ProceduralMapGenerators
{
    public class BPSDungeonGenerator : MonoBehaviour
    {
        public DungeonTemplate DungeonTemplate;

        // Tilemaps
        public Tilemap FloorMap;
        public Tilemap FloorDecorationsMap;
        public Tilemap WallMap;
        public Tilemap WallDecorationsMap;

        void Start()
        {
            long hash = DungeonTemplate.Seed;
            hash = (hash + 0xabcd1234) + (hash << 15);
            hash = (hash + 0x0987efab) ^ (hash >> 11);
            hash ^= DungeonTemplate.Seed;
            hash = (hash + 0x46ac12fd) + (hash << 7);
            hash = (hash + 0xbe9730af) ^ (hash << 11);
            UnityEngine.Random.InitState(DungeonTemplate.Seed);

            // Set Default Tiles
            for (int i = 0; i < DungeonTemplate.Width; i++)
            {
                for (int j = 0; j < DungeonTemplate.Height; j++)
                {
                    FloorMap.SetTile(new Vector3Int(i, j, 0), DungeonTemplate.BackgroundTile);
                    WallMap.SetTile(new Vector3Int(i, j, 0), DungeonTemplate.WallTile);
                }
            }

            var roomTree = new BinaryTree();
            roomTree.Add(new Room(new Rect(0, 0, DungeonTemplate.Width, DungeonTemplate.Height)));
            recurseBSP(roomTree.Root);
            roomTree.Root.CreateRooms();
            BuildLeafs(roomTree.Root, roomTree);
            BuildCorridors(roomTree.Root, roomTree);
            BuildDecorations(roomTree.Root, roomTree);
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
                                if (DungeonTemplate.RoomTemplate.DecorationChance > UnityEngine.Random.value)
                                {
                                    WallDecorationsMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.RoomTemplate.UpperWallDecorations[UnityEngine.Random.Range(0, DungeonTemplate.RoomTemplate.UpperWallDecorations.Length)]);
                                }
                            }
                        }

                        //Room Floor Decorations
                        if (DungeonTemplate.RoomTemplate.FloorDecorations.Length > 0 && y != (int)node.Room.InnerRect.yMax - 1 && y != (int)node.Room.InnerRect.yMin && x != (int)node.Room.InnerRect.xMax - 1 && x != (int)node.Room.InnerRect.xMin)
                        {

                            if (DungeonTemplate.RoomTemplate.DecorationChance > UnityEngine.Random.value)
                            {
                                FloorDecorationsMap.SetTile(new Vector3Int(x, y, 0), DungeonTemplate.RoomTemplate.FloorDecorations[UnityEngine.Random.Range(0, DungeonTemplate.RoomTemplate.FloorDecorations.Length)]);
                            }

                        }

                        //Room Object Decorations
                        if (DungeonTemplate.RoomTemplate.FloorDecorations.Length > 0)
                        {
                            //if there is no any decoration and chance factor ofcourse
                            if (node.Room.Objects.Count < node.Room.MaxNumberOfObjects && FloorDecorationsMap.GetTile(new Vector3Int(x, y, 0)) == null && DungeonTemplate.RoomTemplate.ObjectsChance > UnityEngine.Random.value)
                            {
                                var objectPosition = FloorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                                //to centralize the object within cell
                                var newPos = new Vector3(objectPosition.x, objectPosition.y + .2f, objectPosition.z);
                                var obj = Instantiate(DungeonTemplate.RoomTemplate.Objects[UnityEngine.Random.Range(0, DungeonTemplate.RoomTemplate.Objects.Length)], newPos, Quaternion.identity);
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
                if (node.Split(DungeonTemplate.MinRoomSize))
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
    }
}