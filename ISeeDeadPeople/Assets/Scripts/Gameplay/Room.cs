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
    }
}
