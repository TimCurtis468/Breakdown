using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region Singleton
    private static BricksManager _instance;
    public static BricksManager Instance => _instance;

    public static event Action OnLevelLoaded;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private int numRows = 12;
    private int numCols = 10;
    private float initialBrickSpawnPositionX = -2.35f;
    private float initialBrickSpawnPositionY = 3.125f;
    private float xshiftAmount = 0.5285f;
    private float yshiftAmount = 0.42f;

    private GameObject bricksContainer;

    public static event Action OnLevelComplete;


    public Brick brickPrefab;

    public Sprite[] Sprites;

    public List<Brick> RemainingBricks { get; set; }

    private List<Level> levels;
    public int levelNum = 0;
    private bool allLevelsComplete = false;

    public int InitialBricksCount { get; set; }

    private void Start()
    {
        this.bricksContainer = new GameObject("BricksContainer");
        levels = new List<Level>();
        LoadLevelsData();
        this.GenerateBricks();
        levelNum = 0;
        allLevelsComplete = false;
    }

    public void LoadNextLevel()
    {
        // Check if all levels have been completed
        if (allLevelsComplete == false)
        {
            // Move to next level
            this.levelNum++;
        }
        else
        {
            // Pick a random level as all have been completed
            this.levelNum = UnityEngine.Random.Range(0, this.levels.Count);
        }

        // Check if all levels have been completed
        if (this.levelNum >= this.levels.Count)
        {
            this.levelNum = UnityEngine.Random.Range(0, this.levels.Count);
            allLevelsComplete = true;
        }
        this.LoadLevel(this.levelNum);
    }

    public void LoadLevel(int level)
    {
        this.levelNum = level;
        this.ClearRemainingBricks();
        OnLevelComplete?.Invoke();
        this.GenerateBricks();
    }


    private void ClearRemainingBricks()
    {
        foreach (Brick brick in this.RemainingBricks.ToList())
        {
            Destroy(brick.gameObject);
        }
    }

    private void GenerateBricks()
    {
        this.RemainingBricks = new List<Brick>();
        Level currentLevel = levels[this.levelNum];
        float currentSpawnX = Utilities.ResizeXValue(initialBrickSpawnPositionX);
        float currentSpawnY = Utilities.ResizeYValue(initialBrickSpawnPositionY);
        float zShift = 0;

        for (int row = 0; row < this.numRows; row++)
        {
            for (int col = 0; col < this.numCols; col++)
            {
                int brickHits = currentLevel.hits[col,row];
                if (brickHits > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0 - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, this.Sprites[0], GetColour(currentLevel.colours[col, row]), currentLevel.hits[col, row]);

                    Utilities.ResizeSprite(newBrick.gameObject);

                    this.RemainingBricks.Add(newBrick);
                    zShift += 0.0001f;
                }

                currentSpawnX += Utilities.ResizeXValue(xshiftAmount);
                if (col + 1 >= this.numCols)
                {
                    currentSpawnX = Utilities.ResizeXValue(initialBrickSpawnPositionX);
                }
            }

            currentSpawnY -= Utilities.ResizeYValue(yshiftAmount);
            zShift = ((row + 1) * 0.0005f);
        }

        this.InitialBricksCount = this.RemainingBricks.Count;
        OnLevelLoaded?.Invoke();

    }

    private void LoadLevelsData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;

        string[] levelsFile = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        int currentRow = 0;
        int numLevels = 0;

        while (currentRow < levelsFile.Length)
        {
            Level currentLevel = new Level();
            /* Load hits table for this level */
            for (int y = 0; y < numRows; y++)
            {
                string line = levelsFile[currentRow];
                string[] hits = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numCols; x++)
                {
                    currentLevel.hits[x, y] = int.Parse(hits[x]);
                }

                currentRow++;
            }

            /* Skip end of hits marker */
            currentRow++;

            /* Load colours table for this level */
            for (int y = 0; y < numRows; y++) 
            {
                string line = levelsFile[currentRow];
                string[] colours = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int x = 0; x < numCols; x++)
                {
                    currentLevel.colours[x, y] = int.Parse(colours[x], System.Globalization.NumberStyles.HexNumber);
                }

                currentRow++;
            }

            levels.Add(currentLevel);

            /* Skip end of colours marker */
            currentRow++;

            numLevels++;
        }

    }
    private Color GetColour(int intColour)
    {
        int r = (intColour >> 16) & 0xFF;
        int g = (intColour >> 8) & 0xFF;
        int b = intColour & 0xFF;

        Color color = new Color(r/255.0f, g/255.0f, b/255.0f);

        return color;
    }

    private class Level
    {
        public int[,] hits = new int[10, 12];
        public int[,] colours = new int[10, 12];
    }
}




