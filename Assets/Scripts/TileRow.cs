using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class TileRow : MonoBehaviour
{
    public enum RowTypes
    {
        LeftTile,
        CenterTile,
        RightTile,
        EdgeTiles,
        EdgeTilesWithWall,
        DecoyTile
    }


    private GameObject[] m_tiles;
   

    private DecoyTile[] m_decoyTiles;

    private GameObject m_wallObstacle;

    [SerializeField] private GameObject m_tilePrefab;
    [SerializeField] private GameObject m_wallPrefab;

    private RowTypes m_type = RowTypes.CenterTile;

  
    
    void Awake()
    {
        m_tiles = new GameObject[3];
        m_decoyTiles = new DecoyTile[3];
        m_wallObstacle = new GameObject("Wall");

        for (int i = 0; i < 3; i++)
        {
            m_tiles[i] = Instantiate(m_tilePrefab, transform, false);
            m_decoyTiles[i] = m_tiles[i].GetComponent<DecoyTile>();

        }

       
        m_tiles[0].transform.localPosition = new Vector3(-2, 0, 0); //left 
        m_tiles[1].transform.localPosition = new Vector3(0, 0, 0);  //center
        m_tiles[2].transform.localPosition = new Vector3(2, 0, 0);  //right

        m_wallObstacle = Instantiate(m_wallPrefab, transform, false);
        m_wallObstacle.transform.localPosition = new Vector3(0, 1, 2);


    }

    void OnEnable()
    {
        ActivateRandomType();
    }

    public void ActivateRandomType()
    {
        ActivateWithType(Random.Range(0, 6));
    }

    private void ActivateWithType(int rowTypeAsInt)
    {
        RowTypes rowType = (RowTypes) rowTypeAsInt;
        switch (rowType)
        {
            case RowTypes.LeftTile:
            {
                ActivateSingleTile(m_tiles[0]);
            } break;

            case RowTypes.CenterTile:
            {
               ActivateSingleTile(m_tiles[1]);
            } break;

            case RowTypes.RightTile:
            {
                ActivateSingleTile(m_tiles[2]);
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


    private void ActivateSingleTile(GameObject tileToActivate)
    {
        ResetTiles();

        tileToActivate.SetActive(true);
    }

    private void ActivateEdgeTiles()
    {
        ResetTiles();

        m_tiles[0].SetActive(true);
        m_tiles[2].SetActive(true);

    }

    private void ActivateEdgeTilesWithWall()
    {
        ResetTiles();

        m_tiles[0].SetActive(true);
        m_tiles[2].SetActive(true);
        m_wallObstacle.SetActive(true);

    }

    private void ActivateDecoyTile()
    {
        ActivateEdgeTiles();

        int[] possibleDecoyIndex = {0,2}; //rows with decoys will look like the rows with edge tiles, hence the decoy can only be 0(left) or 2(right)

        int decoyIndex = possibleDecoyIndex[Random.Range(0, 1)];

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

        m_wallPrefab.SetActive(false);
    }
  

    

}
