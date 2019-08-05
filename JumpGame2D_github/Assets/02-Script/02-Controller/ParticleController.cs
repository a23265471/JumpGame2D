using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public static ParticleController instance;
    public ParticleSystem[] particles;


    private void Awake()
    {
        instance = this;
        particles = new ParticleSystem[3];
        particles[0] = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    public void PlayParticle()
    {
        particles[0].Stop();
        particles[0].Play();
    }

}
