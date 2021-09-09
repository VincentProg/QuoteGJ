using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Animator> ListButton;
    public int ButtonToActivate;
    public float Delay;
    private float DelaySet;
    private Player RewiredPlayer;
    public bool VerticalMenu;
    private float input;
    void Start()
    {
        DelaySet = Delay;
        NextButton();
        RewiredPlayer = ReInput.players.GetPlayer("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(VerticalMenu)
        {
            input = Input.GetAxis("Vertical");
        }else
        {
            input = Input.GetAxis("Horizontal");
        }
/*        Debug.Log(input);*/
        if(input>0 && Delay<0)
        {
            Delay = DelaySet;
            NextButton();
        }else if(input<0 && Delay < 0)
        {
            Delay = DelaySet;
            PreviousButton();
        }

        if(Delay>=0)
        {
            Delay -= Time.deltaTime;
        }

        if (RewiredPlayer.GetButtonDown("CrossBT"))
        {
            ListButton[ButtonToActivate].gameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void NextButton()
    {
        Debug.Log("cc");
        ListButton[ButtonToActivate].SetBool("Activate", false);
        ButtonToActivate += 1;
        if(ButtonToActivate>ListButton.Count-1)
        {
            ButtonToActivate = 0;
        }
        ListButton[ButtonToActivate].SetBool("Activate", true);
    }

    public void PreviousButton()
    {
        ListButton[ButtonToActivate].SetBool("Activate", false);
        ButtonToActivate -= 1;
        if (ButtonToActivate < 0)
        {
            ButtonToActivate = ListButton.Count-1;
        }
        ListButton[ButtonToActivate].SetBool("Activate", true);
    }
}
