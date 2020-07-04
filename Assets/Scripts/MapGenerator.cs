using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
	private int width, height;

	[Range(GameConstants.MIN_RANDOM_FILL_PERCENT, GameConstants.MAX_RANDOM_FILL_PERCENT)]
	public int randomFillPercent;

	[Range(GameConstants.MIN_BIRTH_DEATH_LIMIT, GameConstants.MAX_BIRTH_DEATH_LIMIT)]
	public int birthLimit, deathLimit;

	[Range(GameConstants.MIN_REPETITIONS_LIMIT, GameConstants.MAX_REPETITIONS_LIMIT)]
	public int repetitions;

	int[,] terrainMap;
	public Vector3Int terrainMapSize;

	public Tile topTile;
	public Tile botTile;

	public Tilemap topTilemap;
	public Tilemap botTilemap;

	private void Start()
	{
		GenerateMap(repetitions);
	}

	void GenerateMap(int repetitions)
	{
		ClearMap();
		width = terrainMapSize.x;
		height = terrainMapSize.y;

		if (terrainMap == null)
		{
			terrainMap = new int[width, height];
			GenerateTerrain();
		}

		for (int i = 0; i < repetitions; i++)
		{
			terrainMap = GenerateTiles(terrainMap);
		}

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				if(terrainMap[i, j] == 1)
				{
					topTilemap.SetTile(new Vector3Int(-i + width / 2, -j + height / 2, 0), topTile);
					botTilemap.SetTile(new Vector3Int(-i + width / 2, -j + height / 2, 0), botTile);
				}
			}
		}
	}

	void GenerateTerrain()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				int randomMin = GameConstants.MIN_RANDOM_FILL_PERCENT + 1;
				int randomMax = GameConstants.MAX_RANDOM_FILL_PERCENT + 1;
				terrainMap[i, j] = Random.Range(randomMin, randomMax) < randomFillPercent ? 1 : 0;
			}
		}
	}

	int[,] GenerateTiles(int[,] oldMap)
	{
		int[,] newMap = new int[width, height];
		int neighbors;

		BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);

		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				neighbors = 0;

				foreach (Vector3Int bound in bounds.allPositionsWithin)
				{
					if (bound.x == 0 && bound.y == 0)
					{
						continue;
					}

					if (i + bound.x >= 0 && i + bound.x < width &&
						j + bound.y >= 0 && j + bound.y < height)
					{
						neighbors += oldMap[i + bound.x, j + bound.y];
					}
					else
					{
						neighbors++;
					}
				}

				if (oldMap[i, j] == 1)
				{
					if (neighbors < deathLimit) newMap[i, j] = 0;
					else
					{
						newMap[i, j] = 1;
					}
				}

				if (oldMap[i, j] == 0)
				{
					if (neighbors > birthLimit) newMap[i, j] = 1;
					else
					{
						newMap[i, j] = 0;
					}
				}
			}
		}

		return newMap;
	}

	public void ClearMap(bool isComplete = false)
	{
		if (topTilemap != null || botTilemap != null)
		{
			topTilemap.ClearAllTiles();
			botTilemap.ClearAllTiles();
		}

		if (isComplete)
		{
			terrainMap = null;
		}
	}
}
