using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Touch_Display
{
    public string displayName;
    public QTE_BT displayButton;
    public Sprite displaySprite;
    public Sprite pressedSprite;
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

    public void AssignSprite(QTE_BT qteButton, QTEDisplay display)
    {
        foreach (Touch_Display touch in touch_Displays)
        {
            if(touch.displayButton == qteButton)
            {
                display.idleTouchSprite = touch.displaySprite;
                display.pressedTouchSprite = touch.pressedSprite;
            }
        }
    }
}
