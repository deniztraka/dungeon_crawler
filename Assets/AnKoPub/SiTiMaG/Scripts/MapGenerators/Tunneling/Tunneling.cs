using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tunneling : Generator
{
    readonly int MaxTunnels, MaxTunnelLength, MinTunnelLength, TunnelWidth,
        MaxRRoomSize, MinRRoomSize, MaxCRoomRadius, MinCRoomRadius;
    readonly bool BuildRectRoom, BuildCircleRoom, RandomTunnWidth;
    readonly List<Rect> RectRoomsAndTunnels;         //storage for rectangle rooms and tunnels data
    readonly List<Circle> CircleRooms;               //storage for circle rooms data    

    public Tunneling(int mapWidth, int mapHeight, int maxTunnels, int maxTunnelsLength, int minTunnelLength, int tunnelWidth,
                        int maxRectRoomSize, int minRecrRoomSize, int maxCircleRoomSize, int minCircleRoomSize,
                        bool buildRectRoom, bool buildCircleRoom, bool randomTunWidth,
                        Tilemap floorMap, Tilemap wallMap, TileBase floorTile, TileBase wallTile)
    {
        Width = mapWidth;
        Height = mapHeight;
        MaxTunnels = maxTunnels;
        MaxTunnelLength = maxTunnelsLength;
        MinTunnelLength = minTunnelLength;
        TunnelWidth = tunnelWidth;
        MaxRRoomSize = maxRectRoomSize;
        MinRRoomSize = minRecrRoomSize;
        MaxCRoomRadius = maxCircleRoomSize;
        MinCRoomRadius = minCircleRoomSize;        
        BuildRectRoom = buildRectRoom;
        BuildCircleRoom = buildCircleRoom;
        RandomTunnWidth = randomTunWidth;
        FloorTile = floorTile;
        WallTile = wallTile;
        FloorMap = floorMap;
        WallMap = wallMap;
        MapData = new bool[Height, Width];
        RectRoomsAndTunnels = new List<Rect>();
        CircleRooms = new List<Circle>();
    }

    public override void GenerateMap()
    {
        var currentPoint = new Vector2(Random.Range(0, Width), Random.Range(0, Height));
        Vector2 randomDir = new Vector2();
        Vector2 lastDir = new Vector2();
        bool lastWasHall = false;
        for (int i = 0; i <= MaxTunnels;)
        {
            while (randomDir == lastDir || randomDir == -lastDir)
                randomDir = new List<Vector2> { Vector2.up, Vector2.down, Vector2.right, Vector2.left }[Random.Range(0, 4)];

            int nextStep = Random.Range(0, 3);

            if (nextStep == 0)
            {
                currentPoint = GenerateTunnel(currentPoint, ref randomDir);
                lastWasHall = true;
                i++;
                lastDir = randomDir;
            }
            else if (nextStep == 1 && lastWasHall && BuildRectRoom)
            {
                Vector2 nextStart = GenerateRectRoom(currentPoint, ref randomDir);
                currentPoint = nextStart;
                lastWasHall = false;
                lastDir = randomDir;
                i++;
            }
            else if (nextStep == 2 && lastWasHall && BuildCircleRoom)
            {
                Vector2 nextStart = GenerateCircleRoom(currentPoint, ref randomDir);
                currentPoint = nextStart;
                lastWasHall = false;
                lastDir = randomDir;
                i++;
            }
            else continue;
        }
        ConvertCircleRoomsData();
        ConvertRectRooms();
    }

    Vector2 GenerateTunnel(Vector2 start, ref Vector2 direction)
    {
        var randomLength = Random.Range(MinTunnelLength, MaxTunnelLength);
        var tunnel = CheckRoomBounds(
                     new Rect(start, new Vector2(direction.x == 0 ? (RandomTunnWidth ? Random.Range(0, TunnelWidth) : TunnelWidth) : randomLength * direction.x,
                                                 direction.y == 0 ? (RandomTunnWidth ? Random.Range(0, TunnelWidth) : TunnelWidth) : randomLength * direction.y))
                                                );
        RectRoomsAndTunnels.Add(tunnel);
        return new Vector2(tunnel.xMax, tunnel.yMax);
    }

    Vector2 GenerateRectRoom(Vector2 start, ref Vector2 direction)
    {
        var heigth = Random.Range(MinRRoomSize, MaxRRoomSize);
        var width = Random.Range(MinRRoomSize, MaxRRoomSize);
        Vector2 offsetX = new Vector2(Random.Range(1, width), 0);
        Vector2 offsetY = new Vector2(0, Random.Range(1, heigth));
        Vector2 next = start;

        if (direction.y > 0)
        {
            var rect = CheckRoomBounds(new Rect((start - offsetX), new Vector2(width, heigth)));
            RectRoomsAndTunnels.Add(rect);
            direction = new List<Vector2> { Vector2.up, Vector2.right, Vector2.left }[Random.Range(0, 3)];
            next = RandomPointOnRectBorder(rect, direction);
        }
        else if (direction.y < 0)
        {
            var rect = CheckRoomBounds(new Rect((start - offsetX), new Vector2(width, -heigth)));
            RectRoomsAndTunnels.Add(rect);
            direction = new List<Vector2> { Vector2.down, Vector2.right, Vector2.left }[Random.Range(0, 3)];
            next = RandomPointOnRectBorder(rect, direction);
        }
        else if (direction.x > 0)
        {
            var rect = CheckRoomBounds(new Rect((start - offsetY), new Vector2(width, heigth)));
            RectRoomsAndTunnels.Add(rect);
            direction = new List<Vector2> { Vector2.down, Vector2.right, Vector2.up }[Random.Range(0, 3)];
            next = RandomPointOnRectBorder(rect, direction);
        }
        else if (direction.x < 0)
        {
            var rect = CheckRoomBounds(new Rect((start - offsetY), new Vector2(-width, heigth)));
            RectRoomsAndTunnels.Add(rect);
            direction = new List<Vector2> { Vector2.down, Vector2.left, Vector2.up }[Random.Range(0, 3)];
            next = RandomPointOnRectBorder(rect, direction);

        }
        return next;
    }

    Vector2 GenerateCircleRoom(Vector2 start, ref Vector2 direction)
    {
        var radius = Random.Range(MinCRoomRadius, MaxCRoomRadius);
        Vector2 next = start;

        if (direction.y > 0)
        {
            var circleData = new Circle(start - new Vector2(0, radius), radius);
            CircleRooms.Add(circleData);
            next = circleData.RandomPointInCircle();
            direction = new List<Vector2> { Vector2.up, Vector2.left, Vector2.right }[Random.Range(0, 3)];
        }
        else if (direction.y < 0)
        {
            var circleData = new Circle(start + new Vector2(0, radius), radius);
            CircleRooms.Add(circleData);
            next = circleData.RandomPointInCircle();
            direction = new List<Vector2> { Vector2.down, Vector2.left, Vector2.right }[Random.Range(0, 3)];
        }
        else if (direction.x > 0)
        {
            var circleData = new Circle(start - new Vector2(radius, 0), radius);
            CircleRooms.Add(circleData);
            next = circleData.RandomPointInCircle();
            direction = new List<Vector2> { Vector2.down, Vector2.up, Vector2.right }[Random.Range(0, 3)];
        }
        else if (direction.x < 0)
        {
            var circleData = new Circle(start + new Vector2(radius, 0), radius);
            CircleRooms.Add(circleData);
            next = circleData.RandomPointInCircle();
            direction = new List<Vector2> { Vector2.down, Vector2.up, Vector2.left }[Random.Range(0, 3)];
        }
        return next;
    }

    Dictionary<string, Vector2> MapBorder(Dictionary<Vector2, bool> data)
    {
        var minX = data.Min(x => x.Key.x);
        var maxX = data.Max(x => x.Key.x);
        var minY = data.Min(y => y.Key.y);
        var maxY = data.Max(y => y.Key.y);

        return new Dictionary<string, Vector2> { { "MinPoint", new Vector2(minX, minY) }, { "MaxPoint", new Vector2(maxX, maxY) } };
    }

    Vector2 RandomPointInRect(Rect rect)
    {
        return new Vector2(Random.Range(rect.xMin, rect.xMax), Random.Range(rect.yMin, rect.yMax));
    }

    Vector2 RandomPointOnRectBorder(Rect r, Vector2 dir)
    {
        if (dir == Vector2.right) return new Vector2(r.xMax, Random.Range(r.yMin, r.yMax));
        else if (dir == Vector2.left) return new Vector2(r.xMin, Random.Range(r.yMin, r.yMax));
        else if (dir == Vector2.up) return new Vector2(Random.Range(r.xMin, r.xMax), r.yMin);
        else if (dir == Vector2.down) return new Vector2(Random.Range(r.xMin, r.xMax), r.yMax);
        else return RandomPointInRect(r);
    }

    void ConvertRectRooms() //convert the data of each rectangle room and tunnel to map data
    {
        foreach (var room in RectRoomsAndTunnels)
            for (int x = (int)Mathf.Min(room.xMin, room.xMax); x <= (int)Mathf.Max(room.xMin, room.xMax); x++)
                for (int y = (int)Mathf.Min(room.yMin, room.yMax); y <= (int)Mathf.Max(room.yMin, room.yMax); y++)
                {
                    MapData[y, x] = false;
                }
    }

    void ConvertCircleRoomsData() //convert the data of each circle room to map data
    {
        foreach (var room in CircleRooms)
        {
            foreach (var point in room.CircleData)
            {
                if (point.x < 1 || point.x > Width - 2 || point.y < 1 || point.y > Height - 2) continue;
                else
                {
                    MapData[(int)point.y, (int)point.x] = false;
                }
            }
        }
    }

    Rect CheckRoomBounds(Rect rect)
    {
        if (rect.xMax < 1) rect.xMax = 1;
        else if (rect.xMax > Width - 2) rect.xMax = Width - 2;
        if (rect.xMin < 1) rect.xMin = 1;
        else if (rect.xMin > Width - 2) rect.xMin = Width - 2;
        if (rect.yMax < 1) rect.yMax = 1;
        else if (rect.yMax > Height - 2) rect.yMax = Height - 2;
        if (rect.yMin < 1) rect.yMin = 1;
        else if (rect.yMin > Height - 2) rect.yMin = Height - 2;
        return rect;
    }
}