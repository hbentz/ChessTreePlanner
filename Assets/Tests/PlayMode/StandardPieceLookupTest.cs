using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class StandardPieceLookupTest
{
    [UnityTest]
    public IEnumerator PieceLookupHasStandardPieces()
    {
        SceneManager.LoadScene("InteractionScene");
        yield return null;

        StandardPieceLookup lookup = GameObject.FindObjectOfType<StandardPieceLookup>();

        Bishop blackBishop = lookup.PiecePrefab(typeof(Bishop), true).GetComponent<Bishop>();
        Assert.That(blackBishop.isBlack && blackBishop.GetType() == typeof(Bishop));

        Bishop whiteBishop = lookup.PiecePrefab(typeof(Bishop), false).GetComponent<Bishop>();
        Assert.That(!whiteBishop.isBlack && whiteBishop.GetType() == typeof(Bishop));

        King blackKing = lookup.PiecePrefab(typeof(King), true).GetComponent<King>();
        Assert.That(blackKing.isBlack && blackKing.GetType() == typeof(King));

        King whiteKing = lookup.PiecePrefab(typeof(King), false).GetComponent<King>();
        Assert.That(!whiteKing.isBlack && whiteKing.GetType() == typeof(King));

        Knight blackKnight = lookup.PiecePrefab(typeof(Knight), true).GetComponent<Knight>();
        Assert.That(blackKnight.isBlack && blackKnight.GetType() == typeof(Knight));

        Knight whiteKnight = lookup.PiecePrefab(typeof(Knight), false).GetComponent<Knight>();
        Assert.That(!whiteKnight.isBlack && whiteKnight.GetType() == typeof(Knight));

        Pawn blackPawn = lookup.PiecePrefab(typeof(Pawn), true).GetComponent<Pawn>();
        Assert.That(blackPawn.isBlack && blackPawn.GetType() == typeof(Pawn));

        Pawn whitePawn = lookup.PiecePrefab(typeof(Pawn), false).GetComponent<Pawn>();
        Assert.That(!whitePawn.isBlack && whitePawn.GetType() == typeof(Pawn));

        Queen blackQueen = lookup.PiecePrefab(typeof(Queen), true).GetComponent<Queen>();
        Assert.That(blackQueen.isBlack && blackQueen.GetType() == typeof(Queen));

        Queen whiteQueen = lookup.PiecePrefab(typeof(Queen), false).GetComponent<Queen>();
        Assert.That(!whiteQueen.isBlack && whiteQueen.GetType() == typeof(Queen));

        Rook blackRook = lookup.PiecePrefab(typeof(Rook), true).GetComponent<Rook>();
        Assert.That(blackRook.isBlack && blackRook.GetType() == typeof(Rook));

        Rook whiteRook = lookup.PiecePrefab(typeof(Rook), false).GetComponent<Rook>();
        Assert.That(!whiteRook.isBlack && whiteRook.GetType() == typeof(Rook));
    }
}
