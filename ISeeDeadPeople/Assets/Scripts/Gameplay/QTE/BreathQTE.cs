using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreathQTE : QTE
{
    public float stayTime;
    public GameObject stayQTEDisplay;

    private Player rewiredPlayer = null;
    private bool _isCompleted;

    private float internStayTimer = 0;
    private GameObject currentDisplayQTE = null;
    private Image fillIMG = null;

    bool needReload;
    bool hasBeenLaunched = false;
    private QTEDisplay displaySetup;

    protected override void OnExecute()
    {
        _isCompleted = false;
        rewiredPlayer = ReInput.players.GetPlayer("Player");

        currentDisplayQTE = Instantiate(stayQTEDisplay, null);
        currentDisplayQTE.transform.parent = GameManager.Instance.player.transform;
        currentDisplayQTE.transform.position = transform.position;

        fillIMG = currentDisplayQTE.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        //fillIMG = currentDisplayQTE.GetComponentInChildren<Image>();
        fillIMG.fillAmount = 0;

        displaySetup = currentDisplayQTE.GetComponent<QTEDisplay>();
        QTEManager.instance.AssignSprite(Button, displaySetup);
        displaySetup.transform.position = GameManager.Instance.player.transform.position + new Vector3(0,0, 4.5f);

        hasBeenLaunched = true;
    }

    public override void QTEUpdate()
    {
        if (!_isCompleted)
        {
            GameManager.Instance.player.canMove = false;
            fillIMG.fillAmount = internStayTimer / stayTime;
        }

        if (internStayTimer <= stayTime && GoodButtonStay())
        {
            internStayTimer += Time.deltaTime;
            displaySetup.FeedbackPress();
            rewiredPlayer.SetVibration(1, .6f, .1f);
            rewiredPlayer.SetVibration(2, .6f, .1f);
            needReload = false;
        }

        else if (rewiredPlayer.GetButtonUp(ButtonName) && !_isCompleted)
        {
            needReload = true;
        }

        if (internStayTimer > 0 && needReload)
        {
            internStayTimer -= Time.deltaTime * 1.5f;
        }

        if (internStayTimer <= 0) { internStayTimer = 0; needReload = false; }

        else if (!_isCompleted && internStayTimer >= stayTime)
        {
            Destroy(currentDisplayQTE);
            _isCompleted = true;
            GameManager.Instance.player.Blast();
        }

        if (rewiredPlayer.GetButtonUp(ButtonName))
        {
            if (hasBeenLaunched)
            {
                Destroy(currentDisplayQTE);
                _isCompleted = true;
                GameManager.Instance.player.canMove = true;
            }
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
        name = $"Blast QTE {stayTime} sec w/ {Button}";
        return name;
    }
}
