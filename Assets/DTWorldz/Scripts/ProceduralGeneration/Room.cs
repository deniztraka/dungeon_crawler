using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.ProceduralGeneration
{
    public class Room
    {
        public Rect Rect { get; set; }
        public Rect InnerRect { get; set; }  
        public List<GameObject> Objects;
        public int MaxNumberOfObjects {
            get{
                return (int)Math.Ceiling(InnerRect.size.x * InnerRect.size.y / 10);
            }
        }       

        public Room(Rect rect)
        {
            Rect = rect;
            Objects = new List<GameObject>();
        }

        public int GetSurcafeArea()
        {
            return (int)Rect.size.x * (int)Rect.size.y;
        }
    }
}