using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ALLMenus : MonoBehaviour
{

    /* DESCRIPTION : 
     
    Ce script détient les différents comportements possibles pour tout type de menu.
    NECESSITE : S'il est dédié au menu pause, il doit connaître l'interface du menu pause

    CONSIGNE : Attribuez ce script au Canvas. Quand vous créerez des boutons, vous pourrez glisser le Canvas (depuis la hiérarchie) dans un OnClick() event du boutton.
    Vous aurez ainsi accès à toutes les fonctions présentes dans ce script.

    
    DEROULEMENT DU SCRIPT:
    1/ On s'assure que le menu pause est désactivé au lancement de la scène.

    2/ On vérifie si le joueur appuit sur la touche pause désignée, on active ou désactive la pause si la touche est appuyée.
    Pour que la pause fonctionne, on set le time scale à 0, autrement dit on arrête le temps. On active au même instant l'interface Pause.
    Lorsqu'on arrête la pause, on set le time scale à 1, soit la valeur d'écoulement du temps normale. On désactive au même instant l'interface Pause.

    3/ La fonction Retry() relance la scène actuelle.

    4/ La fonction Quit() fait quitter l'application (ne fonctionne que sur la build).

     5/ La fonction Menu() retourne à la scène située au build index 0 (qui doit correspondre au Menu).

    6/ La ofnction NextLevel() permet de charger le lvl suivant
     
    */
    public bool isMenuPause;

    [Header("Main Menu")]
    public string nextScene;

    [Header("Pause Menu")]   
    public KeyCode keyPause;
    public GameObject pauseInterface;
    
    bool isPaused;


    // _1
    private void Start()
    {
        if(pauseInterface != null)
        pauseInterface.SetActive(false);
    }

    // _2
    private void Update()
    {
        if(isMenuPause && pauseInterface != null)
        if (Input.GetKeyDown(keyPause))
        {
            if (isPaused)
            {
                Resume();
            }
            else Pause();
        }
    }

    // _2
    public void Pause()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseInterface.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // _2
    public void Resume()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseInterface.SetActive(false);
            Time.timeScale = 1;
        }
    }
    // _3
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // _4
    public void Quit()
    {
        Application.Quit();
    }

    // _5
    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    // _6
    public void LoadScene(int i = 0)
    {
        SceneManager.LoadScene(i);
    }



}
