
using UnityEngine;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    [Header("TitleBar References")]
    public Image playerXIcon;                                        
    public Image playerOIcon;                                        
    public InputField player1InputField;                            
    public InputField player2InputField;                             
    public Text winnerText;                                          

    [Header("Misc References")]
    public GameObject endGameState;                                  

    [Header("Asset References")]
    public Sprite tilePlayerO;                                       
    public Sprite tilePlayerX;                                       
    public Sprite tileEmpty;                                         
    public Text[] tileList;                                          

    [Header("GameState Settings")]
    public Color inactivePlayerColor;                                
    public Color activePlayerColor;                                  
    public string whoPlaysFirst;                                     

    [Header("Private Variables")]
    private string playerTurn;                                       
    private string player1Name;                                      
    private string player2Name;                                      
    private int moveCount;


    public bool isAIActive = false;



    void CallAIMove()
    {
        AI_Easy easy = FindObjectOfType<AI_Easy>();
        if (easy != null)
        {
            Debug.Log("Calling Easy AI");
            easy.MakeMove();
            return;
        }

        AI_Medium medium = FindObjectOfType<AI_Medium>();
        if (medium != null)
        {
            Debug.Log("Calling Medium AI");
            medium.MakeMove();
            return;
        }

        AI_Hard hard = FindObjectOfType<AI_Hard>();
        if (hard != null)
        {
            Debug.Log("Calling Hard AI");
            hard.MakeMove();
            return;
        }

        Debug.Log("❌ No AI script found in scene!");
    }




    public void UpdatePlayerInput()
    {
        if (!isAIActive) return; 

        bool playerTurnIsX = (playerTurn == "X");
        for (int i = 0; i < tileList.Length; i++)
        {
            var button = tileList[i].GetComponentInParent<Button>();
            button.interactable = playerTurnIsX && (tileList[i].text == "");
        }
    }



    private void Start()
    {
        
        playerTurn = whoPlaysFirst;
        if (playerTurn == "X") playerOIcon.color = inactivePlayerColor;
        else playerXIcon.color = inactivePlayerColor;

        
        player1InputField.onValueChanged.AddListener(delegate { OnPlayer1NameChanged(); });
        player2InputField.onValueChanged.AddListener(delegate { OnPlayer2NameChanged(); });

        
        player1Name = player1InputField.text;
        player2Name = player2InputField.text;
    }

    
    public void EndTurn()
    {
        moveCount++;
        if (tileList[0].text == playerTurn && tileList[1].text == playerTurn && tileList[2].text == playerTurn) GameOver(playerTurn);
        else if (tileList[3].text == playerTurn && tileList[4].text == playerTurn && tileList[5].text == playerTurn) GameOver(playerTurn);
        else if (tileList[6].text == playerTurn && tileList[7].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn);
        else if (tileList[0].text == playerTurn && tileList[3].text == playerTurn && tileList[6].text == playerTurn) GameOver(playerTurn);
        else if (tileList[1].text == playerTurn && tileList[4].text == playerTurn && tileList[7].text == playerTurn) GameOver(playerTurn);
        else if (tileList[2].text == playerTurn && tileList[5].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn);
        else if (tileList[0].text == playerTurn && tileList[4].text == playerTurn && tileList[8].text == playerTurn) GameOver(playerTurn);
        else if (tileList[2].text == playerTurn && tileList[4].text == playerTurn && tileList[6].text == playerTurn) GameOver(playerTurn);
        else if (moveCount >= 9) GameOver("D");
        else
            ChangeTurn();
    }

    
    public void ChangeTurn()
    {
        
        playerTurn = (playerTurn == "X") ? "O" : "X";
        if (playerTurn == "X")
        {
            playerXIcon.color = activePlayerColor;
            playerOIcon.color = inactivePlayerColor;
        }
        else
        {
            playerXIcon.color = inactivePlayerColor;
            playerOIcon.color = activePlayerColor;
        }

        
        

        UpdatePlayerInput();

        if (isAIActive && playerTurn == "O")
        {
            Invoke("CallAIMove", 0.5f);
        }

    }


    /// <param name="winningPlayer">X O D</param>
    private void GameOver(string winningPlayer)
    {
        switch (winningPlayer)
        {
            case "D":
                winnerText.text = "DRAW";
                break;
            case "X":
                winnerText.text = player1Name;
                break;
            case "O":
                winnerText.text = player2Name;
                break;
        }
        endGameState.SetActive(true);
        ToggleButtonState(false);
    }

    
    public void RestartGame()
    {
        
        moveCount = 0;
        playerTurn = whoPlaysFirst;
        ToggleButtonState(true);
        endGameState.SetActive(false);

        
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<TileController>().ResetTile();
        }

        UpdatePlayerInput();

    }


    private void ToggleButtonState(bool state)
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<Button>().interactable = state;
        }
    }


    public string GetPlayersTurn()
    {
        return playerTurn;
    }

    
    public Sprite GetPlayerSprite()
    {
        if (playerTurn == "X") return tilePlayerX;
        else return tilePlayerO;
    }

    
    public void OnPlayer1NameChanged()
    {
        player1Name = player1InputField.text;
    }

    
    public void OnPlayer2NameChanged()
    {
        player2Name = player2InputField.text;
    }
}
