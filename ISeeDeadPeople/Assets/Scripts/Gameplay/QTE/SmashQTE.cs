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

    private QTEDisplay displaySetup;

    protected override void OnExecute()
    {
        _isCompleted = false;
        rewiredPlayer = ReInput.players.GetPlayer("Player");

        currentDisplayQTE = Instantiate(smashQTEDisplay, null);
        currentDisplayQTE.transform.position = transform.position;

        fillIMG = currentDisplayQTE.GetComponentInChildren<Image>();
        fillIMG.fillAmount = 0;

        displaySetup = currentDisplayQTE.GetComponent<QTEDisplay>();
        QTEManager.instance.AssignSprite(Button, displaySetup);
    }

    public override void QTEUpdate()
    {
        if(internSmashCount < smashCount && GoodButtonSmashed())
        {
            rewiredPlayer.SetVibration(1, .9f, .05f);
            rewiredPlayer.SetVibration(2, .9f, .05f);

            displaySetup.FeedbackPress();

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
