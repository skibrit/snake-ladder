using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MATCH TYPE 1 = online
// MATCH TYPE 2 = offline

public static class GameStates
{
    private static int matchType=2;
    private static int playerCount=4;
    private static Match matchState;
    private static Position lastPos;

    public static int MatchType
    {
        get
        {
            return matchType;
        }
        set
        {
            matchType = value;
        }
    }


    public static int PlayerCount
    {
        get
        {
            return playerCount;
        }
        set
        {
            playerCount = value;
        }
    }

    public static Match MatchState
    {
        get
        {
            return matchState;
        }
        set
        {
            matchState = value;
        }
    }

    public static Position LastPos
    {
        get
        {
            return lastPos;
        }
        set
        {
            lastPos = value;
        }
    }

    public static Match createMatchState(int playerCount)
    {
        //create matchstate
        List<Player> players = new List<Player>();
        for (int i = 0; i < playerCount; i++)
        {
            Player pl = new Player();
            pl.id = Random.Range(100, 20000).ToString();
            pl.currentPos = 1;
            players.Add(pl);
        }

        Match match = new Match();
        match.id = Random.Range(100, 20000).ToString();
        match.players = players;
        match.type = playerCount;
        match.whosTurn = 1;
        match.status = 1;
        match.matchIndex = 1;

        return match;
    }

}

/*
[System.Serializable]
public class Match
{
    private string matchID;
    private int matchType;
    private Player[] players;
    private int whosTurn;
    private int winner;
    private int status;

    Match(string mID, int mType, Player[] players)
    {
        this.matchID = mID;
        this.matchType = mType;
        this.players = players;
    }

    public string ID
    {
        get
        {
            return matchID;
        }
    }
    public int MatchType
    {
        get
        {
            return matchType;
        }
        set
        {
            matchType = value;
        }
    }

    public Player[] Players
    {
        get
        {
            return players;
        }
    }

    public int WhosTurn
    {
        get
        {
            return whosTurn;
        }
        set
        {
            whosTurn = value;
        }
    }


    public int Winner
    {
        get
        {
            return winner;
        }
        set
        {
            winner = value;
        }
    }

    public int Status
    {
        get
        {
            return status;
        }
        set
        {
            status = value;
        }
    }
}
 [System.Serializable]
public class Player
{
    private string id;
    private int currentPos;
    private bool isBot;

    Player(string id, bool isBot)
    {
        this.id = id;
        this.isBot = isBot;
    }

    public string ID
    {
        get
        {
            return id;
        }
    }
    public int CurrentPos
    {
        get
        {
            return currentPos;
        }
        set
        {
            currentPos = value;
        }

    }

    public bool IsBot
    {
        get
        {
            return isBot;
        }
        set
        {
            isBot = value;
        }

    }

}*/




public class Player
{
    public string id { get; set; }
    public bool isBot { get; set; }
    public int currentPos { get; set; }
}

public class Match
{
    public string id { get; set; }
    public List<Player> players { get; set; }
    public int type { get; set; }
    public int whosTurn { get; set; }
    public int winner { get; set; }
    public int status { get; set; }
    public int matchIndex { get; set; }
}



public class Position
{
    public int diceResult { get; set; }
    public int newPos { get; set; }
    public int nextTurn { get; set; }
    public string updateOf { get; set; }
    public int updateIndex { get; set; }
}
