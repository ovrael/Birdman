using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomFunctions : MonoBehaviour
{
	public static Transform objectsParent;
	public static GameObject spawn;
	public static GameObject teleport;

	/// <summary>
	/// Creates a perlin noise int array for the top layer of a level
	/// </summary>
	/// <param name="horizontalRooms">Width of the array</param>
	/// <param name="verticalRooms">Height of the array</param>
	/// <param name="roomWidth">Height of the array</param>
	/// <param name="roomHeight">Seed used with perlin function</param>
	/// <returns>An array of ints generated through perlin noise</returns>
	public static Room[,] CreateRooms(int horizontalRooms, int verticalRooms, int roomWidth, int roomHeight)
	{
		Room[,] rooms = new Room[horizontalRooms, verticalRooms];

		for (int i = 0; i < horizontalRooms; i++)
		{
			for (int j = 0; j < verticalRooms; j++)
			{
				rooms[i, j] = new Room(roomWidth, roomHeight);
			}
		}
		return rooms;
	}

	// [UP, DOWN, LEFT, RIGHT]
	private static int[] CheckAvailableRooms(Room[,] rooms, int positionX, int positionY, out int availablePathsCount)
	{
		int horizontalRooms = rooms.GetLength(0);
		int verticalRooms = rooms.GetLength(1);

		availablePathsCount = 0;
		int[] availablePaths = new int[4];
		for (int i = 0; i < availablePaths.Length; i++)
		{
			availablePaths[i] = 1;
		}

		if (positionX == 0)
			availablePaths[2] = 0;
		else
		{
			if (rooms[positionX - 1, positionY].OnPath)
				availablePaths[2] = 0;
		}

		if (positionX == horizontalRooms - 1)
			availablePaths[3] = 0;
		else
		{
			if (rooms[positionX + 1, positionY].OnPath)
				availablePaths[3] = 0;
		}

		if (positionY == 0)
			availablePaths[1] = 0;
		else
		{
			if (rooms[positionX, positionY - 1].OnPath)
				availablePaths[1] = 0;
		}

		if (positionY == verticalRooms - 1)
			availablePaths[0] = 0;
		else
		{
			if (rooms[positionX, positionY + 1].OnPath)
				availablePaths[0] = 0;
		}

		foreach (var room in availablePaths)
		{
			if (room == 1)
				availablePathsCount++;
		}

		return availablePaths;
	}

	private static void CreateSpawn(Room[,] rooms, int x, int y)
	{
		rooms[x, y].OnPath = true;
		rooms[x, y].SetSpawn();
	}

	public static void CreatePathInRooms(Room[,] rooms, int pathLength)
	{
		if (pathLength > rooms.Length)
			pathLength = rooms.Length;

		int horizontalRooms = rooms.GetLength(0);
		int verticalRooms = rooms.GetLength(1);

		int positionX = Random.Range(0, horizontalRooms);
		int positionY = verticalRooms - 1;
		CreateSpawn(rooms, positionX, positionY);

		for (int i = 0; i < pathLength; i++)
		{
			int n = 0;
			int[] availableRooms = CheckAvailableRooms(rooms, positionX, positionY, out int availablePathsCount);
			int[] availableDirections = new int[availablePathsCount];

			for (int j = 0; j < availableRooms.Length; j++)
			{
				if (availableRooms[j] == 1)
				{
					availableDirections[n] = j;
					n++;
				}
			}

			int nextDirection = -1;
			int nextPositionX = positionX;
			int nextPositionY = positionY;

			if (availableDirections.Length > 0)
			{
				int roll = Random.Range(0, availablePathsCount);
				nextDirection = availableDirections[roll];
				nextPositionX = positionX;
				nextPositionY = positionY;

			}

			switch (nextDirection)
			{
				case 0:
					rooms[positionX, positionY].UpOpen = true;
					nextPositionY++;
					rooms[positionX, nextPositionY].DownOpen = true;
					break;
				case 1:
					rooms[positionX, positionY].DownOpen = true;
					nextPositionY--;
					rooms[positionX, nextPositionY].UpOpen = true;
					break;
				case 2:
					rooms[positionX, positionY].LeftOpen = true;
					nextPositionX--;
					rooms[nextPositionX, positionY].RightOpen = true;
					break;
				case 3:
					rooms[positionX, positionY].RightOpen = true;
					nextPositionX++;
					rooms[nextPositionX, positionY].LeftOpen = true;
					break;
			}

			rooms[nextPositionX, nextPositionY].OnPath = true;
			// rooms[nextPositionX, nextPositionY].RandomCategory();

			rooms[positionX, positionY].OpenWallsInMap();
			rooms[nextPositionX, nextPositionY].OpenWallsInMap();


			if (i == pathLength - 1)
			{
				if (rooms[nextPositionX, nextPositionY].Category != RoomCategory.Spawn)
					rooms[nextPositionX, nextPositionY].SetTeleport();
				else
					rooms[positionX, positionY].SetTeleport();
			}

			positionX = nextPositionX;
			positionY = nextPositionY;
		}
	}

	public static void AddTeleport(Vector2 position, float halfRoomWidth, float halfRoomHeight)
	{
		Vector2 bias = new Vector2(0f, +0.2f);
		Vector3 spawnPosition = new Vector3(position.x + halfRoomWidth + bias.x, position.y + halfRoomHeight + bias.y, 0);
		Instantiate(teleport, spawnPosition, Quaternion.identity, objectsParent);
	}

	public static void AddSpawn(Vector2 position, float halfRoomWidth, float halfRoomHeight)
	{
		Vector2 bias = new Vector2(0f, -0.4f);
		Vector3 spawnPosition = new Vector3(position.x + halfRoomWidth + bias.x, position.y + halfRoomHeight + bias.y, 0);
		Instantiate(spawn, spawnPosition, Quaternion.identity, objectsParent);
	}

	public static void AddElements(Room room, Vector2 position)
	{
		float halfRoomWidth = room.Width / 2f;
		float halfRoomHeight = room.Height / 2f;

		switch (room.Category)
		{
			case RoomCategory.Spawn:
				AddSpawn(position, halfRoomWidth, halfRoomHeight);
				break;
			case RoomCategory.Teleport:
				AddTeleport(position, halfRoomWidth, halfRoomHeight);
				break;
			case RoomCategory.Clear:
				break;
			case RoomCategory.WithEnemies:
				break;
			case RoomCategory.WithBounty:
				break;
			default:
				break;
		}
	}

	/// <summary>
	/// Draws the map to the screen
	/// </summary>
	/// <param name="map">Map that we want to draw</param>
	/// <param name="tilemap">Tilemap we will draw onto</param>
	/// <param name="tile">Tile we will draw with</param>
	public static void RenderRooms(Room[,] rooms, Tilemap tilemap, TileBase tile)
	{
		int roomWidth = rooms[0, 0].Width;
		int roomHeight = rooms[0, 0].Height;
		tilemap.ClearAllTiles(); //Clear the map (ensures we dont overlap)
		Vector2Int offset = new Vector2Int(0, 0);

		for (int i = 0; i < rooms.GetLength(1); i++)
		{
			for (int j = 0; j < rooms.GetLength(0); j++)
			{
				RenderMapWithOffset(rooms[j, i].Map, tilemap, tile, offset);
				AddElements(rooms[j, i], offset);

				offset.x += roomWidth;
			}
			offset.x = 0;
			offset.y += roomHeight;
		}
	}

	/// <summary>
	/// Renders a map using an offset provided, Useful for having multiple maps on one tilemap
	/// </summary>
	/// <param name="map">The map to draw</param>
	/// <param name="tilemap">The tilemap to draw on</param>
	/// <param name="tile">The tile to draw with</param>
	/// <param name="offset">The offset to apply</param>
	private static void RenderMapWithOffset(int[,] map, Tilemap tilemap, TileBase tile, Vector2Int offset)
	{
		for (int x = 0; x < map.GetLength(0); x++)
		{
			for (int y = 0; y < map.GetLength(1); y++)
			{
				if (map[x, y] == 1)
				{
					tilemap.SetTile(new Vector3Int(x + offset.x, y + offset.y, 0), tile);
				}
			}
		}
	}

	/// <summary>
	/// Creates the basis for our Advanced Cellular Automata functions.
	/// We can then input this map into different functions depending on
	/// what type of neighbourhood we want
	/// </summary>
	/// <param name="map">The array to be modified</param>
	/// <param name="seed">The seed we will use</param>
	/// <param name="fillPercent">The amount we want the map filled</param>
	/// <param name="edgesAreWalls">Whether we want the edges to be walls</param>
	/// <returns>The modified map array</returns>
	public static int[,] GenerateCellularAutomata(int width, int height, float fillPercent)
	{
		//Set up the size of our array
		int[,] map = new int[width, height];

		//Start looping through setting the cells.
		for (int x = 0; x < map.GetUpperBound(0); x++)
		{
			for (int y = 0; y < map.GetUpperBound(1); y++)
			{
				if (x == 0 || x == map.GetUpperBound(0) - 1 || y == 0 || y == map.GetUpperBound(1) - 1)
				{
					//Set the cell to be active if edges are walls
					map[x, y] = 1;
				}
				else
				{
					//Set the cell to be active if the result of rand.Next() is less than the fill percentage
					map[x, y] = (Random.Range(0f, 100f) < fillPercent) ? 1 : 0;
				}
			}
		}
		return map;
	}

	/// <summary>
	/// Smoothes a map using Moore's Neighbourhood Rules. Moores Neighbourhood consists of all neighbours of the tile, including diagonal neighbours
	/// </summary>
	/// <param name="map">The map to modify</param>
	/// <param name="edgesAreWalls">Whether our edges should be walls</param>
	/// <param name="smoothCount">The amount we will loop through to smooth the array</param>
	/// <returns>The modified map</returns>
	public static int[,] SmoothMooreCellularAutomata(int[,] map, int smoothCount)
	{
		for (int i = 0; i < smoothCount; i++)
		{
			for (int x = 0; x < map.GetLength(0); x++)
			{
				for (int y = 0; y < map.GetLength(1); y++)
				{
					int surroundingTiles = GetMooreSurroundingTiles(map, x, y);


					//Set the edge to be a wall if we have edgesAreWalls to be true
					if (x == 0 || x == (map.GetLength(0) - 1) || y == 0 || y == (map.GetLength(1) - 1))
					{
						map[x, y] = 1;
					}
					//If we have more than 4 neighbours, change to an active cell
					else if (surroundingTiles > 4)
					{
						map[x, y] = 1;
					}
					//If we have less than 4 neighbours, change to be an inactive cell
					else if (surroundingTiles < 4)
					{
						map[x, y] = 0;
					}

					//If we have exactly 4 neighbours, do nothing
				}
			}
		}
		return map;
	}

	/// <summary>
	/// Gets the surrounding amount of tiles using the Moore Neighbourhood
	/// </summary>
	/// <param name="map">The map to check</param>
	/// <param name="x">The x position we are checking</param>
	/// <param name="y">The y position we are checking</param>
	/// <param name="edgesAreWalls">Whether the edges are walls</param>
	/// <returns>An int with the amount of surrounding tiles</returns>
	static int GetMooreSurroundingTiles(int[,] map, int x, int y)
	{
		/* Moore Neighbourhood looks like this ('T' is our tile, 'N' is our neighbours)
		 * 
		 * N N N
		 * N T N
		 * N N N
		 * 
		 */

		int tileCount = 0;

		//Cycle through the x values
		for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
		{
			//Cycle through the y values
			for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
			{
				if (neighbourX >= 0 && neighbourX < map.GetLength(0) && neighbourY >= 0 && neighbourY < map.GetLength(1))
				{
					//We don't want to count the tile we are checking the surroundings of
					if (neighbourX != x || neighbourY != y)
					{
						tileCount += map[neighbourX, neighbourY];
					}
				}
			}
		}
		return tileCount;
	}
}
