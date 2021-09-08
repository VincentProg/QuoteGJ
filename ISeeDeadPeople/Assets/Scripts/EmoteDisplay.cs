using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteDisplay : MonoBehaviour
{
    public Image emoteIMG;
    public float animSpeed = 2f;

    public Sprite[] emoteSprites = null;
    private float timer = 0;
    int cpt = 0;

    private void Update()
    {
        AnimateEmote();
    }

    void AnimateEmote()
    {
        timer += Time.deltaTime;

        cpt = Mathf.FloorToInt(animSpeed * timer) % emoteSprites.Length;
        emoteIMG.sprite = emoteSprites[cpt];
    }
}
