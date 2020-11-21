using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Move dictionary to file

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
        Bear,
        Box,
        Robot
    }

    string[][] namesDictionary; // move to JsonFIle if you have time

    public string name = "Empty";

    bool specialVersion = false;
    public int level = 0;   // building level that is on top of this tile                    
    public Type tileType = Type.Empty;          
    public Coordinates coordinates;

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

    void updateInformation()
    {
        // update names bbased on level and fix levels if needed (is object cannot have other levels)
        switch (tileType)
        {
            case Type.Wood:
                switch (level)
                {
                    case 0:
                        name = "Grass";
                        break;
                    case 1:
                        name = "Bush";
                        break;
                    case 2:
                        name = "Tree";
                        break;
                    case 3:
                        name = "Hut";
                        break;
                    case 4:
                        name = "House";
                        break;
                    case 5:
                        name = "Castle";
                        break;
                    case 6:
                        name = "Floating House";
                        break;
                    case 7:
                        name = "Floating Castle";
                        break;
                    default:
                        Debug.LogError("Tile :" + coordinates.x + "," + coordinates.y + " level was incorrect");
                    break;
                }
                break;
            case Type.Empty:
                name = "Empty";
                break;
            case Type.Holy:
                switch (level)
                {
                    case 0:
                        name = "Tombstone";
                        break;
                    case 1:
                        name = "Church";
                        break;
                    case 2:
                        name = "Catherdral";
                        break;
                    default:
                        Debug.LogError("Tile :" + coordinates.x + "," + coordinates.y + " level was incorrect");
                        break;
                }
                break;
            case Type.Crystal:
                switch (level)
                {
                    case 0:
                        name = "Rock";
                        break;
                    case 1:
                        name = "Big rock";
                        break;
                    default:
                        Debug.LogError("Tile :" + coordinates.x + "," + coordinates.y + " level was incorrect");
                        break;
                }
                break;
            case Type.Bear:

                break;
            case Type.Box:
                switch (level)
                {
                    case 0:
                        name = "Box";
                        break;
                    case 1:
                        name = "Big box";
                        break;
                    default:
                        Debug.LogError("Tile :" + coordinates.x + "," + coordinates.y + " level was incorrect");
                        break;
                }
                break;
            case Type.Robot:
                name = "Robot";
                level = 0;
                break;
            default:
                Debug.LogError("Tile :" + coordinates.x + "," + coordinates.y + " Type was incorrect");
                break;
        }
    } 
}