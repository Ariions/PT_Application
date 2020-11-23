using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TileParser : MonoBehaviour
{
    /// <summary>
    /// File to be as a reference to all the tiles
    /// </summary>

    public event System.Action<int, int> TileUpdated = delegate { };

    public Transform[] TileTransforms;
    private const int xAxis = 6;
    private const int yAxis = 6;
    public Tile[,] tile;

    public static int XAxis => xAxis;
    public static int YAxis => yAxis;

    // Start is called before the first frame update
    void Awake()
    {
        tile = new Tile[XAxis, YAxis];
        ParseTiles();
    }

    public void ParseTiles()
    {
        int count = 0;
        for (int i = 0; i < XAxis; i++)
        {
            for (int j = 0; j < YAxis; j++)
            {
                tile[j, i] = TileTransforms[count].gameObject.GetComponent<Tile>();
                tile[j, i].coordinates.x = j;
                tile[j, i].coordinates.y = i;
                count++;
            }
        }
    }
 
    public Tile[,] GetTiles()
    {
        return tile;
    }

    public Tile GetTile(int x, int y)
    {
        return tile[x, y];
    }

    public void SetTile(int x, int y, Tile.Type type, int level, string tileName, bool isSpecial = false)
    {
        tile[x, y].tileType = type;
        tile[x, y].level = level;
        tile[x, y].tileName = tileName;
        tile[x, y].specialVersion = isSpecial;
        TileUpdated(x, y);
    }

}
