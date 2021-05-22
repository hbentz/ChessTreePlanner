using System;

public interface IGridSpawner
{
	ChessTile[,] SpawnTiles(int rows, int columns);
}
