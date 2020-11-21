using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:
//  - dont check for bear
//  - dont double check false ones
//  - contact animation to indicate possible merge
//  - work with input
//  - work with visuals
//  - crystal upgrade mechanics
   
public class PlacementManager : MonoBehaviour
{

    // as i am making recreation of this game, i am just cutting these in stone
    private const int xAxis = 6;
    private const int yAxis = 6;

    Tile[][] tile;
    bool[][] visited;

    Tile currentPlaceableObject;

    private int TotalSame = 0;

    private void Start()
    {
        // setting values for visited array
        
    }

    public void SelectedTile(Tile tileToCheck)
    {
        if (!tileToCheck.isEmpty())
        {
            
        }
    }

    public bool CheckForUpgrades(Tile tileToCheck)
    {
        if (checkForSameAndMark(tileToCheck, currentPlaceableObject) > 2)
        {
            return true;
        }

        return false;
    }

    public void CreateReference(Tile tileref)
    {
        tile[tileref.coordinates.x][tileref.coordinates.y] = tileref;
    }

    void placeTile(Tile.Coordinates coordinates, Tile.Type tileType)
    {
        tile[coordinates.x][coordinates.y].tileType = tileType;
    }

    void UpgradeTile(Tile upgradableTile, Tile[] sacrificedTiles)
    {

        foreach(Tile t in sacrificedTiles)
        {
            t.ClearTile();
        }

        upgradableTile.Upgrade();
    }

    // check if it is possible to upgrade current tile then returns all of the tiles that are going to be consumed for an upgrade
    void upgradbleTiles(Tile upgradableTile) 
    {
        // if(upgradableTile)
      
     


    }


    // get amount and changes visited array of all the tile that are the same type and are adjust to each other
    int checkForSameAndMark(Tile centerTile, Tile CompareToTile)
    {
        int sameAmount = 0;
        #region check side tile and visit
        //left
        if (centerTile.coordinates.x > 0) //if not on a border
            if (!visited[centerTile.coordinates.x - 1][centerTile.coordinates.y] && tile[centerTile.coordinates.x - 1][centerTile.coordinates.y].tileType == CompareToTile.tileType)
            {
                if (tile[centerTile.coordinates.x][centerTile.coordinates.y + 1].level == CompareToTile.level)
                    Visit(tile[centerTile.coordinates.x - 1][centerTile.coordinates.y]);
            }
        
        //right
        if (centerTile.coordinates.x < xAxis - 1) //if not on a border
            if (!visited[centerTile.coordinates.x + 1][centerTile.coordinates.y] && tile[centerTile.coordinates.x + 1][centerTile.coordinates.y].tileType == CompareToTile.tileType)
            {
                if (tile[centerTile.coordinates.x][centerTile.coordinates.y + 1].level == CompareToTile.level)
                    Visit(tile[centerTile.coordinates.x + 1][centerTile.coordinates.y]);
            }
        
        //top
        if (centerTile.coordinates.y > 0) //if not on a border
            if (!visited[centerTile.coordinates.x][centerTile.coordinates.y - 1] && tile[centerTile.coordinates.x][centerTile.coordinates.y - 1].tileType == CompareToTile.tileType)
            {
                if (tile[centerTile.coordinates.x][centerTile.coordinates.y + 1].level == CompareToTile.level)
                    Visit(tile[centerTile.coordinates.x][centerTile.coordinates.y - 1]);
            }

        //bottom
        if (centerTile.coordinates.y < yAxis - 1) //if not on a border
            if (!visited[centerTile.coordinates.x][centerTile.coordinates.y + 1] && tile[centerTile.coordinates.x][centerTile.coordinates.y + 1].tileType == CompareToTile.tileType)
            {
                if(tile[centerTile.coordinates.x][centerTile.coordinates.y + 1].level == CompareToTile.level)
                    Visit(tile[centerTile.coordinates.x][centerTile.coordinates.y + 1]);
            }
        #endregion

        // maked tile as visited and and call parent method to look for the same type of tiles nearby
        void Visit(Tile tileTovisit)
        {
            visited[tileTovisit.coordinates.x][tileTovisit.coordinates.y] = true; // mark as visited TODO: make so false tiles are marked as visited also
            sameAmount = 1 + checkForSameAndMark(tile[tileTovisit.coordinates.x][tileTovisit.coordinates.y], CompareToTile); // 1 (this tile) + other adjacent tiles
        }
        return sameAmount;
    }

    void resetVisited()
    {
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                visited[i][j] = false;
            }
        }
    }

    Tile UpgradesInto(Tile CheckedTile, bool fourOrMore = false)
    {
        Tile AnswerTileClone = new Tile();





        return AnswerTileClone;
    }

}
