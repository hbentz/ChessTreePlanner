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
        IPieceSpawner pieceSpawner = GameObject.FindGameObjectWithTag("GameController").GetComponent<IPieceSpawner>();
        pieceSpawner.SpawnPiece(chessPiece, PromotionPawn.isBlack, PromotionPawn.Tile.name, PromotionPawn.Tile.Board);

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
