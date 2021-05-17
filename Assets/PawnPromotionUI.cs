using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPromotionUI : MonoBehaviour
{
    public Pawn PromotionPawn;

    public void ReplacePawn(int pieceIndex)
    {
        // Get the chessboard to spawn the new piece
        PromotionPawn.Tile.Board.SpawnChessFigure(pieceIndex, PromotionPawn.Tile.xCoord, PromotionPawn.Tile.xCoord);
        
        // Destroy the pawn and then this object
        Destroy(PromotionPawn.gameObject);
        Destroy(this.gameObject);
    }
}
