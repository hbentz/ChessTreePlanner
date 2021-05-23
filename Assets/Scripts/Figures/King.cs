using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessFigure
{
    // Start is called before the first frame update

    public override bool[,] PossibleMoves(BoardState state)
    {
        bool[,] moves = PossibleAttacks(state);

        // Castle legality check
        if (!HasMoved)  // If the king hasn't moved
        {
            int castleY = isBlack ? 7 : 0; // Figure out which rank the castle happens on

            // If the queenside rook also hasn't moved, the spaces are empty, and the king is not under threat while travelling
            bool canQueensideCastle = state.HasFriendlyPiece(0, castleY, isBlack) && !state.PieceLocations[0, castleY].HasMoved;
            for (int i = 1; i < 4; i++) canQueensideCastle = canQueensideCastle && !state.AnyPieceOn(i, castleY);
            for (int i = 2; i <= 4; i++) canQueensideCastle = canQueensideCastle && !state.Threatened(i, castleY, isBlack);
            moves[2, castleY] = canQueensideCastle;

            // If the kingside rook also hasn't moved, the spaces are empty, and the king is not under threat while travelling
            bool canKingsideCastle = state.HasFriendlyPiece(7, castleY, isBlack) && !state.PieceLocations[7, castleY].HasMoved;
            for (int i = 5; i < 7; i++) canKingsideCastle = canKingsideCastle && !state.AnyPieceOn(i, castleY);
            for (int i = 4; i < 7; i++) canKingsideCastle = canKingsideCastle && !state.Threatened(i, castleY, isBlack);
            moves[6, castleY] = canKingsideCastle;
        }

        return moves;
    }

    public override bool[,] PossibleAttacks(BoardState state)
    {
        bool[,] moves = new bool[8, 8];

        AddMoveIfOnboardAndNoCollision(xCoord + 1, yCoord, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord - 1, yCoord, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord + 1, yCoord + 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord + 1, yCoord - 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord - 1, yCoord + 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord - 1, yCoord - 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord, yCoord + 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord, yCoord - 1, state, ref moves);

        return moves;
    }
}

