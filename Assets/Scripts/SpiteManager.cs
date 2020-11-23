using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpiteManager : MonoBehaviour
{
    string title;
    public int totalScore = 0;
    int scoreAmount;
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
    [SerializeField] TMP_Text scoreAmountText;
    [SerializeField] TMP_Text lastScoreAmount;


    [SerializeField] TileParser tileParser;

    void AddScore(int amount)
    {
        totalScore += amount;
        scoreAmountText.text = totalScore.ToString();
    }

    public void SetLastScore(int amount)
    {
        lastScoreAmount.text = amount.ToString();
    }

    private void Start()
    {
        SetLastScore(0);
        AddScore(0);
        tileParser.TileUpdated += UpdateTile; // when tile gets updated update visuals
    }

    public void UpdateUI(Tile what) 
    {
        title = ConnectDataToVisuals(what);
        nextPlacableObjectImage.sprite = requestedObject.GetComponent<SpriteRenderer>().sprite;
        NextPlacibleObjectName.text = "Place a " + title;
    }

    public void UpdateTile(int x, int y) // clear old and add new gameObject for tile
    {
        tileParser.tile[x, y].tileName = ConnectDataToVisuals(tileParser.tile[x, y]);
            foreach (Transform child in tileParser.tile[x, y].transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        Instantiate(requestedObject, tileParser.tile[x, y].transform);
        AddScore(scoreAmount);
    }

    public void SimulateFake(int x, int y, Tile tamplate) // clear old and add new gameObject for tile
    {
        ConnectDataToVisuals(tamplate);
        foreach (Transform child in tileParser.tile[x, y].transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Instantiate(requestedObject, tileParser.tile[x, y].transform);
    }

    public void ForceUpdateAll()
    {
        foreach(Tile t in tileParser.tile)
        {
            UpdateTile(t.coordinates.x, t.coordinates.y);
        }
    }

    public string ConnectDataToVisuals(Tile tile)
    {
        string name = "Error";
        switch (tile.tileType)
        {
            case Tile.Type.Wood:
                switch (tile.level)
                {
                    case 0:
                        name = "Grass";
                        scoreAmount = 10;
                        requestedObject = grass;
                        break;
                    case 1:
                        name = "Bush";
                        scoreAmount = 50;
                        if (tile.specialVersion)
                            requestedObject = bushSpecial;
                        requestedObject = bush;
                        break;
                    case 2:
                        name = "Tree";
                        scoreAmount = 100;
                        if (tile.specialVersion)
                            requestedObject = treeSpecial;
                        requestedObject = tree;
                        break;
                    case 3:
                        name = "Hut";
                        scoreAmount = 250;
                        if (tile.specialVersion)
                            requestedObject = hutSpecial;
                        requestedObject = hut;
                        break;
                    case 4:
                        name = "House";
                        scoreAmount = 500;
                        if (tile.specialVersion)
                            requestedObject = houseSpecial;
                        requestedObject = house;
                        break;
                    case 5:
                        name = "Castle";
                        scoreAmount = 1000;
                        if (tile.specialVersion)
                            requestedObject = castleSpecial;
                        requestedObject = castle;
                        break;
                    case 6:
                        name = "Floating House";
                        scoreAmount = 2500;
                        if (tile.specialVersion)
                            requestedObject = floatingHouseSpecial;
                        requestedObject = floatingHouse;
                        break;
                    case 7:
                        name = "Floating Castle";
                        scoreAmount = 5000;
                        requestedObject = floatingCastle;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Empty:
                name = "Empty";
                scoreAmount = 0;
                requestedObject = empty;
                break;
            case Tile.Type.Holy:
                switch (tile.level)
                {
                    case 0:
                        name = "Tombstone";
                        scoreAmount = 20;
                        requestedObject = tombstone;
                        break;
                    case 1:
                        name = "Church";
                        scoreAmount = 50;
                        requestedObject = church;
                        break;
                    case 2:
                        name = "Catherdral";
                        scoreAmount = 100;
                        requestedObject = cathedral;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Crystal:
                switch (tile.level)
                {
                    case 0:
                        name = "Rock";
                        scoreAmount = 10;
                        requestedObject = rock;
                        break;
                    case 1:
                        name = "Big rock";
                        scoreAmount = 20;
                        requestedObject = bigRock;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Bear:
                name = "Bear";
                scoreAmount = 0;
                requestedObject = bear;
                break;
            case Tile.Type.Box:
                switch (tile.level)
                {
                    case 0:
                        name = "Box";
                        scoreAmount = 1000;
                        requestedObject = box;
                        break;
                    case 1:
                        name = "Big box";
                        scoreAmount = 2000;
                        requestedObject = chest;
                        break;
                    default:
                        break;
                }
                break;
            case Tile.Type.Robot:
                name = "Robot";
                requestedObject = robot;
                break;
            default:
                break;
        }
        return name;
    }
}
