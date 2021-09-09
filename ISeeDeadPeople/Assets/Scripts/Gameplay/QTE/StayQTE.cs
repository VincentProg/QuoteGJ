using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StayQTE : QTE
{
    public float stayTime;
    public GameObject stayQTEDisplay;

    public float timeBeforeLose = 6f;
    private float timeLose = 0f;
    private bool isLost = false;

    private Player rewiredPlayer = null;
    private bool _isCompleted;

    private float internStayTimer = 0;
    private GameObject currentDisplayQTE = null;
    private Image fillIMG = null;

    bool needReload;
    private QTEDisplay displaySetup;

    protected override void OnExecute()
    {
        _isCompleted = false;
        rewiredPlayer = ReInput.players.GetPlayer("Player");

        currentDisplayQTE = Instantiate(stayQTEDisplay, null);
        currentDisplayQTE.transform.position = transform.position;

        fillIMG = currentDisplayQTE.GetComponentInChildren<Image>();
        fillIMG.fillAmount = 0;

        displaySetup = currentDisplayQTE.GetComponent<QTEDisplay>();
        QTEManager.instance.AssignSprite(Button, displaySetup);
    }

    public override void QTEUpdate()
    {
        if (!_isCompleted)
        {
            fillIMG.fillAmount = internStayTimer / stayTime;
        }

        if(internStayTimer <= stayTime && GoodButtonStay())
        {
            internStayTimer += Time.deltaTime;
            displaySetup.FeedbackPress();
            rewiredPlayer.SetVibration(1, .6f, .1f);
            rewiredPlayer.SetVibration(2, .6f, .1f);
            needReload = false;
        }

        else if(rewiredPlayer.GetButtonUp(ButtonName) && !_isCompleted)
        {
            needReload = true;
        }

        if(internStayTimer > 0 && needReload)
        {
            internStayTimer -= Time.deltaTime * 1.5f;
        }
        
        if(internStayTimer <= 0) { internStayTimer = 0; needReload = false; }

        else if(!_isCompleted && internStayTimer >= stayTime)
        {
            Destroy(currentDisplayQTE);
            _isCompleted = true;
        }

        else if (timeLose > timeBeforeLose && !_isCompleted)
        {
            isLost = true;
        }

        if (isLost)
        {
            Destroy(currentDisplayQTE);
            Destroy(GetComponentInParent<QTESequence>().gameObject);
        }
    }

    public bool GoodButtonStay()
    {
        return (rewiredPlayer.GetButton(ButtonName));
    }

    public override bool IsFinished()
    {
        return _isCompleted;
    }

    protected override string BuildGameObjectName()
    {
        string name = "";
        name = $"Stay QTE {stayTime} sec w/ {Button}"; 
        return name;
    }
}
