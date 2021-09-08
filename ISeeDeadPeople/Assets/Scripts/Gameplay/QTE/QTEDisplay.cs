using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTEDisplay : MonoBehaviour
{
    public QTE_BT assignedBT;
    public float animSpeed = 0;
    public Image displayIMG;

    [HideInInspector]
    public Sprite idleTouchSprite = null;
    [HideInInspector]
    public Sprite pressedTouchSprite = null;

    List<Sprite> animSprites = new List<Sprite>();

    float timer = 0;
    int cpt = 0;
    bool canAnimate = true;
    float cooldownTimer = 0;

    private void Start()
    {
        //-- Temporary Setup --//
        animSprites.Add(idleTouchSprite);
        animSprites.Add(pressedTouchSprite);
        displayIMG.sprite = idleTouchSprite;
    }

    private void Update()
    {
        AnimateButton();

        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            canAnimate = true;
            cooldownTimer = 0;
            cpt = 0;
        }
    }

    public void FeedbackPress()
    {
        canAnimate = false;
        displayIMG.sprite = pressedTouchSprite;
    }

    void AnimateButton()
    {
        if (canAnimate)
        {
            timer += Time.deltaTime;

            cpt = Mathf.FloorToInt(animSpeed * timer) % animSprites.Count;

            displayIMG.sprite = animSprites[cpt];
        }
    }
}
