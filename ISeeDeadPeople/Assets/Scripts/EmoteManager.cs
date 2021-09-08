using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Emote_Data
{
    public string emoteName;
    public Sprite[] emoteSprites;
}

public class EmoteManager : MonoBehaviour
{
    public static EmoteManager instance = null;
    public string debugEmoteToPlay = "Scary_Emote";
    public float destroyTime = 5f;
    public Emote_Data[] emotes;
    public GameObject emoteSource = null;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayEmoteRandomPlaceDebug(debugEmoteToPlay);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            PlayEmoteRandomNameDebug();
        }
    }

    public void PlayEmote(string name)
    {
        Emote_Data e = Array.Find(emotes, emote => emote.emoteName == name);

        GameObject emote = Instantiate(emoteSource);

        emote.GetComponent<EmoteDisplay>().emoteSprites = e.emoteSprites;

        Destroy(emote, destroyTime);
    }

    public GameObject PlayEmoteGameObject(string name)
    {
        Emote_Data e = Array.Find(emotes, emote => emote.emoteName == name);

        GameObject emote = Instantiate(emoteSource);

        emote.GetComponent<EmoteDisplay>().emoteSprites = e.emoteSprites;

        return emote;
    }

    public void PlayEmoteWithPos(string name, Vector3 pos)
    {
        Emote_Data e = Array.Find(emotes, emote => emote.emoteName == name);

        GameObject emote = Instantiate(emoteSource, pos, Quaternion.identity);

        emote.GetComponent<EmoteDisplay>().emoteSprites = e.emoteSprites;

        Destroy(emote, destroyTime);
    }

    public void PlayEmoteWithTransform(string name, Transform trans)
    {
        Emote_Data e = Array.Find(emotes, emote => emote.emoteName == name);

        GameObject emote = Instantiate(emoteSource, new Vector3(trans.position.x, trans.position.y + 0.5f, -3), Quaternion.identity, trans);

        emote.GetComponent<EmoteDisplay>().emoteSprites = e.emoteSprites;

        Destroy(emote, destroyTime);
    }

    public void PlayEmoteRandomPlaceDebug(string name)
    {
        Emote_Data e = Array.Find(emotes, emote => emote.emoteName == name);

        float x = UnityEngine.Random.Range(-5,5);
        float y = UnityEngine.Random.Range(-5,5);

        GameObject emote = Instantiate(emoteSource);
        emote.transform.position = new Vector3(emote.transform.position.x + x, emote.transform.position.y + y, emote.transform.position.z);


        emote.GetComponent<EmoteDisplay>().emoteSprites = e.emoteSprites;

        Destroy(emote, destroyTime);
    }

    public void PlayEmoteRandomNameDebug()
    {
        int value = UnityEngine.Random.Range(0, emotes.Length);
        Debug.Log($"VALUE :{value}");

        PlayEmoteRandomPlaceDebug(emotes[value].emoteName);
    }
}
