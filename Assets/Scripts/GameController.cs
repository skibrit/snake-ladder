using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class GameController : MonoBehaviour
{
    
    [HideInInspector]
    public bool isGameRunning;
    [HideInInspector]
    public int whosTurn;
    private int diceResult;
    private int nextTurn;

    public static GameController instance;
    [SerializeField]
    private float timeLeft = 10.0f;
    private bool shouldIncrementTimer;
    public bool autoTrigger;

    //players
    public GameObject boardWayPoint;
    public GameObject playerPrefab;
    [HideInInspector]
    public GameObject[] players;
    private PathController[] pathControls;
   

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
        int playerCount = GameStates.PlayerCount;
        Sprite[] chipSprites = Resources.LoadAll<Sprite>("chips/");

        Debug.Log("PlayerCount " + playerCount);
        PathController prefabComponent = playerPrefab.GetComponent<PathController>();

        int counter = -1;
        foreach (Transform g in boardWayPoint.transform.GetComponentsInChildren<Transform>())
        {
            if (counter >-1)
            {
                // Debug.Log(g.name);
                prefabComponent.wayPoints[counter] = g;
            }
            counter++;
        }

        //create array
        players = new GameObject[playerCount];
        pathControls = new PathController[playerCount];

        //create players based on user input
        for (int i = 0; i < playerCount; i++)
        {
           GameObject player = Instantiate(playerPrefab, transform.position, Quaternion.identity) as GameObject;
           player.GetComponent<SpriteRenderer>().sprite = chipSprites[i];
           PathController pc = player.GetComponent<PathController>();
           players[i] = player;
           pathControls[i] = pc;
        }
        diceResult = -1;
        isGameRunning = false;
        autoTrigger = false;

        if (GameStates.MatchType == 1)
        {
            whosTurn = GameStates.MatchState.whosTurn;
        }
        else
        {
            whosTurn = Random.Range(0, playerCount) + 1;
        }

        if (GameStates.MatchState.matchIndex == whosTurn)
        {
            Invoke("startCountdown", 5f);
        }

        //UIController.instance.switchTurn();

        UIController.instance.startGame();


        NetworkClient.instance.On("positionUpdate", (e) => {
            Debug.Log("Got Player Pos");
            Position newPos = JsonConvert.DeserializeObject<Position>(e.data.ToString());
            Debug.Log(newPos);
            GameStates.LastPos = newPos;
            GameStates.MatchState.whosTurn = newPos.nextTurn;
            DiceController.instance.DiceSimulator();
        });
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("Whose Turn "+ whosTurn);

        PathController pc = pathControls[whosTurn-1];

        if (pc.moveAllowed && (pc.moveForwardCount == 0) && pc.jumpIndex == 0)
        {
            pc.moveAllowed = false;
            if (GameStates.MatchType == 2)
            {
                whosTurn = whosTurn == GameStates.PlayerCount ? 1 : ++whosTurn;
            }
            else if (GameStates.MatchType == 1)
            {
                if (GameStates.MatchState.matchIndex == whosTurn)
                {
                    whosTurn = whosTurn == GameStates.MatchState.type ? 1 : ++whosTurn;
                }
                else
                {
                    whosTurn = GameStates.MatchState.whosTurn;
                }
            }
            DiceController.instance.shouldRollDice = true;
            UIController.instance.switchTurn();

            if (GameStates.MatchState.matchIndex == whosTurn)
            {
                Invoke("startCountdown", 5f);
            }
        }

        if (pc.currentPathIndex == pc.totalWayPoint)
        {
            gameOver(whosTurn);
        }

        if (shouldIncrementTimer)
        {
            timeLeft -= Time.deltaTime;
            UIController.instance.updateTimerText(timeLeft);
            if (timeLeft<=0)
            {
                autoTrigger = true;
            }
        }
    }

    private void startCountdown()
    {
        shouldIncrementTimer = true;
    }

    public void resetTimer()
    {
        if (shouldIncrementTimer)
        {
            UIController.instance.hideTimerText();
            shouldIncrementTimer = false;
            timeLeft = 10f;
            autoTrigger = false;
        }
        else
        {
            CancelInvoke("startCountdown");
        }
       
    }

    public void movePlayer(int result)
    {
        if (whosTurn == GameStates.MatchState.matchIndex)
        {
            NewPos npos = new NewPos() { diceResult = result,newPos=8 };
            NetworkClient.instance.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(npos)));
        }
        diceResult = result;
        Debug.Log("Dice Result " + diceResult);
        pathControls[whosTurn - 1].moveForwardCount = diceResult;
        pathControls[whosTurn - 1].moveAllowed = true;
    }


    public void gameOver(int winner)
    {
        UIController.instance.gameOver(winner);
    }
}


[System.Serializable]
public class NewPos
{
    public int diceResult;
    public int newPos;
}
