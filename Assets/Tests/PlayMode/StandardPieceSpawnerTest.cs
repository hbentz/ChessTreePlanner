using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class StandardPieceSpawnerTest
{
    [UnityTest]
    public IEnumerator EnsureStandard32Pieces()
    {
        SceneManager.LoadScene("InteractionScene");
        yield return null;

        StandardPieceSpawner spawner = GameObject.FindObjectOfType<StandardPieceSpawner>();
        ChessBoard board = GameObject.FindObjectOfType<ChessBoard>();
        spawner.SpawnStart(board);

        // Make sure that there is exactly 32 pieces
        Assert.That(GameObject.FindObjectsOfType<ChessFigure>().Length == 32);

        // 8 White major pieces on the right squares
        Assert.That(!board.transform.Find("a1").GetComponentInChildren<Rook>().isBlack);
        Assert.That(!board.transform.Find("b1").GetComponentInChildren<Knight>().isBlack);
        Assert.That(!board.transform.Find("c1").GetComponentInChildren<Bishop>().isBlack);
        Assert.That(!board.transform.Find("d1").GetComponentInChildren<Queen>().isBlack);
        Assert.That(!board.transform.Find("e1").GetComponentInChildren<King>().isBlack);
        Assert.That(!board.transform.Find("f1").GetComponentInChildren<Bishop>().isBlack);
        Assert.That(!board.transform.Find("g1").GetComponentInChildren<Knight>().isBlack);
        Assert.That(!board.transform.Find("h1").GetComponentInChildren<Rook>().isBlack);

        // 8 Black major pieces on the right squares
        Assert.That(board.transform.Find("a8").GetComponentInChildren<Rook>().isBlack);
        Assert.That(board.transform.Find("b8").GetComponentInChildren<Knight>().isBlack);
        Assert.That(board.transform.Find("c8").GetComponentInChildren<Bishop>().isBlack);
        Assert.That(board.transform.Find("d8").GetComponentInChildren<Queen>().isBlack);
        Assert.That(board.transform.Find("e8").GetComponentInChildren<King>().isBlack);
        Assert.That(board.transform.Find("f8").GetComponentInChildren<Bishop>().isBlack);
        Assert.That(board.transform.Find("g8").GetComponentInChildren<Knight>().isBlack);
        Assert.That(board.transform.Find("h8").GetComponentInChildren<Rook>().isBlack);

        // 8 pawns for each player on the right squares
        for (int i = 0; i < 8; i++)
        {
            string tileFile = ((char)(i + 97)).ToString();
            Assert.That(!board.transform.Find(tileFile + "2").GetComponentInChildren<Pawn>().isBlack);
            Assert.That(board.transform.Find(tileFile + "7").GetComponentInChildren<Pawn>().isBlack);
        }

    }
}
