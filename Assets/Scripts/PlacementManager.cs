using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public struct Coordinates
    {
        public int x;
        public int y;
    }

    private int xAxis = 6;
    private int yAxis = 6;

    Tile[][] tile;

    // Start is called before the first frame update
    void Start()
    {
        // inicialize all tiles and save their references
        for(int i = 0; i < xAxis; i++)
        {
            for(int j = 0; j < yAxis; j++)
            {
                tile[i][j] = new Tile();
                tile[i][j].tileCoordinates.x = i;
                tile[i][j].tileCoordinates.y = j;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void placeTile(Coordinates spot, Tile.Type _type)
    {
        tile[spot.x][spot.y].tileType = _type;
    }



    void UpgradeTile(Tile upgradedTile, Tile[] sacrificedTiles)
    {

        foreach(Tile t in sacrificedTiles)
        {
            t.ClearTile();
        }

        upgradedTile.Upgrade();
    }

    // check if it is possible to upgrade current tile then returns all of the tiles that are going to be consumed for an upgrade
    Tile[] upgradbleTiles(Coordinates spot, Tile.Type _type) 
    {
        


        return null;
    }

    void Dig(Tile.Type _type)
    {

    }

}
