using UnityEngine;

[System.Serializable]
public class TileData
{
    public int row;
    public int column;
    public GridManager gm;
    public Tile tile;

    public TileData(GridManager gridManager, int newRow, int newColumn, Tile tile)
    {
        row = newRow;
        column = newColumn;
        gm = gridManager;
        this.tile = tile;
    }

    public Vector2Int ToVector()
    {
        return new Vector2Int(row, column);
    }
}