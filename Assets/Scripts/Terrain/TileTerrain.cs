using UnityEngine;
using UnityEngine.Tilemaps;

namespace MightyPirates
{
    [RequireComponent(typeof(Tilemap))]
    public sealed class TileTerrain : MonoBehaviour
    {
        public static TileTerrain Instance;

        [SerializeField]
        private int m_Width = 128;

        [SerializeField]
        private int m_Height = 128;

        [SerializeField]
        private TileBase[] m_Rock;

        [SerializeField]
        private TileBase[] m_Open;

        [SerializeField]
        private bool m_GenerateOnEnable;

        [SerializeField]
        private long m_Seed;

        [SerializeField]
        private float m_NoisePersistence = 0.5f;

        [SerializeField]
        private int m_NoiseOctaves = 3;

        [SerializeField]
        private float m_NoisePeriod = 10f;

        [SerializeField]
        private float m_NoiseLacunarity = 1f;

        [SerializeField]
        private float m_Isolevel = 0.1f;

        [SerializeField]
        private float m_Margin = 10f;

        private Tilemap m_Tilemap;

        public Tilemap Tilemap => m_Tilemap;

        private void Awake()
        {
            if (m_Tilemap == null)
                m_Tilemap = GetComponent<Tilemap>();
        }

        private void OnEnable()
        {
            Instance = this;

            if (m_GenerateOnEnable)
            {
                GenerateTerrain();
            }
        }

        private void OnDisable()
        {
            Instance = null;
        }

        [ContextMenu("Generate")]
        private void GenerateTerrain()
        {
#if UNITY_EDITOR
            if (m_Tilemap == null)
                m_Tilemap = GetComponent<Tilemap>();
#endif

            m_Tilemap.ClearAllTiles();
            m_Tilemap.BoxFill(new Vector3Int(m_Width - 1, m_Height - 1, 0), m_Rock[0], 0, 0, m_Width - 1, m_Height - 1);

            long seed = m_Seed != 0 ? m_Seed : (long) (Random.value * int.MaxValue);
            FractalNoise noise = new FractalNoise(new OpenSimplexNoise(seed));
            noise.Persistence = m_NoisePersistence;
            noise.Octaves = m_NoiseOctaves;
            noise.Period = m_NoisePeriod;
            noise.Lacunarity = m_NoiseLacunarity;

            for (int x = 0; x < m_Width; x++)
            {
                float xMarginContribution = Mathf.Clamp01(Mathf.Max(m_Margin - x, m_Margin - (m_Width - x)) / m_Margin);
                for (int y = 0; y < m_Height; y++)
                {
                    float yMarginContribution = Mathf.Clamp01(Mathf.Max(m_Margin - y, m_Margin - (m_Height - y)) / m_Margin);
                    float marginContribution = Mathf.Max(xMarginContribution, yMarginContribution);

                    if (noise.Evaluate(x, y) - marginContribution > m_Isolevel)
                    {
                        m_Tilemap.SetTile(new Vector3Int(x, y, 0), m_Open[0]);
                    }
                }
            }
        }
    }
}