using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] PlacementManager placement;
    [SerializeField] SpiteManager spiteManager;

    ArrayList TilesPickChances;
    BearAI bearAI;
    public GameObject endButton;

    [Header("Balance")]
    [SerializeField] int grassWeight = 50;
    [SerializeField] int bearWeight = 10;
    [SerializeField] int bushWeight = 10;
    [SerializeField] int treeWeight = 3;
    [SerializeField] int hutWeight = 1;
    [SerializeField] int crystalWeight = 3;
    [SerializeField] int robotWeight = 3;

    // Start is called before the first frame update
    void Awake()
    {
        bearAI = GetComponent<BearAI>();
        TilesPickChances = new ArrayList();
        GenereteChances();
        Tile.Type nextType;
        int nextLevel;
        GetNextTile(out nextType, out nextLevel);
        placement.SetNextTile(nextType, nextLevel);

    }

    private void Start()
    {
        endButton.SetActive(false);
        placement.OnTilePlace += TurnEnd;
        placement.GenerateMap();
    }

    public void Restart()
    {
        endButton.SetActive(false);
        placement.GenerateMap();
        spiteManager.SetLastScore(spiteManager.totalScore);
        spiteManager.totalScore = 0;
    }

    void TurnEnd()
    {
        bearAI.UpdateBears();
        Tile.Type nextType;
        int nextLevel;
        GetNextTile(out nextType, out nextLevel);
        SendNextTile(nextType, nextLevel);
        if (placement.checkIfLost())
        {
            endButton.SetActive(true);
        }
    }

    public void SendNextTile(Tile.Type nextType, int nextLevel)
    {
        placement.SetNextTile(nextType, nextLevel);
    }

    void GenereteChances() // generates array with diffrent weights from which later random change will pick the "winner" 
    {
        for(int i = 0; i < grassWeight; i++)
        {
            TilesPickChances.Add(Tile.Type.Wood);
            TilesPickChances.Add(0);
        }
        for (int i = 0; i < bearWeight; i++)
        {
            TilesPickChances.Add(Tile.Type.Bear);
            TilesPickChances.Add(0);
        }
        for (int i = 0; i < bushWeight; i++)
        {
            TilesPickChances.Add(Tile.Type.Wood);
            TilesPickChances.Add(1);
        }
        for (int i = 0; i < treeWeight; i++)
        {
            TilesPickChances.Add(Tile.Type.Wood);
            TilesPickChances.Add(2);
        }
        for (int i = 0; i < hutWeight; i++)
        {
            TilesPickChances.Add(Tile.Type.Wood);
            TilesPickChances.Add(3);
        }
        for (int i = 0; i < crystalWeight; i++)
        {
            TilesPickChances.Add(Tile.Type.Crystal);
            TilesPickChances.Add(0);
        }
        for (int i = 0; i < robotWeight; i++)
        {
            TilesPickChances.Add(Tile.Type.Robot);
            TilesPickChances.Add(0);
        }
    }

    // generate next tile type for you to place
    void GetNextTile(out Tile.Type type, out int level)
    {
        int randomindex = ((int)Random.Range(0f, (float)TilesPickChances.Count / 2)) * 2;
        type = (Tile.Type)TilesPickChances[randomindex];
        level = (int)TilesPickChances[randomindex + 1];
    }
}
