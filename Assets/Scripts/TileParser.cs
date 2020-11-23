using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileParser : MonoBehaviour
{
    public Transform[] TileTransforms;
    private PlacementManager placementManager;
    private const int xAxis = 6;
    private const int yAxis = 6;

    // Start is called before the first frame update
    void Start()
    {
        placementManager = GetComponent<PlacementManager>();
        ParseTiles();
    }

    public void ParseTiles()
    {
        int count = 0;
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                placementManager.tile[j, i] = TileTransforms[count].gameObject.GetComponent<Tile>();
                placementManager.tile[j, i].coordinates.x = j;
                placementManager.tile[j, i].coordinates.y = i;
                count++;
            }
        }
    }
}
