using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
namespace System{

    public class LobbyManager : MonoBehaviour
    {

        private bool matchFound = false;

        // Start is called before the first frame update
        void Start()
        {
            MatchType mt = new MatchType { lobbyType = GameStates.PlayerCount };
          
          //  NetworkClient.instance.Off("matchFound",(e)=> { });
          /*  NetworkClient.instance.On("matchFound",(e)=> {
           
                Match match = JsonConvert.DeserializeObject<Match>(e.data.ToString());
                //GameStates.MatchType = 1;
               // GameStates.PlayerCount = match.type;
                GameStates.MatchState = match;
                // Debug.Log("Match Index "+ match.matchIndex);
                // Debug.Log(match.players.Count);
                //Debug.Log(match.players[0].id);

                SceneManager.LoadScene("gameScene");

            });*/


            NetworkClient.instance.Emit("matchRequest", new JSONObject(JsonUtility.ToJson(mt)));

        }

        // Update is called once per frame
        void Update()
        {
           
        }

    }


    [Serializable]
    public class MatchType
    {
        public int lobbyType;
    }

}