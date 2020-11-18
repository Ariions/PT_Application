using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public struct Coordinates
    {
        public int x;
        public int y;
    }

    public enum Type
    {
        Empty,
        Holy,
        Crystal,
        Wood,
        Bear
    }

    public int level = 0;                       // building level that is on top of this tile
    public Type tileType = Type.Empty;          
    public Coordinates tileCoordinates;

    // clear any buildings from the tile and sets building level to 0
    public void ClearTile() 
    {
        tileType = Type.Empty;
        level = 0;
    }

    public void Upgrade()
    {
        level++;
    }

    public bool isEmpty()
    {
        if(tileType == Type.Empty)
            return  true;

        return false;
    }
}