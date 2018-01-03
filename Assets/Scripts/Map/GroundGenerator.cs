using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GroundGenerator : BaseSingletonMono<GroundGenerator>
{
    //private GameObject m_grassTilePrefab;
    //private GameObject m_stoneTilePrefab;
    private CubeTile m_tilePrefab;
    public int width = 20;
    public int height = 20;

    [SerializeField]
    private bool m_hasInited = false;

    //public GameObject GrassTile
    //{
    //    get
    //    {
    //        if (m_grassTilePrefab == null)
    //            m_grassTilePrefab = Resources.Load<GameObject>("Grid_Grass");
    //        return m_grassTilePrefab;
    //    }
    //}

    //public GameObject StoneTile
    //{
    //    get
    //    {
    //        if (m_stoneTilePrefab == null)
    //            m_stoneTilePrefab = Resources.Load<GameObject>("Grid_Stone");
    //        return m_stoneTilePrefab;
    //    }
    //}

    public CubeTile TilePrefab
    {
        get
        {
            if (m_tilePrefab == null)
                m_tilePrefab = Resources.Load<CubeTile>("CubeTile");
            return m_tilePrefab;
        }

        set
        {
            m_tilePrefab = value;
        }
    }

    // Use this for initialization
    void Awake()
    {
        if (m_hasInited == false)
            GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateMap()
    {
        m_hasInited = true;
        int tileWidth = (int)TilePrefab.transform.localScale.x;
        int tileHeight = (int)TilePrefab.transform.localScale.z;

        for (int x = -width / 2; x < (width + 1) / 2; x++)
        {
            for (int y = -height / 2; y < (height + 1) / 2; y++)
            {
                CubeTile tile = GameObject.Instantiate(TilePrefab,
                    new Vector3(x * tileWidth, 0, y * tileHeight), Quaternion.identity,
                    this.transform);
                tile.name = "Grass Tile (" + (x + width / 2) + ", " + (y + height / 2) + ")";
                tile.Type = CubeTile.TileType.Grass;
            }
        }
    }
}
