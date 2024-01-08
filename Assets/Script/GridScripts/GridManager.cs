using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private int maxRow;
    [SerializeField] private int maxColumn;
    [SerializeField] private Dictionary<Vector2Int, TileData> mapTiles = new Dictionary<Vector2Int, TileData>();
    private Grid gridData;
    public delegate void GridGenerated(Dictionary<Vector2Int, TileData> mapTiles);
    public GridGenerated onGridGenerated;
    [SerializeField] private Vector3 offsetGrid;


    [SerializeField] private List<Vector2Int> BreadPositions;
    [SerializeField] private List<Vector2Int> LettucePositions;
    [SerializeField] private List<Vector2Int> TomatoPositions;
    [SerializeField] private List<Vector2Int> BaconPositions;
    [SerializeField] private List<Vector2Int> CheddarPositions;

    public int MaxRow { get => maxRow; }
    public int MaxColumn { get => maxColumn; }
    public Dictionary<Vector2Int, TileData> MapTiles { get => mapTiles; }

    private void Awake()
    {
        gridData = GetComponent<Grid>();
        GenerateGrid();
    }

    private void Start()
    {
        if (BreadPositions.Count != 2) { Debug.Log("Not 2 Bread"); return; }
        FillStartGrid();
    }

    public bool CheckFinishLevel()
    {
        int FillCount = 0;
        Tile LastTile = null;
        foreach (var tileData in mapTiles.Values)
        {
            if (tileData.tile.isFill)
            {
                FillCount++;
                LastTile = tileData.tile;
            }
        }
        if (FillCount == 1 && LastTile.layers[0] == LastTile.Bread && LastTile.layers[LastTile.layers.Count-1] == LastTile.Bread) return true;
        else return false;
    }

    public void RestoreTiles()
    {
        Tile originTile = GameManager.instance.saveMove.tileOrigin;
        Tile destinationTile = GameManager.instance.saveMove.tileDestination;
        originTile.layers.Clear();
        destinationTile.layers.Clear();
        originTile.RefreshIngredients();
        destinationTile.RefreshIngredients();
        originTile.layers = GameManager.instance.saveMove.ingredientsOrigin;
        destinationTile.layers = GameManager.instance.saveMove.ingredientsDestination;
        originTile.RefreshIngredients();
        destinationTile.RefreshIngredients();
        GameManager.instance.uiManager.UndoButtonView(false);
    }

    public Tile GetTileWithPosition(Vector2Int position)
    {
        if (mapTiles.TryGetValue(position, out TileData tileData))
        {
            return tileData.tile;
        }
        else return null;
    }

    private void FillStartGrid()
    {
        foreach (Vector2Int position in BreadPositions)
        {
            if (mapTiles.TryGetValue(position, out TileData tileData))
            {
                tileData.tile.AddBread();
                tileData.tile.RefreshIngredients();
            }
        }
        foreach (Vector2Int position in LettucePositions)
        {
            if (mapTiles.TryGetValue(position, out TileData tileData))
            {
                tileData.tile.AddLettuce();
                tileData.tile.RefreshIngredients();
            }
        }
        foreach (Vector2Int position in TomatoPositions)
        {
            if (mapTiles.TryGetValue(position, out TileData tileData))
            {
                tileData.tile.AddTomato();
                tileData.tile.RefreshIngredients();
            }
        }
        foreach (Vector2Int position in BaconPositions)
        {
            if (mapTiles.TryGetValue(position, out TileData tileData))
            {
                tileData.tile.AddBacon();
                tileData.tile.RefreshIngredients();
            }
        }
        foreach (Vector2Int position in CheddarPositions)
        {
            if (mapTiles.TryGetValue(position, out TileData tileData))
            {
                tileData.tile.AddCheddar();
                tileData.tile.RefreshIngredients();
            }
        }
    }

    #region GenerationGrid
    private void GenerateGrid()
    {
        for (int row = 0; row < maxRow; row++)
        {
            for (int column = 0; column < maxColumn; column++)
            {
                var tile = Instantiate(tilePrefab, GetWorld3DPosition(new Vector2Int(row, column)), Quaternion.identity, transform);
                tile.transform.localScale = gridData.cellSize;
                tile.Initialize(this, row, column, tile);
                tile.name = "Tile - (" + row.ToString() + " - " + column.ToString() + ")";
                mapTiles[new Vector2Int(row, column)] = tile.data;
            }
        }
        if (onGridGenerated != null) onGridGenerated(mapTiles);
        //CenterCamera();
    }

    #region GetWorld2DPosition
    public Vector2 GetWorld2DPosition(Vector2Int position)
    {
        return new Vector2(position.x * (gridData.cellSize.x + gridData.cellGap.x), position.y * (gridData.cellSize.z + gridData.cellGap.z));
    }

    public Vector2 GetWorld2DPosition(int x, int y)
    {
        return new Vector2(x * (gridData.cellSize.x + gridData.cellGap.x), y * (gridData.cellSize.z + gridData.cellGap.z));
    }
    #endregion

    #region GetWorld3DPosition
    public Vector3 GetWorld3DPosition(Vector2Int position)
    {
        return new Vector3(position.x * (gridData.cellSize.x + gridData.cellGap.x), 0, position.y * (gridData.cellSize.z + gridData.cellGap.z));
    }

    public Vector3 GetWorld3DPosition(int x, int y)
    {
        return new Vector3(x * (gridData.cellSize.x + gridData.cellGap.x), 0, y * (gridData.cellSize.z + gridData.cellGap.z));
    }
    #endregion

    public bool CheckGridBounds(Vector2Int coordinates)
    {
        return (coordinates.x < 0 || coordinates.x >= maxRow || coordinates.y < 0 || coordinates.y >= maxColumn);
    }

    //public bool CheckWalkable(Vector2Int coordinates)
    //{
    //    return mapTiles[coordinates].isFill;
    //}

    private void CenterCamera()
    {
        Vector3 startGrid = GetWorld3DPosition(0, 0);
        Vector3 endGrid = GetWorld3DPosition(maxRow - 1, maxColumn - 1);

        Camera.main.transform.position = new Vector3((startGrid.x + endGrid.x) / 2, HeightCamera(), (startGrid.z + endGrid.z) / 2);
    }

    private float HeightCamera()
    {
        return maxColumn * (gridData.cellGap.z + gridData.cellSize.z) + (gridData.cellGap.y + gridData.cellSize.y) + 1;
    }
    #endregion


    #region GenerateRandomRowAndColumn
    public void GenerateRowAndColumnRandom(out Vector2Int position)
    {
        int randomRow = Random.Range(0, maxRow);
        int randomColumn = Random.Range(0, maxColumn);
        position = new Vector2Int(randomRow, randomColumn);
    }

    public void GenerateRowAndColumnRandom(out int randomRow, out int randomColumn)
    {
        randomRow = Random.Range(0, maxRow);
        randomColumn = Random.Range(0, maxColumn);
    }
    #endregion

    public bool isSurroundedByFilledTile(Tile tile)
    {
        Vector2Int tileCoordinates = new Vector2Int(tile.data.row, tile.data.column);

        Vector2Int[] surroundOffsets = new Vector2Int[]
        {
            new Vector2Int(0, 1),//sopra
            new Vector2Int(1, 0),//destra
            new Vector2Int(0, -1),//sotto
            new Vector2Int(-1, 0)//sinistra
        };

        foreach (Vector2Int offset in surroundOffsets)
        {
            Vector2Int surroundCoords = tileCoordinates + offset;

            if (!CheckGridBounds(surroundCoords))
                continue;

            //if (mapTiles[surroundCoords].isFill)
            //    return true;
        }
        return false;
    }

    /// <summary>
    /// Mi salva in MissingFill tutti quelli vuoti a cui fare check
    /// </summary>
    /// <param name="tile"></param>
    //public void GetMissingFill(Tile tile)
    //{
    //    Vector2Int tileCoordinates = new Vector2Int(tile.data.row, tile.data.column);

    //    Vector2Int[] surroundOffsets = new Vector2Int[]
    //    {
    //    new Vector2Int(0, 1),
    //    new Vector2Int(1, 0),
    //    new Vector2Int(0, -1),
    //    new Vector2Int(-1, 0)
    //    };

    //    foreach (Vector2Int offset in surroundOffsets)
    //    {
    //        Vector2Int surroundCoords = tileCoordinates + offset;

    //        if (!CheckGridBounds(surroundCoords))
    //            continue;

    //        Tile surroundTile = mapTiles[surroundCoords].tile;

    //        if (surroundTile != null && surroundTile.isFill)
    //        {
    //            tile.GenerateLayer();
    //            return;
    //        }
            //else
            //{ if (!MissingFill.Contains(surroundTile)) MissingFill.Add(surroundTile); }
    //    }
    //}

    //Per me del futuro, usa come quello sopra, ma parti dal pane e aggiungi in maniera ricorsiva
    //tutti i tile combacianti a una lista controllando che non ci sia già per poi
    //eliminare nel game manager quelli non combacianti

    //public void GenerateLevel()
    //{
    //    foreach (var item in tileConfigs)
    //    {
    //        int row = item.row;
    //        int column = item.column;
    //        foreach (Tile tile in GameManager.instance.tilesList)
    //        {
    //            if (tile.data.row == row && tile.data.column == column)
    //            {
    //                item.tile = tile;
    //            }
    //        }
    //    }
    //    foreach (var item in tileConfigs)
    //    {
            
    //    }
    //}
}