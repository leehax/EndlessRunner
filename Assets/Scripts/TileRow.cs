using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileRow : MonoBehaviour
{
    public enum RowTypes
    {
        SingleTile,
        EdgeTiles,
        EdgeTilesWithWall,
        DecoyTile
    }


    private GameObject[] m_tiles;
   

    private DecoyTile[] m_decoyTiles;

    private GameObject m_wallObstacle;

    [SerializeField] private GameObject m_tilePrefab;
    [SerializeField] private GameObject m_wallPrefab;

    public string m_prevType="";

    private int m_amountOfTypes = Enum.GetValues(typeof(RowTypes)).Length;

    void Awake()
    {
        m_tiles = new GameObject[3];
        m_decoyTiles = new DecoyTile[3];
       
        for (int i = 0; i < 3; i++)
        {
            m_tiles[i] = Instantiate(m_tilePrefab, transform, false);
            m_decoyTiles[i] = m_tiles[i].GetComponent<DecoyTile>(); //cache decoy tile component to minimize GetComponent calls
           
        }

       
        m_tiles[0].transform.localPosition = new Vector3(-2, 0, 0); //left 
        m_tiles[1].transform.localPosition = new Vector3(0, 0, 0);  //center
        m_tiles[2].transform.localPosition = new Vector3(2, 0, 0);  //right

       m_wallObstacle = Instantiate(m_wallPrefab, transform, false);
       m_wallObstacle.transform.localPosition = new Vector3(0, 1, 2);


    }

   

    public RowTypes ActivateRandomType(AnimationCurve probabilityCurve)
    {
        RowTypes randomType = (RowTypes) CalculateWeightedRandom(probabilityCurve);
        ActivateWithType(randomType);
        return randomType;
    }

    private void ActivateWithType(RowTypes rowType)
    {
        switch (rowType)
        {
            case RowTypes.SingleTile:
            {
                ActivateAnySingleTile();
            } break;

          case RowTypes.EdgeTiles:
            {
                ActivateEdgeTiles();
            } break;

            case RowTypes.EdgeTilesWithWall:
            {
                ActivateEdgeTilesWithWall();
            } break;

            case RowTypes.DecoyTile:
            {
                ActivateDecoyTile();
            } break;
        }
    }

    public RowTypes ActivateLeftOrRight()
    {
        ResetTiles();

        gameObject.name = "RowSingleTile";
        int[] possibleTileIndex = { 0, 2 }; //0(left) or 2(right)

        int tileIndex = possibleTileIndex[Random.Range(0, 2)]; //random range(int) max is exclusive 

        m_tiles[tileIndex].SetActive(true);
        return RowTypes.SingleTile;

    }

    private void ActivateAnySingleTile()
    {
        ResetTiles();
        gameObject.name = "RowSingleTile";
        m_tiles[Random.Range(0,3)].SetActive(true);
    }
    private void ActivateSingleTile(GameObject tileToActivate)
    {
        ResetTiles();

        gameObject.name = "RowSingleTile";
        tileToActivate.SetActive(true);
    }

    private void ActivateEdgeTiles()
    {
        ResetTiles();

        gameObject.name = "RowEdgeTiles";
        m_tiles[0].SetActive(true);
        m_tiles[2].SetActive(true);

    }

    private void ActivateEdgeTilesWithWall()
    {
        ResetTiles();

        gameObject.name = "RowEdgeTilesWall";
        m_tiles[0].SetActive(true);
        m_tiles[2].SetActive(true);
        m_wallObstacle.SetActive(true);

    }

    private void ActivateDecoyTile()
    {
        ActivateEdgeTiles();

        gameObject.name = "RowDecoyTile";
        int[] possibleDecoyIndex = {0,2}; //rows with decoys will look like the rows with edge tiles, hence the decoy can only be 0(left) or 2(right)

        int decoyIndex = possibleDecoyIndex[Random.Range(0, 2)]; //random range(int) max is exclusive 

        m_decoyTiles[decoyIndex].enabled = true;
        m_decoyTiles[decoyIndex].gameObject.tag = "Obstacle";

    }

    private void ResetTiles()
    {
        
        foreach (GameObject tile in m_tiles)
        {
            tile.SetActive(false);
            tile.tag = "Platform";
        }

        foreach (DecoyTile decoyTile in m_decoyTiles)
        {
            decoyTile.enabled = false;
        }

        m_wallObstacle.SetActive(false);
    }


    private int CalculateWeightedRandom( AnimationCurve probabilityCurve)
    {
        return (int)probabilityCurve.Evaluate(Random.value);
    }
    

}
