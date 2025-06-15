using UnityEngine;
using System.Collections.Generic;

public class AI_Easy : MonoBehaviour
{
    public GameStateController gameController;

    public void MakeMove()
    {
        Debug.Log("AI is making a move");


        if (gameController.GetPlayersTurn() != "O") return;

        List<TileController> availableTiles = new List<TileController>();
        foreach (var tile in gameController.tileList)
        {
            if (tile.text == "")
                availableTiles.Add(tile.GetComponentInParent<TileController>());
        }

        if (availableTiles.Count > 0)
        {
            int randomIndex = Random.Range(0, availableTiles.Count);
            availableTiles[randomIndex].UpdateTile();
        }
    }
}
