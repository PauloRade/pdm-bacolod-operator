using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class MeshGradientOpacityBorder : MonoBehaviour
{
    [Header("Texture Settings")]
    public int width = 512;
    public int height = 512;

    [Header("Grid Settings")]
    [Range(2, 10)]
    public int gridX = 4;
    [Range(2, 10)]
    public int gridY = 4;

    [Header("4 Base Colors")]
    public Color colorA = Color.red;
    public Color colorB = Color.blue;
    public Color colorC = Color.green;
    public Color colorD = Color.yellow;

    [Header("Corner Radii (0-1)")]
    [Range(0f, 1f)] public float topLeftRadius = 0.2f;
    [Range(0f, 1f)] public float topRightRadius = 0.2f;
    [Range(0f, 1f)] public float bottomLeftRadius = 0.2f;
    [Range(0f, 1f)] public float bottomRightRadius = 0.2f;

    [Header("Border Settings")]
    public int borderThickness = 4; // pixels

    [Header("Opacity")]
    [Range(0f, 1f)]
    public float opacity = 1f;

    [Header("Random Seed")]
    public int seed = 0;

    private Texture2D texture;
    private RawImage rawImage;

    private float lastTL, lastTR, lastBL, lastBR, lastOpacity;
    private int lastBorder, lastSeed;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        GenerateMesh();
        StoreLastValues();
    }

    void Update()
    {
        if (topLeftRadius != lastTL || topRightRadius != lastTR ||
            bottomLeftRadius != lastBL || bottomRightRadius != lastBR ||
            opacity != lastOpacity || borderThickness != lastBorder ||
            seed != lastSeed)
        {
            GenerateMesh();
            StoreLastValues();
        }
    }
    public void updateColor()
    {
        GenerateMesh();
        StoreLastValues();
    }

    private void StoreLastValues()
    {
        lastTL = topLeftRadius;
        lastTR = topRightRadius;
        lastBL = bottomLeftRadius;
        lastBR = bottomRightRadius;
        lastOpacity = opacity;
        lastBorder = borderThickness;
        lastSeed = seed;
    }

    public void GenerateMesh()
    {
        if (texture == null)
            texture = new Texture2D(width, height, TextureFormat.ARGB32, false);

        Color[,] gridColors = CreateRandomGrid(seed);

        int tlPx = Mathf.RoundToInt(topLeftRadius * Mathf.Min(width, height));
        int trPx = Mathf.RoundToInt(topRightRadius * Mathf.Min(width, height));
        int blPx = Mathf.RoundToInt(bottomLeftRadius * Mathf.Min(width, height));
        int brPx = Mathf.RoundToInt(bottomRightRadius * Mathf.Min(width, height));

        for (int px = 0; px < width; px++)
        {
            for (int py = 0; py < height; py++)
            {
                bool insideShape = true;

                // Check if pixel is inside the rounded rectangle
                if (px < tlPx && py >= height - tlPx)
                    insideShape &= (Mathf.Pow(tlPx - px - 1, 2) + Mathf.Pow(py - (height - tlPx), 2) <= tlPx * tlPx);
                if (px >= width - trPx && py >= height - trPx)
                    insideShape &= (Mathf.Pow(px - (width - trPx), 2) + Mathf.Pow(py - (height - trPx), 2) <= trPx * trPx);
                if (px < blPx && py < blPx)
                    insideShape &= (Mathf.Pow(blPx - px - 1, 2) + Mathf.Pow(blPx - py - 1, 2) <= blPx * blPx);
                if (px >= width - brPx && py < brPx)
                    insideShape &= (Mathf.Pow(px - (width - brPx), 2) + Mathf.Pow(brPx - py - 1, 2) <= brPx * brPx);

                Color finalColor = new Color(0, 0, 0, 0);

                if (insideShape)
                {
                    bool inBorder = false;

                    // Edges
                    if (px < borderThickness || px >= width - borderThickness ||
                        py < borderThickness || py >= height - borderThickness)
                        inBorder = true;

                    // Corner borders
                    if (px < tlPx && py >= height - tlPx)
                        inBorder |= (Mathf.Sqrt(Mathf.Pow(tlPx - px - 1, 2) + Mathf.Pow(py - (height - tlPx), 2)) >= tlPx - borderThickness);
                    if (px >= width - trPx && py >= height - trPx)
                        inBorder |= (Mathf.Sqrt(Mathf.Pow(px - (width - trPx), 2) + Mathf.Pow(py - (height - trPx), 2)) >= trPx - borderThickness);
                    if (px < blPx && py < blPx)
                        inBorder |= (Mathf.Sqrt(Mathf.Pow(blPx - px - 1, 2) + Mathf.Pow(blPx - py - 1, 2)) >= blPx - borderThickness);
                    if (px >= width - brPx && py < brPx)
                        inBorder |= (Mathf.Sqrt(Mathf.Pow(px - (width - brPx), 2) + Mathf.Pow(brPx - py - 1, 2)) >= brPx - borderThickness);

                    finalColor = SampleMesh(px, py, gridColors);
                    finalColor.a *= opacity; // apply opacity to everything
                }

                texture.SetPixel(px, py, finalColor);
            }
        }

        texture.Apply();
        if (!rawImage) rawImage = GetComponent<RawImage>();
        rawImage.texture = texture;
    }

    private Color SampleMesh(int px, int py, Color[,] gridColors)
    {
        float u = (float)px / (width - 1) * (gridX - 1);
        int x0 = Mathf.FloorToInt(u);
        int x1 = Mathf.Clamp(x0 + 1, 0, gridX - 1);
        float tx = u - x0;

        float v = (float)py / (height - 1) * (gridY - 1);
        int y0 = Mathf.FloorToInt(v);
        int y1 = Mathf.Clamp(y0 + 1, 0, gridY - 1);
        float ty = v - y0;

        Color c00 = gridColors[x0, y0];
        Color c10 = gridColors[x1, y0];
        Color c01 = gridColors[x0, y1];
        Color c11 = gridColors[x1, y1];

        Color cx0 = Color.Lerp(c00, c10, tx);
        Color cx1 = Color.Lerp(c01, c11, tx);
        return Color.Lerp(cx0, cx1, ty);
    }

    private Color[,] CreateRandomGrid(int seed)
    {
        Random.InitState(seed); // use the seed for reproducibility

        Color[] colorPool = new Color[gridX * gridY];
        Color[] baseColors = new Color[] { colorA, colorB, colorC, colorD };

        for (int i = 0; i < colorPool.Length; i++)
            colorPool[i] = baseColors[i % 4];

        // Shuffle
        for (int i = colorPool.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Color temp = colorPool[i];
            colorPool[i] = colorPool[j];
            colorPool[j] = temp;
        }

        Color[,] gridColors = new Color[gridX, gridY];
        for (int x = 0; x < gridX; x++)
            for (int y = 0; y < gridY; y++)
                gridColors[x, y] = colorPool[y * gridX + x];

        return gridColors;
    }
}