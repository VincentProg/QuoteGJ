using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFlip : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform TransToFlip;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Flip(Vector3 Move)
    {
        if(Move.x>0)
        {
            TransToFlip.eulerAngles = new Vector3(TransToFlip.eulerAngles.x, 90, TransToFlip.eulerAngles.z);
        }
        else if(Move.x<0)
        {
            TransToFlip.eulerAngles = new Vector3(TransToFlip.eulerAngles.x, -90, TransToFlip.eulerAngles.z);
        }
    }
}
