using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.ScriptableObjects;
using UnityEngine;
using Random = System.Random;

namespace DTWorldz.ProceduralGeneration
{
    public class BinaryTreeNode
    {
        public BinaryTreeNode LeftNode { get; set; }
        public BinaryTreeNode RightNode { get; set; }

        public Room Room { get; set; }

        private List<Rect> corridors = new List<Rect>();
        private Random random;


        public BinaryTreeNode(Room room, Random random)
        {
            Room = room;
            this.random = random;

        }

        private void SetChildNodes(BinaryTreeNode node1, BinaryTreeNode node2)
        {
            if (node1.Room.GetSurcafeArea() < node2.Room.GetSurcafeArea())
            {
                LeftNode = node1;
                RightNode = node2;
            }
            else
            {
                LeftNode = node2;
                RightNode = node1;
            }
        }

        internal bool Split(DungeonTemplate dungeonTemplate)
        {
            // Should not split rooms that have already been split
            if (LeftNode != null || RightNode != null)
            {
                return false;
            }

            // Randomly pick if the split will be horizontal or vertical
            bool horSplit = (random.NextDouble() < 0.5f);

            // To prevent splitting a room in one way too much
            if (Room.Rect.width / Room.Rect.height >= 1.25)
            {
                horSplit = false;
            }
            else if (Room.Rect.height / Room.Rect.width >= 1.25)
            {
                horSplit = true;
            }

            // Determine if an appropriate size is impossible
            int maxDimension = (horSplit ? (int)Room.Rect.height : (int)Room.Rect.width) - dungeonTemplate.MinRoomSize;
            if (maxDimension < dungeonTemplate.MinRoomSize)
            {
                return false;
            }

            // Calculate where the split will occur
            int split = random.Next(dungeonTemplate.MinRoomSize, maxDimension);

            // Split the room into two children
            if (horSplit)
            {
                var node1 = new BinaryTreeNode(new Room(new Rect(Room.Rect.x, Room.Rect.y, Room.Rect.width, split)), random);
                var node2 = new BinaryTreeNode(new Room(new Rect(Room.Rect.x, Room.Rect.y + split, Room.Rect.width, Room.Rect.height - split)), random);

                SetChildNodes(node1, node2);
            }
            else
            {
                var node1 = new BinaryTreeNode(new Room(new Rect(Room.Rect.x, Room.Rect.y, split, Room.Rect.height)), random);
                var node2 = new BinaryTreeNode(new Room(new Rect(Room.Rect.x + split, Room.Rect.y, Room.Rect.width - split, Room.Rect.height)), random);
                SetChildNodes(node1, node2);
            }

            // Split was successful
            return true;
        }

        internal IEnumerable<Rect> GetCorridors()
        {
            return corridors;
        }

        internal void CreateRooms()
        {
            // Current room is not a leaf
            if (LeftNode != null || RightNode != null)
            {
                // Recurse through valid children
                if (LeftNode != null)
                {
                    LeftNode.CreateRooms();
                }
                if (RightNode != null)
                {
                    RightNode.CreateRooms();
                }


                CreateCorridorBetweenChilds();

            }
            else
            {
                int roomWidth = random.Next((int)Room.Rect.width / 2, (int)Room.Rect.width - 2);

                int roomHeight = random.Next((int)Room.Rect.height / 2, (int)Room.Rect.height - 2);

                int roomX = random.Next(1, (int)Room.Rect.width - roomWidth - 1);

                int roomY = random.Next(1, (int)Room.Rect.height - roomHeight - 1);


                Room.InnerRect = new Rect(Room.Rect.x + roomX, Room.Rect.y + roomY, roomWidth, roomHeight);
            }
        }        

        private void CreateCorridorBetweenChilds()
        {
            //if there are both left and right children in this Leaf, create a corridor between them
            if (LeftNode != null && RightNode != null)
            {
                Rect lroom = LeftNode.Room.Rect;
                Rect rroom = RightNode.Room.Rect;

                // Rect lroom = LeftNode.Room.InnerRect;
                // Rect rroom = RightNode.Room.InnerRect

                //Debug.Log("Creating corridor(s) between " + left + "(" + lroom + ") and " + right + " (" + rroom + ")");

                // attach the corridor to a random point in each room

                // Vector2 lpoint = new Vector2((int)Random.Range(lroom.xMin+1, lroom.xMax), (int)Random.Range(lroom.yMin+1, lroom.yMax));
                // Vector2 rpoint = new Vector2((int)Random.Range(rroom.xMin+1, rroom.xMax), (int)Random.Range(rroom.yMin+1, rroom.yMax));
                var lpoint = lroom.center;
                var rpoint = rroom.center;

                // always be sure that left point is on the left to simplify the code
                if (lpoint.x > rpoint.x)
                {
                    Vector2 temp = lpoint;
                    lpoint = rpoint;
                    rpoint = temp;
                }

                int w = (int)(rpoint.x - lpoint.x);
                int h = (int)(lpoint.y - rpoint.y);

                //Debug.Log("lpoint: " + lpoint + ", rpoint: " + rpoint + ", w: " + w + ", h: " + h);

                // if the points are not aligned horizontally
                if (w != 0)
                {
                    // choose at random to go horizontal then vertical or the opposite
                    if (random.Next(0, 1) > 2)
                    {
                        // add a corridor to the right
                        corridors.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 1));

                        // if left point is below right point go up
                        // otherwise go down
                        if (h < 0)
                        {
                            corridors.Add(new Rect(rpoint.x, lpoint.y, 2, Mathf.Abs(h)));
                        }
                        else
                        {
                            corridors.Add(new Rect(rpoint.x, lpoint.y, 2, -Mathf.Abs(h)));
                        }
                    }
                    else
                    {
                        // go up or down
                        if (h < 0)
                        {
                            corridors.Add(new Rect(lpoint.x, lpoint.y, 2, Mathf.Abs(h)));
                        }
                        else
                        {
                            corridors.Add(new Rect(lpoint.x, rpoint.y, 2, Mathf.Abs(h)));
                        }

                        // then go right
                        corridors.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 2));
                    }
                }
                else
                {
                    // if the points are aligned horizontally
                    // go up or down depending on the positions
                    if (h < 0)
                    {
                        corridors.Add(new Rect((int)lpoint.x, (int)lpoint.y, 2, Mathf.Abs(h)));
                    }
                    else
                    {
                        corridors.Add(new Rect((int)rpoint.x, (int)rpoint.y, 2, Mathf.Abs(h)));
                    }
                }
            }
        }
    }
}