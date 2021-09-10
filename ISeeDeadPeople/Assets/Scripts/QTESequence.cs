using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTESequence : MonoBehaviour
{
    public bool playOnStart = true;

    private QTE[] _qtes;
    private int _currentQTEIndex = 0;


    private bool _isRunning = false;
    public bool IsRunning { get { return _isRunning; } }

    private bool _isStart = true;
    public bool sequenceFinished = false;
    public bool sequenceLost = false;

    private void Start()
    {
        if (playOnStart)
        {
            Play();
            _isStart = false;
        }
    }

    private void OnEnable()
    {
        if(playOnStart && !_isStart)
        {
            Play();
        }
    }

    private void Awake()
    {
        _qtes = GetComponentsInChildren<QTE>();
    }

    private void Update()
    {
        if (!_isRunning) return;
        
        if(_currentQTEIndex >= _qtes.Length)
        {
            Stop();
            return;
        }

        QTE qte = null;
        bool qteFinished = false;
        do
        {
            qte = _qtes[_currentQTEIndex];

            if (!qte.IsRunning())
            {
                qte.Execute();
            }
            else
            {
                qte.QTEUpdate();
            }
            qteFinished = qte.IsFinished();
            if (qteFinished)
            {
                _currentQTEIndex++;
            }
        } while ((qte != null) && qteFinished && (_currentQTEIndex < _qtes.Length));

        if(_qtes[_qtes.Length - 1].IsFinished())
        {
            sequenceFinished = true;
        }
    }

    public void Play()
    {
        _currentQTEIndex = 0;
        _isRunning = true;
    }

    public void Stop()
    {
        _isRunning = false;
    }
}
