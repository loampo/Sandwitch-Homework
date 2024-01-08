using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveMove
{
    public Tile tileOrigin, tileDestination;
    public List<GameObject> ingredientsOrigin;
    public List<GameObject> ingredientsDestination;
}
public class GameManager : Singleton<GameManager>
{
    public GridManager gridManager;
    public UIManager uiManager;
    public bool isLevelFinish => gridManager.CheckFinishLevel();
    public Tile SelectedTile;
    public SaveMove saveMove;

    private void Awake()
    {
        gridManager = GetComponent<GridManager>();
        uiManager = GetComponent<UIManager>();
    }
    protected override void Start()
    {
        base.Start();
    }

    //void CreateLevel()
    //{
    //    int Bread = 0;
    //    while (Bread < 2)
    //    {
    //        foreach (Tile tile in tilesList)
    //        {
    //            if (Random.value < 0.2f && Bread < 2)
    //            {
    //                tile.GenerateBread();
    //                Bread++;
    //            }
    //        }
    //    }
    //    int Ingregients = 0;
    //    while (Ingregients < 2)
    //    {
    //        foreach (Tile tile in tilesList)
    //        {
    //            if (!tile.data.isFill && Random.value < 0.5f)
    //            {
    //                tile.GenerateLayer();
    //                Ingregients++;
    //            }
    //        }
    //    }
    //    while (isLevelCreateFinish)
    //    {
    //        List<Tile> FilledTileError = new List<Tile>();
    //        while (FilledTileError.Count == 0)
    //        {
    //            foreach (Tile tile in tilesList)
    //            {
    //                if (tile.data.isFill && !gridManager.isSurroundedByFilledTile(tile))
    //                {
    //                    FilledTileError.Add(tile);
    //                }
    //            }
    //            foreach (Tile tile in FilledTileError)
    //            {
    //                gridManager.GetMissingFill(tile);
    //            }
    //            if (gridManager.MissingFill.Count > 0)
    //            {
    //                foreach (Tile tile in gridManager.MissingFill)
    //                {
    //                    gridManager.GetMissingFill(tile);
    //                }
    //                gridManager.MissingFill.Clear();
    //                FilledTileError.Clear();
    //                foreach (Tile tile in tilesList)
    //                {
    //                    if (tile.data.isFill && !gridManager.isSurroundedByFilledTile(tile))
    //                    {
    //                        FilledTileError.Add(tile);
    //                    }
    //                }
    //            }
    //            else isLevelCreateFinish = true;
    //        }
    //    }
    //}
}