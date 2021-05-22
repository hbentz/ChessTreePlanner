using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardPieceLookup : MonoBehaviour, IPieceGetter
{
    // Responsible for holding references to all of the chess piece prefabs
    [SerializeField] private List<GameObject> _chessPiecePrefabs;

    private Dictionary<Type, GameObject> _blackPieceDict = new Dictionary<Type, GameObject>();
    private Dictionary<Type, GameObject> _whitePieceDict = new Dictionary<Type, GameObject>();

    private void Awake()
    {
        // Track each piece prefab
        foreach (GameObject prefab in _chessPiecePrefabs)
        {
            ChessFigure prefabFigure = prefab.GetComponent<ChessFigure>();
            if (prefabFigure.isBlack) _blackPieceDict.Add(prefabFigure.GetType(), prefab);
            else _whitePieceDict.Add(prefabFigure.GetType(), prefab);
        }
    }

    public GameObject PiecePrefab(Type pieceType, bool isBlack) => isBlack ? _blackPieceDict[pieceType] : _whitePieceDict[pieceType];
}
