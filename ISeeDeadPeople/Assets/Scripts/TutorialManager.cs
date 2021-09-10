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
    public Item itemNear = null;
    bool hasInteractedItem = false;

    private float timerFear = 4;
    private float internTimerFear = 0;
    private GameObject hunterToScare = null;
    bool needToScare = false;

    //void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 10, 250, 20), $"CPT STATE : " + cpt);
    //    if(itemNear)
    //    GUI.Label(new Rect(10, 30, 500, 20), $"Number : {GameManager.Instance.player.itemsNear.Count} | Near Object : " + itemNear.transform.name);
    //    GUI.Label(new Rect(10, 50, 250, 20), $"Hunter 1 : " + hunters[0].transform.name);
    //    GUI.Label(new Rect(10, 70, 250, 20), $"Fear Timer : " + internTimerFear);
    //    GUI.Label(new Rect(10, 90, 250, 20), $"Need Scare : " + needToScare);
    //}

    private void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer("Player");
        tutoTexts[0].SetActive(true);

    }

    public void Update()
    {
        Tutorial();
        FearHunter(hunterToScare);
    }

    void FearHunter(GameObject hunterToScare)
    {
        if(needToScare)
        {
            Debug.Log("Scaring Hunter");

            if(internTimerFear > 0)
            {
                internTimerFear -= Time.deltaTime;
            }

            else if(internTimerFear <= 0)
            {
                internTimerFear = 0;

                AudioManager.instance.Play("Fear");
                hunterToScare.SetActive(false);
                hasInteractedItem = false;

                cpt++;
                UpdateCheckboxes();

                hunterToScare = null;
                needToScare = false;
            }
        }
    } 

    //IEnumerator FearHunter()
    //{
    //    EmoteManager.instance.PlayEmoteWithPos("Surprise_Emote", hunterToScare.transform.position + new Vector3(0, 1.7f, 0));

    //    yield return new WaitForSeconds(3);
    //    AudioManager.instance.Play("Fear");
    //    hunterToScare.SetActive(false);
    //    hasInteractedItem = false;

    //    cpt++;
    //    UpdateCheckboxes();
    //}

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

                if (GameManager.Instance.player.itemsNear.Count > 0)
                {
                    itemNear = GameManager.Instance.player.itemsNear[0];

                    if (hasInteractedItem)
                    {
                        internTimerFear = 2;
                        needToScare = true;
                        hasInteractedItem = false;
                    }

                    if (itemNear.isInteracting && !hasInteractedItem)
                    {
                        hasInteractedItem = true;
                    }


                    if (rewiredPlayer.GetButtonDown("SquareBT"))
                        {
                        tutoTexts[cpt].SetActive(false);

                        hunterToScare = hunters[0];

                       

                       
                                //StartCoroutine(FearHunter(hunters[0]));
                                //EmoteManager.instance.PlayEmoteWithPos("Surprise_Emote", hunters[0].transform.position + new Vector3(0, 1.7f, 0));
                                //AudioManager.instance.Play("Fear");
                                //hunters[0].SetActive(false);
                                //hasInteractedItem = false;

                                //cpt++;
                                //UpdateCheckboxes();
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

                Debug.Log($"Item near : {GameManager.Instance.player.itemsNear.Count}");

                if (GameManager.Instance.player.itemsNear.Count > 0)
                {
                    itemNear = GameManager.Instance.player.itemsNear[0];

                    if (hasInteractedItem)
                    {
                        internTimerFear = 2;
                        needToScare = true;
                        hasInteractedItem = false;
                    }

                    if (itemNear.isInteracting && !hasInteractedItem)
                    {
                        hasInteractedItem = true;
                    }


                    if (rewiredPlayer.GetButtonDown("SquareBT"))
                    {
                        tutoTexts[cpt].SetActive(false);

                        hunterToScare = hunters[1];




                        //StartCoroutine(FearHunter(hunters[0]));
                        //EmoteManager.instance.PlayEmoteWithPos("Surprise_Emote", hunters[0].transform.position + new Vector3(0, 1.7f, 0));
                        //AudioManager.instance.Play("Fear");
                        //hunters[0].SetActive(false);
                        //hasInteractedItem = false;

                        //cpt++;
                        //UpdateCheckboxes();
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
