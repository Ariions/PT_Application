using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SpiteManager : MonoBehaviour
{
    /// <summary>
    ///  ideally this all would and should be just sprite references but as  i dont have time to edit art most of art are created from multiple sprites
    /// </summary>
    /// 
    string title;
    public GameObject requestedObject;

    [SerializeField] GameObject empty;
    [Header("Wood type")]

    [SerializeField] GameObject grass;
    [SerializeField] GameObject bush;
    [SerializeField] GameObject bushSpecial;
    [SerializeField] GameObject tree;
    [SerializeField] GameObject treeSpecial;
    [SerializeField] GameObject hut;
    [SerializeField] GameObject hutSpecial;
    [SerializeField] GameObject house;
    [SerializeField] GameObject houseSpecial;
    [SerializeField] GameObject castle;
    [SerializeField] GameObject castleSpecial;
    [SerializeField] GameObject floatingHouse;
    [SerializeField] GameObject floatingHouseSpecial;
    [SerializeField] GameObject floatingCastle;

    [Header("Holy type")]
    [SerializeField] GameObject bear;
    [SerializeField] GameObject tombstone;
    [SerializeField] GameObject church;
    [SerializeField] GameObject cathedral;
    [SerializeField] GameObject box;
    [SerializeField] GameObject chest;

    [Header("Crystal type")]
    [SerializeField] GameObject rock;
    [SerializeField] GameObject bigRock;

    [Header("Misc type")]
    [SerializeField] GameObject robot;

    [Header("UI")]
    [Header("UI Elements")]
    [SerializeField] Image nextPlacableObjectImage;
    [SerializeField] TMP_Text NextPlacibleObjectName;

    [Header("sprites")]
    Sprite Grass;

  

    public void UpdateInfo(Tile.Type type, int level)
    {
        title = ConnectDataToVisuals(type, level);
        nextPlacableObjectImage.sprite = requestedObject.GetComponent<SpriteRenderer>().sprite;
        NextPlacibleObjectName.text = "Place a " + title;
    }

    public string ConnectDataToVisuals(Tile.Type type, int level, bool isSpecial = false)
    {
        string name = "Error";
        switch (type)
        {
            case Tile.Type.Wood:
                switch (level)
                {
                    case 0:
                        name = "Grass";
                        requestedObject = grass;
                        break;
                    case 1:
                        name = "Bush";
                        if (isSpecial)
                            requestedObject = bushSpecial;
                        requestedObject = bush;
                        break;
                    case 2:
                        name = "Tree";
                        if (isSpecial)
                            requestedObject = treeSpecial;
                        requestedObject = tree;
                        break;
                    case 3:
                        name = "Hut";
                        if (isSpecial)
                            requestedObject = hutSpecial;
                        requestedObject = hut;
                        break;
                    case 4:
                        name = "House";
                        if (isSpecial)
                            requestedObject = houseSpecial;
                        requestedObject = house;
                        break;
                    case 5:
                        name = "Castle";
                        if (isSpecial)
                            requestedObject = castleSpecial;
                        requestedObject = castle;
                        break;
                    case 6:
                        name = "Floating House";
                        if (isSpecial)
                            requestedObject = floatingHouseSpecial;
                        requestedObject = floatingHouse;
                        break;
                    case 7:
                        name = "Floating Castle";
                        requestedObject = floatingCastle;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Empty:
                name = "Empty";
                requestedObject = empty;
                break;
            case Tile.Type.Holy:
                switch (level)
                {
                    case 0:
                        name = "Tombstone";
                        requestedObject = tombstone;
                        break;
                    case 1:
                        name = "Church";
                        requestedObject = church;
                        break;
                    case 2:
                        name = "Catherdral";
                        requestedObject = cathedral;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Crystal:
                switch (level)
                {
                    case 0:
                        name = "Rock";
                        requestedObject = rock;
                        break;
                    case 1:
                        name = "Big rock";
                        requestedObject = bigRock;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Bear:
                name = "Bear";
                requestedObject = bear;
                break;
            case Tile.Type.Box:
                switch (level)
                {
                    case 0:
                        name = "Box";
                        requestedObject = box;
                        break;
                    case 1:
                        name = "Big box";
                        requestedObject = chest;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Robot:
                name = "Robot";
                requestedObject = robot;
                level = 0;
                break;
            default:
                break;
        }
        return name;
    }
}
