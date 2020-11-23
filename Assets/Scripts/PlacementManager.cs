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

[RequireComponent(typeof(TileParser))]
public class PlacementManager : MonoBehaviour
{
    public event System.Action OnTilePlace = delegate { };

    bool[,] visited;
    bool tileGotPlaced = false;

    [SerializeField]
    Tile lastHoveredTile;
    [SerializeField]
    Tile currentPlaceableObject;

    [SerializeField] SpiteManager spiteManager;
    [SerializeField] InputManager inputManager;
    TileParser tileParser;

    int xAxis = TileParser.XAxis;
    int yAxis = TileParser.YAxis;

    private void Awake()
    {

        tileParser = GetComponent<TileParser>();
        visited = new bool[xAxis, yAxis];
        resetVisited();
        currentPlaceableObject = new Tile();
    }
    
    private void Start()
    {
        inputManager.OnPointerStay += HoveringOverTiles;
        inputManager.OnPointerRelease += SelectedTile;

    }


    void HoveringOverTiles(Tile hoveredTile) // get called every frame while mouse/ touch is down/active
    {
        if (lastHoveredTile == null || tileGotPlaced)
        {
            lastHoveredTile = hoveredTile;
            tileGotPlaced = false;
        }
        if (hoveredTile.isEmpty() && !(hoveredTile.coordinates.x == 0 && hoveredTile.coordinates.y == 0) && currentPlaceableObject.tileType != Tile.Type.Robot)
        {
            if (hoveredTile != lastHoveredTile && !(lastHoveredTile.coordinates.x == 0 && lastHoveredTile.coordinates.y == 0))
            {
                lastHoveredTile.ClearTile();
                spiteManager.UpdateTile(lastHoveredTile.coordinates.x, lastHoveredTile.coordinates.y);

                lastHoveredTile = hoveredTile;
                PlaceObject(lastHoveredTile, currentPlaceableObject);   // place new object on new tile
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
            temp.tileType = tileParser.tile[0, 0].tileType;
            temp.level = tileParser.tile[0, 0].level;
            temp.tileName = tileParser.tile[0, 0].tileName;
            if (tileParser.tile[0, 0].tileType != Tile.Type.Empty)
            {
                // swap atributes
                PlaceObject(tileParser.tile[0, 0], currentPlaceableObject, true);
                currentPlaceableObject.tileType = temp.tileType;
                currentPlaceableObject.level = temp.level;
                currentPlaceableObject.tileName = temp.tileName;
                spiteManager.UpdateUI(temp);
            }
            else
            {
                PlaceObject(tileParser.tile[0, 0], currentPlaceableObject, true);
                tileGotPlaced = true;
                OnTilePlace();
            }

        }
        if (currentPlaceableObject.tileType != Tile.Type.Robot) {
            if (selectedTile.isEmpty() && !(selectedTile.coordinates.x == 0 && selectedTile.coordinates.y == 0))// removed bank tile
        {
                tileGotPlaced = true;
                PlaceObject(selectedTile, currentPlaceableObject, true); // replace same object


                if (selectedTile.tileType != Tile.Type.Bear)
                    CheckForUpgrades(selectedTile); // event header to let gameManager know that tile got placed also sending if upgrade is in order
                resetVisited();


                OnTilePlace();
            }
        }else if(!selectedTile.isEmpty() && !(selectedTile.coordinates.x == 0 && selectedTile.coordinates.y == 0))
        {
            if (selectedTile.tileType != Tile.Type.Bear)
            {
                selectedTile.ClearTile();
            }
            else
            {
                selectedTile.Upgrade(); 
                CheckForUpgrades(selectedTile); // event header to let gameManager know that tile got placed also sending if upgrade is in order
                resetVisited();
            }
            tileGotPlaced = true;
            spiteManager.UpdateTile(selectedTile.coordinates.x, selectedTile.coordinates.y);
            OnTilePlace();
        }
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
        Tile[] tilesAr = new Tile[amount];
        int temp = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if(!(i == 0 && j ==0) && visited[i,j])
                {
                    tilesAr[temp] = tileParser.tile[i, j];
                    temp++;
                }
            }
        }
        return tilesAr;
    }

    void Upgrade(Tile[] allTiles, Tile location)
    {
        foreach (Tile t in allTiles)
        {
            if (t != location)
            {
                t.ClearTile();
                spiteManager.UpdateTile(t.coordinates.x, t.coordinates.y);
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
        spiteManager.UpdateTile(location.coordinates.x, location.coordinates.y);
        resetVisited();
        CheckForUpgrades(location);
    }


    // get amount and changes visited array of all the tile that are the same type and are adjust to each other
    int checkForSameAndMark(Tile centerTile)
    {
        
        int sameAmount = 0;
        int x = centerTile.coordinates.x;
        int y = centerTile.coordinates.y;

        #region check side tile and visit
        //left

        if (x > 0) //if not on a border
            if (!visited[x - 1, y] && tileParser.tile[x - 1, y].tileType == centerTile.tileType)
            {
                
                if (tileParser.tile[x - 1, y].level == centerTile.level)
                {
                    visited[x - 1, y] = true;
                    sameAmount += 1 + checkForSameAndMark(tileParser.tile[x - 1, y]);
                   
                }
            }

        //right
        if (x < xAxis - 1) //if not on a border
            if (!visited[x + 1, y] && tileParser.tile[x + 1, y].tileType == centerTile.tileType)
            {
                if (tileParser.tile[x + 1, y].level == centerTile.level)
                {
                    visited[x + 1, y] = true;
                    sameAmount += 1 + checkForSameAndMark(tileParser.tile[x + 1, y]);

                }
            }
        
        //top
        if (y > 0) //if not on a border
            if (!visited[x, y - 1] && tileParser.tile[x, y - 1].tileType == centerTile.tileType)
            {
                if (tileParser.tile[x, y - 1].level == centerTile.level)
                {
                    visited[x, y - 1] = true;
                    sameAmount += 1 + checkForSameAndMark(tileParser.tile[x, y - 1]);

                }
            }

        //bottom
        if (y < yAxis - 1) //if not on a border
            if (!visited[x, y + 1] && tileParser.tile[x, y + 1].tileType == centerTile.tileType)
            {
                if (tileParser.tile[x, y + 1].level == centerTile.level)
                {
                    visited[x, y + 1] = true;
                    sameAmount += 1 + checkForSameAndMark(tileParser.tile[x, y + 1]);
                   
                }
            }                             

        #endregion

        // maked tile as visited and and call parent method to look for the same type of tiles nearby

        return sameAmount;
    }


    public void resetVisited()
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
        spiteManager.ForceUpdateAll();
        currentPlaceableObject.tileType = furuteType;
        currentPlaceableObject.level = futureLevel;
        spiteManager.UpdateUI(currentPlaceableObject);
    }




    void PlaceObject(Tile where, Tile what, bool leave = false)
    {
        if (leave)
            tileParser.SetTile(where.coordinates.x, where.coordinates.y, what.tileType, what.level, what.tileName);
        else
            spiteManager.SimulateFake(where.coordinates.x, where.coordinates.y, what);
    }


    public void GenerateMap()
    {
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    if ((int)Random.Range(0f, 35f) % 4 == 0) // 
                    {
                        if (currentPlaceableObject.tileType == Tile.Type.Robot)
                            currentPlaceableObject.tileType = Tile.Type.Wood;
                        PlaceObject(tileParser.tile[i, j], currentPlaceableObject, true);
                        OnTilePlace();
                    }
                    else
                        tileParser.tile[i, j].ClearTile();
                }
            }
        }
        tileParser.tile[0, 0].ClearTile();
        spiteManager.ForceUpdateAll();
    }

    public bool checkIfLost()
    {
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                if (tileParser.tile[i, j].tileType == Tile.Type.Empty) // if there is a spot you cannot lose
                    return false;
            }
        }
        return true;
    }
}
