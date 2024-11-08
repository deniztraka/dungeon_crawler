﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DTWorldz.Models
{
    [Serializable]
    public class Audio
    {
        public string Name;
        public AudioClip Clip;
        [Range(0, 1)]
        public float Volume;
        [Range(.1f, 3f)]
        public float Pitch;
        public float MaxDistance;
        [HideInInspector]
        public AudioSource Source;
        public bool Loop;
    }
}