using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject car;
    public GameObject city;
    public Canvas canvasPause;
    public Canvas canvasEnd;
    public Canvas canvasUI;
    public Canvas canvasWin;
    public PlayerController playerCon;
    public Text humansText;
    public enum GameStates { PLAY, PAUSE, END, WIN };
    public GameStates state;
    public static int humans;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("InstantiateCar", 1, 10);
        state = GameStates.PLAY;
        Time.timeScale = 1;
        canvasPause.enabled = false;
        canvasEnd.enabled = false;
        canvasWin.enabled = false;
        humans = 4;
   
    }

    // Update is called once per frame
    void Update()
    {
        // Pause and unpause
        if (Input.GetKeyDown(KeyCode.Escape) && state == GameStates.PLAY)
        {
            canvasPause.enabled = true;
            state = GameStates.PAUSE;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && state == GameStates.PAUSE)
        {
            Unpause();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            city.isStatic = !city.isStatic;
            
        }
    }

    private void LateUpdate()
    {
        // Refresh UI
        humansText.text = "=  " + humans;

        // Win
        if (humans <= 0)
        {
            state = GameStates.WIN;
        }

        // Game Over
        if (state == GameStates.END)
        {
            canvasEnd.enabled = true;
            CancelInvoke();
            playerCon.enabled = false;
        }
        else if (state == GameStates.WIN)
        {
            canvasWin.enabled = true;
            CancelInvoke();
            playerCon.enabled = false;
        }
    }

    void InstantiateCar()
    {
        Vector3 carMaker; // Global position for car spawning
        Vector3 carRotation;
        float rand = Random.Range(1, 11);

        if (rand > 5)
        {
            float posX = Random.Range(31, 44);
            carMaker = new Vector3(posX, 0.2f, 100);
            carRotation = new Vector3(0, 0, 0);
        }
        else
        {
            float posZ = Random.Range(-16, -4);
            carMaker = new Vector3(190, 0.2f, posZ);
            carRotation = new Vector3(0, 90, 0);
        }
        Instantiate(car, carMaker, Quaternion.Euler(carRotation));
    }

    public void Unpause()
    {
        canvasPause.enabled = false;
        state = GameStates.PLAY;
        Time.timeScale = 1;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
