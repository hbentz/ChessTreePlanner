using System;

public static class ChessNotationConversion
{
	public static string TileName(int x, int y) => Convert.ToChar(x + 97).ToString() + (y + 1).ToString();

    public static (int, int) TileCoords(string tileName) => ((int)tileName[0] - 97 , int.Parse(tileName.Substring(1)));
}
