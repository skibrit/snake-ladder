using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using Newtonsoft.Json;
//using UnityEngine.SceneManagement;

public class NetworkClient : SocketIOComponent
{

    private string networkID;
    public static NetworkClient instance;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
             instance = this;
        }
        setupEvents();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void setupEvents()
    {
        On("register", (e) =>
            {
                Debug.Log(e);
                networkID = e.data["id"].ToString().Trim('"');
                Debug.Log("Your id is " + networkID);
              //  UIController.instance.loadMatchScene(2);
            }
        );
        On("disconnect", (e) =>
        {
            Debug.Log("User Disconnected");
        }
        );

        On("matchFound", (e) =>
        {
            Match match = JsonConvert.DeserializeObject<Match>(e.data.ToString());
            GameStates.MatchState = match;
            //  GameStates.MatchState = GameStates.createMatchState(GameStates.PlayerCount);
            UIController.instance.loadGameScene();
        }
       );
    }

 
    public string getID()
    {
        return networkID;
    }
}
