using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmashQTE : QTE
{
    public int smashCount;
    public GameObject smashQTEDisplay;

    private Player rewiredPlayer = null;
    private bool _isCompleted;

    private int internSmashCount = 0;
    private GameObject currentDisplayQTE = null;
    private Image fillIMG = null;

    protected override void OnExecute()
    {
        _isCompleted = false;
        rewiredPlayer = ReInput.players.GetPlayer("Player");

        currentDisplayQTE = Instantiate(smashQTEDisplay, null);
        currentDisplayQTE.transform.position = transform.position;

        fillIMG = currentDisplayQTE.GetComponentInChildren<Image>();
        fillIMG.fillAmount = 0;

        Image[] images = currentDisplayQTE.GetComponentsInChildren<Image>();
        images[1].sprite = QTEManager.instance.AssignSprite(Button);
    }

    public override void QTEUpdate()
    {
        if(internSmashCount < smashCount && GoodButtonSmashed())
        {
            rewiredPlayer.SetVibration(1, .9f, .05f);
            rewiredPlayer.SetVibration(2, .9f, .05f);

            internSmashCount++;
        }
        else if (internSmashCount >= smashCount)
        {
            Destroy(currentDisplayQTE);
            _isCompleted = true;
        }

        fillIMG.fillAmount = (float)internSmashCount / smashCount;
    }

    public bool GoodButtonSmashed()
    {
        //-- Debug Only Remove Later --//
        if (rewiredPlayer.GetButtonDown(ButtonName))
        {
            Debug.LogWarning($"Button {ButtonName} pressed");
        }
        return (rewiredPlayer.GetButtonDown(ButtonName));
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        string name = "";
        name = $"Smash QTE {smashCount} w/ {Button}"; 
        return name;
    }
}
