using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedQTE : QTE
{
    public float timedTime = 0;
    public float errorOffset = 0.3f;
    public GameObject timedQTEDisplay;

    private Player rewiredPlayer = null;
    private bool _isCompleted;

    private GameObject currentDisplayQTE = null;
    private Image timedIMG = null;

    private QTEDisplay displaySetup;

    private float internTimer = 0;
    bool isMissed = false;
    RectTransform timedRect = null;
    Image[] images = null;

    protected override void OnExecute()
    {
        _isCompleted = false;
        rewiredPlayer = ReInput.players.GetPlayer("Player");

        currentDisplayQTE = Instantiate(timedQTEDisplay, null);
        currentDisplayQTE.transform.position = transform.position;

        images = currentDisplayQTE.GetComponentsInChildren<Image>();
        images[0].fillAmount = errorOffset / timedTime;
        images[1].fillAmount = 0;

        displaySetup = currentDisplayQTE.GetComponent<QTEDisplay>();
        QTEManager.instance.AssignSprite(Button, displaySetup);
    }

    public override void QTEUpdate()
    {
        if (!isMissed)
        {
            images[1].fillAmount = internTimer / timedTime;
            internTimer += Time.deltaTime;
        }

        if(timedTime - errorOffset <= internTimer && timedTime + errorOffset >= internTimer && GoodButtonSmashed())
        {
            _isCompleted = true;
            Destroy(currentDisplayQTE);
        }

        else if (internTimer < timedTime - errorOffset && GoodButtonSmashed())
        {
            isMissed = true;
        }
        
        
        
        

        if(internTimer > timedTime + errorOffset)
        {
            isMissed = true;
            Debug.LogWarning("QTE missed/out of time");
        }

        if (isMissed)
        {
            Destroy(currentDisplayQTE);
            Destroy(GetComponentInParent<QTESequence>().gameObject);
        }
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
        name = $"Timed QTE {timedTime} w/ {Button}"; 
        return name;
    }
}
