using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearAI : MonoBehaviour
{
    [SerializeField]
    PlacementManager placementManager;
    [SerializeField]
    TileParser tileParser;
    bool[,] visited;
    bool[,] blockedBears;
    Tile[] bears;
    int currentAmount = 0;


    int xAxis = TileParser.XAxis;
    int yAxis = TileParser.YAxis;

    private void Start()
    {
        bears = new Tile[xAxis * yAxis];
        visited = new bool[xAxis, yAxis];
        blockedBears = new bool[xAxis, yAxis];
        resetArrays();
    }

    void resetArrays()
    {
        for(int i =0; i < xAxis; i ++)
        {
            for (int j = 0; j < xAxis; j++)
            {
                visited[i, j] = false;
                blockedBears[i, j] = true;
            }
        }
    }

    public void UpdateBears()
    {
        bears = GetAllBears();
        if (currentAmount > 0)
        {
            blockedBears = CheckForBlockedBears();
            for (int i = 0; i < currentAmount; i++)
            {
                if (!(bears[i].coordinates.x == 0 && bears[i].coordinates.y == 0 ))
                {
                    if (!blockedBears[bears[i].coordinates.x, bears[i].coordinates.y])
                    {
                        tileParser.tile[bears[i].coordinates.x, bears[i].coordinates.y].Upgrade();
                        placementManager.CheckForUpgrades(tileParser.tile[bears[i].coordinates.x, bears[i].coordinates.y]);
                        placementManager.resetVisited();
                    }
                    else
                    {
                        TryMoveBear(bears[i]);
                    }
                }
            }
        }
        resetArrays();
    }

    bool[,] CheckForBlockedBears()
    {
        bool[,] temp;
        temp = new bool[xAxis, yAxis];
        for(int i = 0; i < currentAmount; i++) { 
                int x = bears[i].coordinates.x;
                int y = bears[i].coordinates.y;
                temp[x, y] = !checkIfGroupIsBlocked(bears[i]);
        }
        return temp;
    }

    void TryMoveBear(Tile bears)
    {
        int x = bears.coordinates.x;
        int y = bears.coordinates.y;
        bool moved = false;

        //left
        if (x > 0) //if not on a border
            if (tileParser.tile[x - 1, y].tileType == Tile.Type.Empty && !(x - 1 == 0 && y == 0))
            {
                if (!moved)
                {
                    moved = true;
                    Move(tileParser.tile[x, y], tileParser.tile[x - 1, y]);
                }
            }

        //right
        if (x < xAxis - 1) //if not on a border
            if (tileParser.tile[x + 1, y].tileType == Tile.Type.Empty)
            {
                if (!moved)
                {
                    moved = true;
                    Move(tileParser.tile[x, y], tileParser.tile[x + 1, y]);
                }
            }

        //top
        if (y > 0) //if not on a border
            if (tileParser.tile[x, y - 1].tileType == Tile.Type.Empty && !(x == 0 && y - 1 == 0))
            {
                if (!moved)
                {
                    moved = true;
                    Move(tileParser.tile[x, y], tileParser.tile[x, y -1]);
                }
            }

        //bottom
        if (y < yAxis - 1) //if not on a border
            if (tileParser.tile[x, y + 1].tileType == Tile.Type.Empty)
            {
                if (!moved)
                {
                    moved = true;
                    Move(tileParser.tile[x, y], tileParser.tile[x, y+1]);
                }
            }
    }

    void Move(Tile from, Tile to)
    {
        tileParser.SetTile(to.coordinates.x, to.coordinates.y, from.tileType, from.level, from.tileName);
        tileParser.tile[from.coordinates.x, from.coordinates.y].ClearTile();
    }

    Tile[] GetAllBears()
    {
        currentAmount = 0;
        Tile[] temp = new Tile[xAxis * yAxis];
        foreach(Tile t in tileParser.tile)
        {
            if (t.tileType == Tile.Type.Bear)
            {
                temp[currentAmount] = t;
                currentAmount++;
            }
        }
        return temp;
    }


    bool checkIfGroupIsBlocked(Tile centerTile) // i even one bear in a group is not blocked mark as not blocked
    {
        bool blocked = true;
        #region check side tile and visit
        //left
        int x = centerTile.coordinates.x;
        int y = centerTile.coordinates.y;

        if (x > 0) //if not on a border
        {
            if (!visited[x - 1, y])
            {
                if (tileParser.tile[x - 1, y].tileType == Tile.Type.Bear)
                {
                    visited[x - 1, y] = true;
                    if (blocked || checkIfGroupIsBlocked(tileParser.tile[x - 1, y]))
                    {
                        blocked ^= true;
                        blockedBears[x - 1, y] = true;
                    }
                }
                if (tileParser.tile[x - 1, y].tileType == Tile.Type.Empty && !(x-1 == 0 && y == 0))
                {
                    blockedBears[x - 1, y] = false;
                    return false;
                }
                else
                {
                    blocked ^= true;
                    blockedBears[x - 1, y] = true;
                }
            }
        }
        else
        {
            blocked ^= true;
        }
        //right
        if (x < xAxis - 1) //if not on a border
        {
            if (!visited[x + 1, y])
            {
                if (tileParser.tile[x + 1, y].tileType == Tile.Type.Bear)
                {
                    visited[x + 1, y] = true;
                    if (blocked && checkIfGroupIsBlocked(tileParser.tile[x + 1, y]))
                    {
                        blocked = true;
                        blockedBears[x + 1, y] = true;
                    }
                }
                if (tileParser.tile[x + 1, y].tileType == Tile.Type.Empty)
                {
                    blockedBears[x + 1, y] = false;
                    return false;
                }
                else
                {
                    blocked ^= true;
                    blockedBears[x + 1, y] = true;
                }
            }
        }
        else
        {
            blocked ^= true;
        }
        //top
        if (y > 0) //if not on a border
        {
            if (!visited[x, y - 1])
            {
                if (tileParser.tile[x, y - 1].tileType == Tile.Type.Bear)
                {
                    visited[x, y - 1] = true;
                    if (blocked && checkIfGroupIsBlocked(tileParser.tile[x, y - 1]))
                    {
                        blocked = true;
                        blockedBears[x, y - 1] = true;
                    }
                }
                if (tileParser.tile[x, y - 1].tileType == Tile.Type.Empty && !(x == 0 && y - 1 == 0))
                {
                    blockedBears[x, y - 1] = false;
                    return false;
                }
                else
                {
                    blocked ^= true;
                    blockedBears[x, y - 1] = true;
                }
            }
        }
        else
        {
            blocked ^= true;
        }
        //bottom
        if (y < yAxis - 1) //if not on a border
        {
            if (!visited[x, y + 1])
            {
                if (tileParser.tile[x, y + 1].tileType == Tile.Type.Bear)
                {
                    visited[x, y + 1] = true;
                    if (blocked && checkIfGroupIsBlocked(tileParser.tile[x, y + 1]))
                    {
                        blocked = true;
                        blockedBears[x, y + 1] = true;
                    }
                }
                if (tileParser.tile[x, y + 1].tileType == Tile.Type.Empty)
                {
                    blockedBears[x, y + 1] = false;
                    return false;
                }
                else
                {
                    blocked ^= true;
                    blockedBears[x, y + 1] = true;
                }
            }
        }
        else
        {
            blocked ^= true;
        }

        #endregion

        // maked tile as visited and and call parent method to look for the same type of tiles nearby

        return blocked;
    }


}
