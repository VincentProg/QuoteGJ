using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static VictoryManager instance;
    public GameObject Victoire;
    public GameObject Defeat;
    void Awake()
    {
        transform.parent = null;

        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
    

    public void EndGame(bool Victory)
    {
        if(Victory)
        {
            Instantiate(Victoire);
        }else
        {
            Instantiate(Defeat);
        }
    }
}
