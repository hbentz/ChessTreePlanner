using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class StandardGridTest
{
    [UnityTest]
    public IEnumerator EnsureStandard64Grid()
    {
        SceneManager.LoadScene("InteractionScene");
        yield return null;

        ChessBoard board = GameObject.FindObjectOfType<ChessBoard>();
        ChessTile[] tiles = GameObject.FindObjectsOfType<ChessTile>();

        Assert.That(tiles.Length == 64);

        // Check that each tile is created only once
        for (int y = 0; y < 8; y++)
        {
            string tileRank = (y + 1).ToString();

            for (int x = 0; x < 8; x++)
            {
                string tileFile = Convert.ToChar(x + 97).ToString();
                Assert.That(board.transform.GetComponentsInChildren<Transform>().Where(t => t.name == tileFile + tileRank).Count() == 1);
            }

        }

    }
}
