using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public static UIController instance;
    public GameObject gameOverPanel;
    public GameObject activeDice;
    public Text whosTurnText;
    public Text winnerText;
    public GameObject[] playerHuds;
    public float distance;
    public Text timerText;
    public GameObject timerObj;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
      //  DontDestroyOnLoad(gameObject);
        Screen.fullScreen = false;
        Screen.SetResolution(480, 800, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void startGame()
    {
        GameObject[] players = GameController.instance.players;

        for(int i = 0; i < players.Length; i++)
        {
            GameObject g =  playerHuds[i].transform.Find("player_chip").gameObject;
            GameObject gT = playerHuds[i].transform.Find("player_name").gameObject;
            g.GetComponent<Image>().sprite = players[i].GetComponent<SpriteRenderer>().sprite;
            if (GameStates.MatchType == 2)
            {
                if (i == 0)
                {
                    gT.GetComponent<Text>().text = "MySelf";
                }
                else
                {
                    gT.GetComponent<Text>().text = "Com " + (i + 1);
                }
            }else if (GameStates.MatchType == 1)
            {
                if ((i+1) == GameStates.MatchState.matchIndex)
                {
                    gT.GetComponent<Text>().text = "MySelf";
                }
                else
                {
                    gT.GetComponent<Text>().text = "Player " + (i + 1);
                }
            }
           
            playerHuds[i].SetActive(true);
        }
        switchTurn();
    }


    public void switchTurn()
    {
        string playerName = "my Turn";
        int whosTurn = GameController.instance.whosTurn;
        int totalPlayer = GameStates.PlayerCount;

        if (GameStates.MatchType == 2)
        {
            if (whosTurn > 1)
            {
                playerName = "Com " + (totalPlayer > 2 && whosTurn > 2 ? (whosTurn - 1) + "" : "");
            }
        }else if (GameStates.MatchType == 1)
        {
            if (whosTurn != GameStates.MatchState.matchIndex)
            {
                playerName = "Player " + whosTurn;
            }
        }
        whosTurnText.text = playerName;

        GameObject gT = playerHuds[whosTurn-1].transform.Find("player_name").gameObject;
        activeDice.SetActive(true);
        if (whosTurn == 2 || whosTurn==4)
        {
            activeDice.transform.position = new Vector2(gT.transform.position.x - distance, gT.transform.position.y);
        }
        else
        {
            activeDice.transform.position = new Vector2(gT.transform.position.x + distance, gT.transform.position.y);
        }
    }

    public void updateTimerText(float newTimer)
    {
        timerObj.SetActive(true);
        int timeLeft = (int) newTimer;
        timerText.text = timeLeft.ToString();
    }

    public void hideTimerText()
    {
        timerObj.SetActive(false);
    }

    public void gameOver(int winner)
    {
        winnerText.text = "Player " + winner;
        gameOverPanel.SetActive(true);
    }

    public void loadMenuScene()
    {
        GameStates.MatchType = 0;
        GameStates.PlayerCount = 0;
        SceneManager.LoadScene("menuScene");
    }

    public void loadMatchScene(int matchType)
    {
        GameStates.MatchType = matchType;
        SceneManager.LoadScene("matchScene");
    }


    public void detectGameType(int playerCount)
    {
        GameStates.PlayerCount = playerCount;
        if (GameStates.MatchType == 1)
        {
            loadLobbyScene();
        }
        else
        {
            GameStates.MatchState = GameStates.createMatchState(playerCount);
            loadGameScene();
        }
    }

    public void loadGameScene()
    {

        SceneManager.LoadScene("gameScene");
    }

    public void loadLobbyScene()
    {
        SceneManager.LoadScene("lobbyScene");
    }

    public void resetGameScene()
    {
        SceneManager.LoadScene("gameScene");
    }

}
