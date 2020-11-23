using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO:
//  - dont check for bear
//  - dont double check false ones
//  - contact animation to indicate possible merge
//  + work with input
//  + work with visuals
//  - crystal upgrade mechanics
   
public class PlacementManager : MonoBehaviour
{
    public event System.Action<bool> TileGotPlaced = delegate { };
    public event System.Action<Tile.Type, int> TileGotSwaped = delegate { };

    // as i am making recreation of this game, i am just cutting these in stone
    private const int xAxis = 6;
    private const int yAxis = 6;

    public Tile[,] tile;
    bool[,] visited;
    bool tileGotPlaced = false;


   
    Tile lastHoveredTile;
    Tile currentPlaceableObject;

    [SerializeField] SpiteManager spiteManager;
    [SerializeField] InputManager inputManager;


    private void Awake()
    {
        tile = new Tile[xAxis, yAxis];
        visited = new bool[xAxis, yAxis];
        resetVisited();
        currentPlaceableObject = new Tile();
    }
    
    private void Start()
    {
        inputManager.OnPointerStay += HoveringOverTiles;
        inputManager.OnPointerRelease += SelectedTile;
        UpdateAllTiles();
    }


    void HoveringOverTiles(Tile hoveredTile) // get called every frame while mouse/ touch is down/active
    {
        if (lastHoveredTile == null || tileGotPlaced)
        {
            lastHoveredTile = hoveredTile;
            tileGotPlaced = false;
        }
        if (hoveredTile.isEmpty() && !(hoveredTile.coordinates.x == 0 && hoveredTile.coordinates.y == 0))
        {
            if (hoveredTile != lastHoveredTile && !(lastHoveredTile.coordinates.x == 0 && lastHoveredTile.coordinates.y == 0))
            {
                lastHoveredTile.ClearTile();
                PlaceObject(lastHoveredTile, lastHoveredTile);    // place empty object back
                lastHoveredTile = hoveredTile;
                PlaceObject(lastHoveredTile, currentPlaceableObject);   // place new object on new tile
                UpdateAllTiles();
            }
            else
            {
                PlaceObject(lastHoveredTile, currentPlaceableObject); // replace same object
            }
        }
    }

    void SelectedTile(Tile selectedTile) // gets called the frame when moese click or touch het realesed
    {
        if (selectedTile.coordinates.x == 0 && selectedTile.coordinates.y == 0) // bank part
        {
            Tile temp = new Tile();
            temp.tileType = tile[0, 0].tileType;
            temp.level = tile[0, 0].level;
            temp.tileName = tile[0, 0].tileName;
            if (tile[0, 0].tileType != Tile.Type.Empty)
            {
                // swap atributes
                PlaceObject(tile[0, 0], currentPlaceableObject, true);
                TileGotSwaped(temp.tileType, temp.level);
                //PlaceObject(currentPlaceableObject, temp, true);
            }
            else
            {
                PlaceObject(tile[0, 0], currentPlaceableObject, true);
                tileGotPlaced = true;
                TileGotPlaced(false);
            }
        }
        if (selectedTile.isEmpty() && !(selectedTile.coordinates.x == 0 && selectedTile.coordinates.y == 0))// removed bank tile
        {
            
                
            tileGotPlaced = true;

            PlaceObject(selectedTile, currentPlaceableObject, true); // replace same object

            if (selectedTile.tileType != Tile.Type.Bear)
                TileGotPlaced(CheckForUpgrades(selectedTile)); // event header to let gameManager know that tile got placed also sending if upgrade is in order
            else
                TileGotPlaced(false);
            resetVisited();
        }
        UpdateAllTiles();
 
    }

    public bool CheckForUpgrades(Tile tileToCheck)
    {
        int amount = checkForSameAndMark(tileToCheck);
        if (amount > 2)
        {
            Upgrade(getAllUpgradeTiles(amount), tileToCheck);
            return true;
        }
        return false;
    }

    Tile[] getAllUpgradeTiles(int amount)
    {
        Tile[] tiles = new Tile[amount];
        int temp = 0;
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                if(!(i == 0 && j ==0) && visited[i,j])
                {
                    tiles[temp] = tile[i, j];
                    temp++;
                }
            }
        }
        return tiles;
    }

    void Upgrade(Tile[] allTiles, Tile location)
    {
        foreach (Tile t in allTiles)
        {
            if (t != location)
            {
                t.ClearTile();
            }
        }
        location.Upgrade();
        if(allTiles.Length > 3)
        {
            location.specialVersion = true;
        }
        else
        {
            location.specialVersion = false;
        }
        resetVisited();
        CheckForUpgrades(location);
    }

    void UpdateAllTiles()
    {
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                PlaceObject(tile[i, j], tile[i, j]); // just replace visual based on infomation
            }
        }
    }

    // get amount and changes visited array of all the tile that are the same type and are adjust to each other
    int checkForSameAndMark(Tile centerTile)
    {
        
        int sameAmount = 0;

        #region check side tile and visit
        //left

        if (centerTile.coordinates.x > 0) //if not on a border
            if (!visited[centerTile.coordinates.x - 1, centerTile.coordinates.y] && tile[centerTile.coordinates.x - 1, centerTile.coordinates.y].tileType == centerTile.tileType)
            {
                
                if (tile[centerTile.coordinates.x - 1, centerTile.coordinates.y].level == centerTile.level)
                {
                    visited[centerTile.coordinates.x - 1, centerTile.coordinates.y] = true;
                    sameAmount += 1 + checkForSameAndMark(tile[centerTile.coordinates.x - 1, centerTile.coordinates.y]);

                }
            }

        //right
        if (centerTile.coordinates.x < xAxis - 1) //if not on a border
            if (!visited[centerTile.coordinates.x + 1, centerTile.coordinates.y] && tile[centerTile.coordinates.x + 1, centerTile.coordinates.y].tileType == centerTile.tileType)
            {
                if (tile[centerTile.coordinates.x + 1, centerTile.coordinates.y].level == centerTile.level)
                {
                    visited[centerTile.coordinates.x + 1, centerTile.coordinates.y] = true;
                    sameAmount += 1 + checkForSameAndMark(tile[centerTile.coordinates.x + 1, centerTile.coordinates.y]);

                }
            }
        
        //top
        if (centerTile.coordinates.y > 0) //if not on a border
            if (!visited[centerTile.coordinates.x, centerTile.coordinates.y - 1] && tile[centerTile.coordinates.x, centerTile.coordinates.y - 1].tileType == centerTile.tileType)
            {
                if (tile[centerTile.coordinates.x, centerTile.coordinates.y - 1].level == centerTile.level)
                {
                    visited[centerTile.coordinates.x, centerTile.coordinates.y - 1] = true;
                    sameAmount += 1 + checkForSameAndMark(tile[centerTile.coordinates.x, centerTile.coordinates.y - 1]);

                }
            }

        //bottom
        if (centerTile.coordinates.y < yAxis - 1) //if not on a border
            if (!visited[centerTile.coordinates.x, centerTile.coordinates.y + 1] && tile[centerTile.coordinates.x, centerTile.coordinates.y + 1].tileType == centerTile.tileType)
            {
                if (tile[centerTile.coordinates.x, centerTile.coordinates.y + 1].level == centerTile.level)
                {
                    visited[centerTile.coordinates.x, centerTile.coordinates.y + 1] = true;
                    sameAmount += 1 + checkForSameAndMark(tile[centerTile.coordinates.x, centerTile.coordinates.y + 1]);
                   
                }
            }                             

        #endregion

        // maked tile as visited and and call parent method to look for the same type of tiles nearby

        return sameAmount;
    }


    void resetVisited()
    {
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                visited[i, j] = false;
            }
        }
        visited[0, 0] = true; // so code doesnt check bank
    }


    public void SetNextTile(Tile.Type furuteType, int futureLevel)
    {
        currentPlaceableObject.tileType = furuteType;
        currentPlaceableObject.level = futureLevel;
        spiteManager.UpdateInfo(furuteType, futureLevel);
    }

    void PlaceObject(Tile where, Tile what, bool leave = false)
    {
        spiteManager.ConnectDataToVisuals(what.tileType, what.level);
        if (leave)
        {
            tile[where.coordinates.x,where.coordinates.y].level = what.level;
            tile[where.coordinates.x, where.coordinates.y].tileType = what.tileType;
            tile[where.coordinates.x, where.coordinates.y].tileName = what.tileName;
        }
        // destroy all children and attach new one 
        if(where.transform)                                                             
            foreach (Transform child in where.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        Instantiate(spiteManager.requestedObject, where.transform);
    }

    public void GenerateMap()
    {

        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                if ((int)Random.Range(0f, 35f) % 4 == 0) // 
                {
                    PlaceObject(tile[i, j], currentPlaceableObject, true);
                    TileGotPlaced(false);
                }
            }
        }
        UpdateAllTiles();
    }

    
}
