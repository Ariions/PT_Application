using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move dictionary to file

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


    public Coordinates coordinates;

    public string tileName;
    public bool specialVersion = false;
    public int level = 0;   // building level that is on top of this tile                    
    public Type tileType = Type.Empty;


    private void Start()
    {
       // tileName = "Empty";
    }

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
    ///test 
}