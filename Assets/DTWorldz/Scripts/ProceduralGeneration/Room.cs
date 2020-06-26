using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ProceduralGeneration
{
    public class Room
    {
        public Rect Rect { get; set; }
        public Rect InnerRect { get; set; }        

        public Room(Rect rect)
        {
            Rect = rect;
        }

        public int GetSurcafeArea()
        {
            return (int)Rect.size.x * (int)Rect.size.y;
        }
    }
}