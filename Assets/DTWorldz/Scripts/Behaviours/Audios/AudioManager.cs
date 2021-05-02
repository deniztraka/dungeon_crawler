using System;
using System.Collections;
using System.Collections.Generic;
using DTWorldz.Models;
using UnityEngine;

namespace DTWorldz.Behaviours.Audios
{
    public class AudioManager : MonoBehaviour
    {
        private Audio lastSound;
        public Audio[] Sounds;
        public bool AutoPlayFirst = false;

        public void Awake()
        {
            foreach (var sound in Sounds)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                sound.Source.pitch = sound.Pitch;
                sound.Source.loop = sound.Loop;
                sound.Source.spatialBlend = 1f;
                sound.Source.minDistance = 1;
                sound.Source.maxDistance = sound.MaxDistance;
                sound.Source.rolloffMode = AudioRolloffMode.Linear;
            }
        }

        public void Start()
        {
            if (AutoPlayFirst && Sounds != null && Sounds.Length > 0)
            {
                Play(Sounds[0].Name);
            }
        }

        public void Play(string name)
        {
            if (lastSound != null)
            {
                //Debug.Log(name + ":" + lastSound.Name);
            }

            //dont try to play walking sound everytime
            if (name == "Walking" && lastSound != null && lastSound.Name == "Walking" && lastSound.Source.isPlaying)
            {
                return;
            }

            var sounds = Array.FindAll(Sounds, s => s.Name.Equals(name));
            if (sounds != null && sounds.Length > 0)
            {
                var randomSoundIndex = UnityEngine.Random.Range(0, sounds.Length);
                var sound = sounds[randomSoundIndex];
                if (name == "Walking")
                {
                    sound.Source.pitch = sound.Pitch;
                }
                sound.Source.Play();

                //Debug.Log(name + ", " + sound.Clip.name);
                lastSound = sound;
            }
        }

        internal void SetVolume(float v)
        {
            if (lastSound != null && lastSound.Source != null && lastSound.Source.isPlaying)
            {
                lastSound.Source.volume = v;
            }
        }

        public void SetCurrentPitch(float pitch)
        {
            if (lastSound != null && lastSound.Source != null && lastSound.Source.isPlaying)
            {
                if (lastSound.Name == "Walking")
                {
                    lastSound.Source.pitch = pitch;
                }
            }
        }

        public void Stop(string name)
        {
            //dont try to play walking sound everytime
            if (lastSound != null && lastSound.Name == name && !lastSound.Source.isPlaying)
            {
                return;
            }

            var sound = Array.Find(Sounds, s => s.Name.Equals(name));
            if (sound != null && sound.Source.isPlaying)
            {
                sound.Source.Stop();
            }
        }

        public void Stop()
        {
            //dont try to play walking sound everytime
            if (lastSound != null && lastSound.Source.isPlaying)
            {
                lastSound.Source.Stop();
            }
        }

        public void StopAll()
        {
            var sound = Array.Find(Sounds, s => s.Name.Equals(name));
            if (sound != null && sound.Source.isPlaying)
            {
                sound.Source.Stop();
            }
        }


    }
}
