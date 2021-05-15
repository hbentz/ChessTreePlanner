using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessFigure
{
    // Start is called before the first frame update
    public override bool[,] PossibleMove()
    {     
        return new bool[8, 8];
    }
}
