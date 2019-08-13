using System.Collections.Generic;
using System;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public enum ParticleInfo
    {
        ScoreParticle = 0,
        aaaaa,
        ssss,
        dddd,
        da,
        wq,
        w,
        e,
        r,
        t,
        

           
    }
    public static ParticleController instance;
    public TriggerParticle[] Particle;
    private Dictionary<int, ParticleSystem> ParticleCollection;

    [System.Serializable]
    public struct TriggerParticle
    {
        public ParticleInfo ParticleID;
        public ParticleSystem particleSystem;
    }

    private void Awake()
    {
        ParticleCollection = new Dictionary<int, ParticleSystem>();
        instance = this;
        for (int i = 0; i < Particle.Length; i++)
        {
            ParticleCollection[(int)Particle[i].ParticleID] = Particle[i].particleSystem;

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            for (int i = 0; i < ParticleCollection.Count; i++)
            {
                ParticleCollection[i].Stop();
                ParticleCollection[i].Play();
            }
        }

    }




    private void Init()
    {
        

    }

    public void PlayParticle()
    {
       
    }

}


