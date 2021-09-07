using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Touch_Display
{
    public string displayName;
    public QTE_BT displayButton;
    public Sprite displaySprite;
}

public class QTEManager : MonoBehaviour
{
    public static QTEManager instance = null;
    public List<Touch_Display> touch_Displays = new List<Touch_Display>();
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        else if (instance != null) 
        {
            Destroy(gameObject);
        }
    }

    public Sprite AssignSprite(QTE_BT qteButton)
    {
        Sprite spriteToAssign = null;

        foreach (Touch_Display display in touch_Displays)
        {
            if(display.displayButton == qteButton)
            {
                spriteToAssign = display.displaySprite;
            }
        }

        return spriteToAssign;
    }
}
