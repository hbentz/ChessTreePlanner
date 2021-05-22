using System;
using UnityEngine;

public interface IPieceGetter
{
    GameObject PiecePrefab(Type pieceType, bool isBlack);
}