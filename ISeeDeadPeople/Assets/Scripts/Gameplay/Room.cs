using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Room : MonoBehaviour
{
    public string roomName;
    public List<GameObject> enemiesCollider = new List<GameObject>();
    public List<GameObject> interactionCollider = new List<GameObject>();
    public List<Candle> myCandles = new List<Candle>();
    public bool isOn = false;

    public bool playerIsInRoom = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesCollider.Add(other.gameObject);
        }

        if (other.gameObject.CompareTag("Interaction"))
        {
            interactionCollider.Add(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInRoom = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            enemiesCollider.Remove(other.gameObject);
        }

        if (other.gameObject.CompareTag("Interaction"))
        {
            interactionCollider.Remove(other.gameObject);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            playerIsInRoom = false;
        }
    }

    public void TurnOn()
    {
        isOn = true;
        foreach(Candle candle in myCandles)
        {
            candle.turnOn();
        }
    }
    public void TurnOff(Vector3 v)
    {
        isOn = false;
        foreach (Candle candle in myCandles)
        {
            candle.turnOff(candle.transform.position.x - v.x <= 0);
        }
    }
}
