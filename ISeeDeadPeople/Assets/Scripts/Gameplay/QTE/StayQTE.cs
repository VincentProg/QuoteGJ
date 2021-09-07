using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StayQTE : QTE
{
    public float stayTime;
    public GameObject stayQTEDisplay;

    private Player rewiredPlayer = null;
    private bool _isCompleted;

    private float internStayTimer = 0;
    private GameObject currentDisplayQTE = null;
    private Image fillIMG = null;

    bool needReload;

    protected override void OnExecute()
    {
        _isCompleted = false;
        rewiredPlayer = ReInput.players.GetPlayer("Player");

        currentDisplayQTE = Instantiate(stayQTEDisplay, null);
        currentDisplayQTE.transform.position = transform.position;

        fillIMG = currentDisplayQTE.GetComponentInChildren<Image>();
        fillIMG.fillAmount = 0;

        Image[] images = currentDisplayQTE.GetComponentsInChildren<Image>();
        images[1].sprite = QTEManager.instance.AssignSprite(Button);
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
            rewiredPlayer.SetVibration(1, .6f, .1f);
            rewiredPlayer.SetVibration(2, .6f, .1f);
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
