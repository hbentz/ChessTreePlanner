using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnPromotionUI : MonoBehaviour
{
    public Pawn PromotionPawn;

    public void ReplacePawn(Type chessPiece)
    {

        // Get the chessboard to spawn the new piece
        PromotionPawn.Tile.Board.SpawnChessFigure(chessPiece, PromotionPawn.isBlack, PromotionPawn.Tile.xCoord, PromotionPawn.Tile.yCoord);

        // Destroy the pawn on the promotion tile
        Destroy(PromotionPawn.gameObject);

        // Self destruct
        Destroy(this.gameObject);
    }

    public void PromoteQueen() => ReplacePawn(typeof(Queen));
    public void PromoteRook() => ReplacePawn(typeof(Queen));
    public void PromoteKnight() => ReplacePawn(typeof(Queen));
    public void PromoteBisihop() => ReplacePawn(typeof(Queen));
}
