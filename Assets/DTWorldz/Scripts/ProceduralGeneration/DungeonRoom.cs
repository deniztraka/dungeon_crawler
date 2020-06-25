using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ProceduralGeneration
{

    public class DungeonRoom
    {
        // Rectangular representation of room
        public Rect roomRect;
        public int debugId;
        public List<Rect> corridors = new List<Rect>();

        // private static int debugCounter = 0;

        // Children of the room generated in split()
        public DungeonRoom leftChild;
        public DungeonRoom rightChild;

        public DungeonRoom(Rect newRect)
        {
            roomRect = newRect;
            // debugId = debugCounter;
            // debugCounter++;
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        }

        public bool split(int minRoomSize)
        {
            // Should not split rooms that have already been split
            if (leftChild != null || rightChild != null)
            {
                return false;
            }

            // Randomly pick if the split will be horizontal or vertical
            bool horSplit = (Random.value < 0.5f);

            // To prevent splitting a room in one way too much
            if (roomRect.width / roomRect.height >= 1.25)
            {
                horSplit = false;
            }
            else if (roomRect.height / roomRect.width >= 1.25)
            {
                horSplit = true;
            }

            // Determine if an appropriate size is impossible
            int maxDimension = (horSplit ? (int)roomRect.height : (int)roomRect.width) - minRoomSize;
            if (maxDimension < minRoomSize)
            {
                return false;
            }

            // Calculate where the split will occur
            int split = Random.Range(minRoomSize, maxDimension);

            // Split the room into two children
            if (horSplit)
            {
                leftChild = new DungeonRoom(new Rect(roomRect.x, roomRect.y, roomRect.width, split));
                rightChild = new DungeonRoom(new Rect(roomRect.x, roomRect.y + split, roomRect.width, roomRect.height - split));
            }
            else
            {
                leftChild = new DungeonRoom(new Rect(roomRect.x, roomRect.y, split, roomRect.height));
                rightChild = new DungeonRoom(new Rect(roomRect.x + split, roomRect.y, roomRect.width - split, roomRect.height));
            }

            // Split was successful
            return true;
        }

        public void CreateCorridorBetween(Rect left, Rect right)
        {
            Rect lroom = left;
            Rect rroom = right;

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

            Debug.Log("lpoint: " + lpoint + ", rpoint: " + rpoint + ", w: " + w + ", h: " + h);

            // if the points are not aligned horizontally
            if (w != 0)
            {
                // choose at random to go horizontal then vertical or the opposite
                if (Random.Range(0, 1) > 2)
                {
                    // add a corridor to the right
                    corridors.Add(new Rect(lpoint.x, lpoint.y, Mathf.Abs(w) + 1, 1));

                    // if left point is below right point go up
                    // otherwise go down
                    if (h < 0)
                    {
                        corridors.Add(new Rect(rpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        corridors.Add(new Rect(rpoint.x, lpoint.y, 1, -Mathf.Abs(h)));
                    }
                }
                else
                {
                    // go up or down
                    if (h < 0)
                    {
                        corridors.Add(new Rect(lpoint.x, lpoint.y, 1, Mathf.Abs(h)));
                    }
                    else
                    {
                        corridors.Add(new Rect(lpoint.x, rpoint.y, 1, Mathf.Abs(h)));
                    }

                    // then go right
                    corridors.Add(new Rect(lpoint.x, rpoint.y, Mathf.Abs(w) + 1, 1));
                }
            }
            else
            {
                // if the points are aligned horizontally
                // go up or down depending on the positions
                if (h < 0)
                {
                    corridors.Add(new Rect((int)lpoint.x, (int)lpoint.y, 1, Mathf.Abs(h)));
                }
                else
                {
                    corridors.Add(new Rect((int)rpoint.x, (int)rpoint.y, 1, Mathf.Abs(h)));
                }
            }

            // Debug.Log("Corridors: ");
            // foreach (Rect corridor in corridors)
            // {
            //     Debug.Log("corridor: " + corridor);
            // }
        }
    

        public void CreateRooms()
        {
            // Current room is not a leaf
            if (leftChild != null || rightChild != null)
            {
                // Recurse through valid children
                if (leftChild != null)
                {
                    leftChild.CreateRooms();
                }
                if (rightChild != null)
                {
                    rightChild.CreateRooms();
                }


                // if there are both left and right children in this Leaf, create a corridor between them
                if (leftChild != null && rightChild != null)
                {
                    CreateCorridorBetween(leftChild.roomRect, rightChild.roomRect);
                }                
            }
            else
            {
                int roomWidth = (int)Random.Range(roomRect.width / 2, roomRect.width - 2);
                int roomHeight = (int)Random.Range(roomRect.height / 2, roomRect.height - 2);
                int roomX = (int)Random.Range(1, roomRect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, roomRect.height - roomHeight - 1);

                roomRect = new Rect(roomRect.x + roomX, roomRect.y + roomY, roomWidth, roomHeight);
            }
        }
    }
}

