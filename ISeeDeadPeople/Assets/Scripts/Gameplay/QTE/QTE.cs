using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QTE_BT
{
    CROSS,
    SQUARE,
    TRIANGLE,
    CIRCLE
}

public abstract class QTE : MonoBehaviour
{
    public QTE_BT Button;
    private bool _isRunning = false;
    public string ButtonName { get { return buttonName; } }
    private string buttonName = "";

    public bool IsRunning()
    {
        return _isRunning;
    }

    public void Execute()
    {
        if (_isRunning) return;
        ChangeButtonName();
        _isRunning = true;
        OnExecute();
    }

    protected abstract void OnExecute();

    public virtual void QTEUpdate()
    {

    }

    public virtual bool IsFinished()
    {
        return true;
    }

    private void OnValidate()
    {
        string newGameObjectName = BuildGameObjectName();
        if(gameObject.name != newGameObjectName)
        {
            gameObject.name = newGameObjectName;
        }
    }
    protected abstract string BuildGameObjectName();

    public void ChangeButtonName() 
    {
        switch (Button)
        {
            case QTE_BT.CROSS:
                buttonName = "CrossBT";
                break;
            case QTE_BT.SQUARE:
                buttonName = "SquareBT";
                break;
            case QTE_BT.TRIANGLE:
                buttonName = "TriangleBT";
                break;
            case QTE_BT.CIRCLE:
                buttonName = "CircleBT";
                break;
            default:
                break;
        }
    }
}
