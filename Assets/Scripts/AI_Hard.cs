using UnityEngine;

public class AI_Hard : MonoBehaviour
{
    public GameStateController gameController;

    public void MakeMove()
    {
        if (gameController.GetPlayersTurn() != "O") return;

        int bestScore = int.MinValue;
        int move = -1;

        for (int i = 0; i < 9; i++)
        {
            if (gameController.tileList[i].text == "")
            {
                gameController.tileList[i].text = "O";
                int score = Minimax(0, false);
                gameController.tileList[i].text = "";

                if (score > bestScore)
                {
                    bestScore = score;
                    move = i;
                }
            }
        }

        if (move != -1)
        {
            gameController.tileList[move].GetComponentInParent<TileController>().UpdateTile();
        }
    }

    int Minimax(int depth, bool isMaximizing)
    {
        string result = CheckWinner();
        if (result == "O") return 10 - depth;
        if (result == "X") return depth - 10;
        if (IsDraw()) return 0;

        int bestScore = isMaximizing ? int.MinValue : int.MaxValue;

        for (int i = 0; i < 9; i++)
        {
            if (gameController.tileList[i].text == "")
            {
                gameController.tileList[i].text = isMaximizing ? "O" : "X";
                int score = Minimax(depth + 1, !isMaximizing);
                gameController.tileList[i].text = "";

                bestScore = isMaximizing ? Mathf.Max(score, bestScore) : Mathf.Min(score, bestScore);
            }
        }

        return bestScore;
    }

    bool IsDraw()
    {
        foreach (var tile in gameController.tileList)
        {
            if (tile.text == "") return false;
        }
        return CheckWinner() == null;
    }

    string CheckWinner()
    {
        var t = gameController.tileList;
        string[][] winCombos = new string[][]
        {
            new[] { t[0].text, t[1].text, t[2].text },
            new[] { t[3].text, t[4].text, t[5].text },
            new[] { t[6].text, t[7].text, t[8].text },
            new[] { t[0].text, t[3].text, t[6].text },
            new[] { t[1].text, t[4].text, t[7].text },
            new[] { t[2].text, t[5].text, t[8].text },
            new[] { t[0].text, t[4].text, t[8].text },
            new[] { t[2].text, t[4].text, t[6].text }
        };

        foreach (var combo in winCombos)
        {
            if (combo[0] != "" && combo[0] == combo[1] && combo[1] == combo[2])
                return combo[0];
        }

        return null;
    }
}
