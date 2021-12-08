using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace SpellSlinger
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public Sound[] sounds;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);


            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playOnAwake;
            }
        }

        private void Start()
        {
            Play("Menu");
            Play("Ambience");
        }

        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null) return;
            s.source.Play();
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            if (s == null) return;
            s.source.Stop();
        }
    }
}
