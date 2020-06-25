using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ProceduralGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DTWorldz.Behaviours.ProceduralMapGenerators
{
    public class DungeonGenerator : MonoBehaviour
    {
        private int roomCounter;

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

            // Set background color
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    FloorMap.SetTile(new Vector3Int(i, j, 0), BackgroundTile);
                }
            }

            // Represents the overal dimensions of the level
            DungeonRoom rootLevel = new DungeonRoom(new Rect(0, 0, Width, Height));

            // Starts recursion at root
            recurseBSP(rootLevel);
            //StartCoroutine(Wait());

            rootLevel.CreateRooms();

            //createCorridors(rootLevel);
            //StartCoroutine(Wait());

            drawLeafs(rootLevel);
            //StartCoroutine(Wait());

            drawCorridors(rootLevel);
        }

        private void createCorridors(DungeonRoom parent)
        {
            if (parent.leftChild != null && parent.rightChild != null)
            {
                parent.CreateCorridorBetween(parent.leftChild.roomRect, parent.rightChild.roomRect);
            }else{
                createCorridors(parent);
            }
        }

        IEnumerator Wait()
        {
            //Print the time of when the function is first called.
            Debug.Log("Started Coroutine at timestamp : " + Time.time);

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(5);

            //After we have waited 5 seconds print the time again.
            Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        }

        private void drawCorridors(DungeonRoom parent)
        {


            // Fail if the input was null
            if (parent == null)
            {
                return;
            }

            foreach (var corridor in parent.corridors)
            {
                // Draw the floor of the room
                for (int x = (int)corridor.x; x < (int)corridor.x + (int)corridor.width; x++)
                {
                    for (int y = (int)corridor.y; y < (int)corridor.y + (int)corridor.height; y++)
                    {
                        FloorMap.SetTile(new Vector3Int(x, y, 0), CorridorTile);
                    }
                }
            }
            drawCorridors(parent.leftChild);
            drawCorridors(parent.rightChild);

        }
        public void recurseBSP(DungeonRoom parent)
        {
            // Fail if the input was null
            if (parent == null)
            {
                return;
            }

            // Should not recurse on already split rooms
            if (parent.leftChild != null || parent.rightChild != null)
            {
                return;
            }

            // Only attempt a split if the room is large enough
            if (parent.roomRect.width > MaxRoomSize || parent.roomRect.height > MaxRoomSize)
            {
                // Only recurse if the split was successful
                if (parent.split(MinRoomSize))
                {
                    recurseBSP(parent.leftChild);
                    recurseBSP(parent.rightChild);
                }
            }




        }

        public void drawLeafs(DungeonRoom parent)
        {
            //Debug.Log("Call of draw");

            // Fail if the input was null
            if (parent == null)
            {
                return;
            }

            // Draw the room if it is a leaf, otherwise recurse
            if (parent.leftChild == null && parent.rightChild == null)
            {

                //Debug.Log("Drew room " + " in rectangle " + parent.roomRect);
                //drawnRooms.Add(parent.roomRect);

                // Draw the floor of the room
                for (int x = (int)parent.roomRect.xMin; x < (int)parent.roomRect.xMax; x++)
                {
                    for (int y = (int)parent.roomRect.yMin; y < (int)parent.roomRect.yMax; y++)
                    {
                        FloorMap.SetTile(new Vector3Int(x, y, 0), FloorTile);
                    }
                }

                // // Set x and y for convenience
                // int x = (int)parent.roomRect.x;
                // int y = (int)parent.roomRect.y;

                // // Draw the upper borders
                // tilemap_walls.SetTile(new Vector3Int(x - 1, y + (int)parent.roomRect.height, 0), upperBorderCornerL);
                // tilemap_walls.SetTile(new Vector3Int(x + (int)parent.roomRect.width, y + (int)parent.roomRect.height, 0), upperBorderCornerR);

                // for (int i = x; i < x + (int)parent.roomRect.width; i++)
                // {
                //     tilemap_walls.SetTile(new Vector3Int(i, y + (int)parent.roomRect.height, 0), upperBorder);
                // }

                // // Draw the upper walls
                // TileBase[] upperWalls = { upperWall0, upperWall1, upperWall2, upperWall3, upperWall4, upperWallGate };
                // tilemap_walls.SetTile(new Vector3Int(x - 1, y + (int)parent.roomRect.height - 1, 0), upperWallCornerL);
                // tilemap_walls.SetTile(new Vector3Int(x + (int)parent.roomRect.width, y + (int)parent.roomRect.height - 1, 0), upperWallCornerR);

                // for (int i = x; i < x + (int)parent.roomRect.width; i++)
                // {
                //     // Selects a random wall tile, with a preference for plain
                //     TileBase selWall = upperWalls[Random.Range(0, upperWalls.Length)];
                //     if (Random.value < 0.7f)
                //     {
                //         selWall = upperWall0;
                //     }
                //     tilemap_walls.SetTile(new Vector3Int(i, y + (int)parent.roomRect.height - 1, 0), selWall);
                // }

                // // Draw the lower borders
                // tilemap_walls.SetTile(new Vector3Int(x - 1, y, 0), lowerBorderCornerL);
                // tilemap_walls.SetTile(new Vector3Int(x + (int)parent.roomRect.width, y, 0), lowerBorderCornerR);

                // for (int i = x; i < x + (int)parent.roomRect.width; i++)
                // {
                //     tilemap_walls.SetTile(new Vector3Int(i, y, 0), lowerBorder);
                // }

                // // Draw the left borders
                // for (int i = y + 1; i < y + (int)parent.roomRect.height - 1; i++)
                // {
                //     tilemap_walls.SetTile(new Vector3Int(x - 1, i, 0), leftBorder);
                // }

                // // Draw the right borders
                // for (int i = y + 1; i < y + (int)parent.roomRect.height - 1; i++)
                // {
                //     tilemap_walls.SetTile(new Vector3Int(x + (int)parent.roomRect.width, i, 0), rightBorder);
                // }

                // // Add decorations
                // TileBase[] decorations = { crate, chest, knightRed, knightBlue, knightGreen, knightPurple, rocks };
                // for (int i = (int)parent.roomRect.x; i < (int)parent.roomRect.x + (int)parent.roomRect.width; i++)
                // {
                //     for (int j = (int)parent.roomRect.y + 1; j < (int)parent.roomRect.y + (int)parent.roomRect.height - 1; j++)
                //     {
                //         if (Random.value < 0.05f)
                //         {
                //             TileBase selDec = decorations[Random.Range(0, decorations.Length)];
                //             tilemap_walls.SetTile(new Vector3Int(i, j, 0), selDec);
                //         }
                //     }
                // }
            }
            else
            {
                drawLeafs(parent.leftChild);
                drawLeafs(parent.rightChild);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}