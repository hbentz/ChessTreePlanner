using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMoves(BoardState state)
    {
        bool[,] moves = new bool[8, 8];

        AddMoveIfOnboardAndNoCollision(xCoord + 2, yCoord + 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord + 2, yCoord - 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord + 1, yCoord + 2, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord - 1, yCoord + 2, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord - 2, yCoord + 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord - 2, yCoord - 1, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord + 1, yCoord - 2, state, ref moves);
        AddMoveIfOnboardAndNoCollision(xCoord - 1, yCoord - 2, state, ref moves);

        return moves;
    }

}
