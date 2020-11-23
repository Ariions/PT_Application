using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum Type
    {
        Empty,
        Holy,
        Crystal,
        Wood,
        Bear,
        Box,
        Robot
    }

    public struct Coordinates
    {
        public int x;
        public int y;
    }

    public int score;
    public Coordinates coordinates;
    public string tileName;
    public bool specialVersion = false;
    public int level = 0;   // building level that is on top of this tile                    
    public Type tileType = Type.Empty;


    // clear any buildings from the tile and sets building level to 0
    public void ClearTile() 
    {
        tileType = Type.Empty;
        level = 0;
    }

    public void Upgrade() // upgrade and upgrade rules
    {
        if(tileType == Type.Wood && level < 4)
        {
            level++;
        }
        if (tileType == Type.Holy)
        {
            if (level < 2)
            {
                level++;
            }
            else
            {
                tileType = Type.Box;
                level = 0;
            }
        }
        if (tileType == Type.Bear)
        {
            tileType = Type.Holy;
            level = 0;
        }
        if (tileType == Type.Crystal && level >= 1)
        {
            if (level < 1)
            {
                level++;
            }
            else
            {
                tileType = Type.Box;
                level = 0;
            }
        }
        if (tileType == Type.Box && level <1)
        {
            tileType = Type.Box;
            level++;
        }
    }

    public bool isEmpty()
    {
        if(tileType == Type.Empty)
            return  true;
        return false;
    }
}