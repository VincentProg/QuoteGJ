using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Rewired;

public class TutorialManager : MonoBehaviour
{
    private int cpt = 0;
    public GameObject tutorialCanva = null;
    public GameObject[] checkboxes = null;
    public GameObject[] tutoTexts = null;
    public GameObject[] hunters = null;
    public Room[] rooms = null;
    public BoxCollider[] interactionColliders = null;

    bool canInteract = false;
    bool tutoFinished = false;
    GameObject interactDisplay = null;

    public SceneChanger sceneChanger;

    private Player rewiredPlayer = null;

    private void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer("Player");
        tutoTexts[0].SetActive(true);

    }

    public void Update()
    {
        Tutorial();
    }

    void Tutorial()
    {
        if(cpt == 0)
        {
            if (rooms[1].playerIsInRoom)
            {
                if (rewiredPlayer.GetButton("MoveHorizontal"))
                {
                    tutoTexts[cpt].SetActive(false);
                    cpt++;
                    UpdateCheckboxes();
                }
            }
        }

        if(cpt == 1)
        {
            if (rooms[1].playerIsInRoom)
            {
                //if (canInteract)
                //{
                //    interactDisplay = EmoteManager.instance.PlayEmoteGameObject("Interact_Emote");
                //    interactDisplay.transform.position = new Vector3(GameManager.Instance.player.transform.position.x, GameManager.Instance.player.transform.position.y + 0.7f, -2);
                //    interactDisplay.transform.parent = GameManager.Instance.player.transform;
                //}

                Debug.Log($"Item near : {GameManager.Instance.player.itemsNear.Count}");

                if(GameManager.Instance.player.itemsNear.Count > 0)
                {
                    if (rewiredPlayer.GetButtonDown("SquareBT"))
                    {
                        tutoTexts[cpt].SetActive(false);

                        EmoteManager.instance.PlayEmoteWithPos("Surprise_Emote", hunters[0].transform.position + new Vector3(0, 1.7f, 0));
                        AudioManager.instance.Play("Fear");
                        hunters[0].SetActive(false);

                        cpt++;
                        UpdateCheckboxes();
                    }
                }
            }
        }

        if(cpt == 2)
        {
            if (rooms[0].playerIsInRoom)
            {
                if (rewiredPlayer.GetButton("MoveHorizontal"))
                {
                    tutoTexts[cpt].SetActive(false);
                    cpt++;
                    UpdateCheckboxes();
                }
            }
        }

        if(cpt == 3)
        {
            if (rooms[0].playerIsInRoom)
            {
                if(!rooms[0].myCandles[0].isOn && !rooms[0].myCandles[2].isOn && !rooms[0].myCandles[1].isOn)
                {
                    tutoTexts[cpt].SetActive(false);
                    cpt++;
                    UpdateCheckboxes();
                }

                //if (rewiredPlayer.GetButton("CircleBT"))
                //{
                //}
            }
        }

        if (cpt == 4)
        {
            if (rooms[0].playerIsInRoom)
            {
                //if (canInteract)
                //{
                //    interactDisplay = EmoteManager.instance.PlayEmoteGameObject("Interact_Emote");
                //    interactDisplay.transform.position = new Vector3(GameManager.Instance.player.transform.position.x, GameManager.Instance.player.transform.position.y + 0.7f, -2);
                //    interactDisplay.transform.parent = GameManager.Instance.player.transform;
                //}

                if(GameManager.Instance.player.itemsNear.Count > 0)
                {
                    if (rewiredPlayer.GetButton("SquareBT"))
                    {
                        tutoTexts[cpt].SetActive(false);

                        AudioManager.instance.Play("Fear");
                        EmoteManager.instance.PlayEmoteWithPos("Surprise_Emote", hunters[1].transform.position + new Vector3(0,1.7f,0));
                        hunters[1].SetActive(false);

                        cpt++;
                        UpdateCheckboxes();
                    }
                }
            }
        }
    }

    void UpdateCheckboxes()
    {
        if(cpt > 4) { tutorialCanva.SetActive(false); tutoFinished = true; Debug.Log("Tutorial is finished"); StartCoroutine(LoadLevel()) ; return; }
        checkboxes[cpt - 1].SetActive(true);
        tutoTexts[cpt].SetActive(true);
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(6);
        sceneChanger.ChangeScene("Prototype");
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        canInteract = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        if(interactDisplay != null)
    //        {
    //            Destroy(interactDisplay);
    //        }

    //        canInteract = false;
    //    }
    //}
}
