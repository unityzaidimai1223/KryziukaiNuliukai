using UnityEngine;
using System.Collections.Generic;

public class AI_Medium : MonoBehaviour
{
    public GameStateController gameController;

    public void MakeMove()
    {
        if (gameController.GetPlayersTurn() != "O") return;

        int move = FindBestMove("O"); 
        if (move == -1) move = FindBestMove("X"); 
        if (move == -1) move = GetRandomMove(); 

        if (move != -1)
        {
            gameController.tileList[move].GetComponentInParent<TileController>().UpdateTile();
        }
    }

    private int FindBestMove(string player)
    {
        for (int i = 0; i < 9; i++)
        {
            if (gameController.tileList[i].text == "")
            {
                gameController.tileList[i].text = player;
                if (IsWinning(player))
                {
                    gameController.tileList[i].text = "";
                    return i;
                }
                gameController.tileList[i].text = "";
            }
        }
        return -1;
    }

    private bool IsWinning(string player)
    {
        var t = gameController.tileList;
        return (t[0].text == player && t[1].text == player && t[2].text == player) ||
               (t[3].text == player && t[4].text == player && t[5].text == player) ||
               (t[6].text == player && t[7].text == player && t[8].text == player) ||
               (t[0].text == player && t[3].text == player && t[6].text == player) ||
               (t[1].text == player && t[4].text == player && t[7].text == player) ||
               (t[2].text == player && t[5].text == player && t[8].text == player) ||
               (t[0].text == player && t[4].text == player && t[8].text == player) ||
               (t[2].text == player && t[4].text == player && t[6].text == player);
    }

    private int GetRandomMove()
    {
        List<int> available = new List<int>();
        for (int i = 0; i < 9; i++)
        {
            if (gameController.tileList[i].text == "")
                available.Add(i);
        }

        if (available.Count == 0) return -1;
        return available[Random.Range(0, available.Count)];
    }
}
