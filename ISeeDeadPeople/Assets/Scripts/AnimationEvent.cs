using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvent : MonoBehaviour
{

    public void ParticlesBoom()
    {
        foreach(Transform child in transform.GetChild(0))
        child.GetComponent<ParticleSystem>().Play();
    }
}
