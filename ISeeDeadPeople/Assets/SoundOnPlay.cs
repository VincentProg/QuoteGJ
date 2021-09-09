using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public string SongName;
    void Start()
    {
        AudioManager.instance.Play(SongName);
    }

    // Update is called once per frame
   
}
