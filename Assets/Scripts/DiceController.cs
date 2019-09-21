using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController : MonoBehaviour
{

    public static DiceController instance;
    private SpriteRenderer rend;
    private Sprite[] diceSprites;
    [HideInInspector]
    public bool shouldRollDice;
    private bool willInvokeAITurn;
  
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
        rend = GetComponent<SpriteRenderer>();
        diceSprites = Resources.LoadAll<Sprite>("dice/");
        rend.sprite = diceSprites[1];
        shouldRollDice = true;
        willInvokeAITurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStates.MatchType == 2)
        {
            //offline with ai
            if ((Input.GetMouseButtonDown(0) || GameController.instance.autoTrigger) 
                && shouldRollDice && GameController.instance.whosTurn == 1)
            {
                GameController.instance.resetTimer();
                RollDice();
            }
            else if (shouldRollDice && GameController.instance.whosTurn > 1 && !willInvokeAITurn)
            {
                willInvokeAITurn = true;
                Invoke("AITurn", 1f);
            }
        }else if (GameStates.MatchType==1)
        {
            //online with players
            if ((Input.GetMouseButtonDown(0) || GameController.instance.autoTrigger) 
                && shouldRollDice && GameController.instance.whosTurn 
                == GameStates.MatchState.matchIndex)
            {
                GameController.instance.resetTimer();
                RollDice();
            }
        }
    }

    public void AITurn()
    {
        RollDice();
        willInvokeAITurn = false;
    }

    public void RollDice()
    {
        shouldRollDice = false;
        StartCoroutine("RollDice_courotine");
    }


    public void DiceSimulator()
    {
        shouldRollDice = false; 
        StartCoroutine("simulateDice");
    }


    //simulate couroutine method
    private IEnumerator simulateDice()
    {
        int result = 0;
        int preResult = GameStates.LastPos.diceResult;
        for (int i = 0; i < 15; i++)
        {
            result = Random.Range(0, 6);
            rend.sprite = diceSprites[result];
            yield return new WaitForSeconds(0.08f);
        }
        rend.sprite = diceSprites[preResult-1];
        GameController.instance.movePlayer(preResult);
    }

    //dice couroutine method
    private IEnumerator RollDice_courotine()
    {
        int result = 0;

        for(int i = 0; i < 15; i++)
        {
            result = Random.Range(0, 6);
            rend.sprite = diceSprites[result];
            yield return new WaitForSeconds(0.08f);
        }

        result = result+1;
        GameController.instance.movePlayer(result);
    }
}
