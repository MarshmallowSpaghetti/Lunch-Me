using UnityEngine;
using System.Collections;

// Remove in build version
[ExecuteInEditMode]
public class CubeTile : MonoBehaviour
{
    public enum TileType
    {
        Grass,
        Stone
    }

    // Remove in build version
    [SerializeField]
    private TileType m_type;
    // For level designer. Remove in build version
    private TileType m_previousType;

    private MeshRenderer m_renderer;

    public TileType Type
    {
        get
        {
            return m_type;
        }

        set
        {
            if (m_type != value)
            {
                ApplyTileType(value);
            }
            m_type = value;
        }
    }

    public MeshRenderer Renderer
    {
        get
        {
            if (m_renderer == null)
                m_renderer = GetComponent<MeshRenderer>();
            return m_renderer;
        }

        private set
        {
            m_renderer = value;
        }
    }

    private void Awake()
    {
        m_previousType = m_type;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_type != m_previousType)
        {
            ApplyTileType(m_type);
            m_previousType = m_type;
        }
    }

    private void ApplyTileType(TileType _type)
    {
        try
        {
            Material[] newMaterial = new Material[2];
            newMaterial[0] = Resources.Load<Material>(_type.ToString() + "_Dirt");
            newMaterial[1] = Resources.Load<Material>(_type.ToString() + "_Surface");
            Renderer.sharedMaterials = newMaterial;

            //Renderer.materials[0] = Resources.Load<Material>(_type.ToString() + "_Dirt");
            //Renderer.materials[1] = Resources.Load<Material>(_type.ToString() + "_Surface");
        }
        catch (System.Exception)
        {
            throw new System.Exception("Can't find material for" + _type);
        }
    }
}
