using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData data;
    public GameObject Bread, Lettuce, Tomato, Bacon, Cheddar;
    public List<GameObject> layers;
    public bool isFill
    {
        get { return layers.Count > 0; }
    }


    public void Initialize(GridManager gridM, int rowInit, int columnInit, Tile tile)
    {
        data = new TileData(gridM, rowInit, columnInit, tile);
    }

    public void RefreshIngredients()
    {
        if (layers == null || layers.Count == 0) 
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            return;
        }
        Vector3 StartPosition = new Vector3 (transform.position.x, transform.position.y + 0.6f, transform.position.z);
        Vector3 StackPosition = new Vector3 (0f,0.2f,0f);
        Vector3 NewPosition = StartPosition;
        for (int i = 0; i < layers.Count; i++)
        {
            Instantiate(layers[i], NewPosition, Quaternion.identity, transform);
            NewPosition += StackPosition;
        }
    }

    public void TransferIngredients(Tile destinationTile)
    {
        GameManager.instance.saveMove.tileOrigin = this;
        GameManager.instance.saveMove.tileDestination = destinationTile;
        GameManager.instance.saveMove.ingredientsDestination = new List<GameObject>(destinationTile.layers);
        GameManager.instance.saveMove.ingredientsOrigin = new List<GameObject>(layers);
        GameManager.instance.uiManager.UndoButtonView(true);
        for (int i = layers.Count - 1; i >= 0; i--)
        {
            destinationTile.layers.Add(layers[i]);
        }
        layers.Clear();
        RefreshIngredients();
        destinationTile.RefreshIngredients();
        if (GameManager.instance.isLevelFinish) GameManager.instance.uiManager.UIWin();
    }

    public void AddBread()
    {
        layers.Add(Bread); 
    }
    public void AddLettuce()
    {
        layers.Add(Lettuce);
    }
    public void AddTomato()
    {
        layers.Add(Tomato);
    }
    public void AddBacon()
    {
        layers.Add(Bacon);
    }
    public void AddCheddar()
    {
        layers.Add(Cheddar);
    }

    //Per l'autogenerazione (WIP)
    //public void GenerateLayer()
    //{
    //    switch (Mathf.FloorToInt(Random.value * 4))
    //    {
    //        case 0:
    //            Debug.Log("Lattuga");
    //            break;
    //        case 1:
    //            Debug.Log("Pomodoro");
    //            break;
    //        case 2:
    //            Debug.Log("Bacon");
    //            break;
    //        case 3:
    //            Debug.Log("Cheddar");
    //            break;
    //        default:
    //            Debug.Log("Lattuga");
    //            break;
    //    }
    //}
}