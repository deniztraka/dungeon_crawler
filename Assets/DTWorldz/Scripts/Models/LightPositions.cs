using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models
{
    public class LightPositions
    {
        public List<Vector3> Top;
        public List<Vector3> Bottom;
        public List<Vector3> Right;
        public List<Vector3> Left;

        public LightPositions()
        {
            Top = new List<Vector3>();
            Bottom = new List<Vector3>();
            Right = new List<Vector3>();
            Left = new List<Vector3>();
        }
    }
}