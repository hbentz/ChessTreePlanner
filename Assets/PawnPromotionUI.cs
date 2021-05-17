using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPromotionUI : MonoBehaviour
{
    public Pawn PromotionPawn;

    public void ReplacePawn(int pieceIndex)
    {

        // Get the chessboard to spawn the new piece
        PromotionPawn.Tile.Board.SpawnChessFigure(pieceIndex, PromotionPawn.Tile.xCoord, PromotionPawn.Tile.yCoord);

        // Destroy the pawn on the promotion tile
        Destroy(PromotionPawn.gameObject);

        // Self destruct
        Destroy(this.gameObject);
    }
}
