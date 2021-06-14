using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public enum RoomCategory
{
	Spawn = 1,
	Teleport = 2,
	MinCategory = 10,
	Enemies = 11,
	Clear = 12,
	MaxCategory,
	Default = 100
}

public class Room
{
	int width;
	int height;
	int[,] map;
	bool upOpen, downOpen, leftOpen, rightOpen;
	bool onPath;
	RoomCategory category;

	public Room(int width, int height)
	{
		this.width = width;
		this.height = height;
		this.map = new int[width, height];
		onPath = false;
		Category = RoomCategory.Default;

		SetExits(false, false, false, false);
		SetFullMap();
	}

	public void SetExits(bool upOpen, bool downOpen, bool leftOpen, bool rightOpen)
	{
		this.upOpen = upOpen;
		this.downOpen = downOpen;
		this.leftOpen = leftOpen;
		this.rightOpen = rightOpen;
	}

	private void SetSpecialRoom()
	{
		int platformLength = 7;

		for (int i = 1; i < width - 1; i++)
		{
			for (int j = 1; j < height - 1; j++)
			{
				map[i, j] = 0;
			}
		}

		int startIndex = width / 2 - platformLength / 2;

		int bias = 2;
		int platformHeight = height / 2 - bias;

		for (int i = startIndex; i < platformLength + startIndex; i++)
		{
			map[i, platformHeight] = 1;
		}
	}

	public void SetSpawn()
	{
		category = RoomCategory.Spawn;
		SetSpecialRoom();
	}

	public void SetTeleport()
	{
		category = RoomCategory.Teleport;
		SetSpecialRoom();
	}

	private void SetFullMap()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				map[i, j] = 1;
			}
		}
	}

	public void SetClearMap()
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				map[i, j] = 0;
			}
		}
	}

	public int[] GetIntArrayExits()
	{
		int[] exits = new int[4];

		if (upOpen)
			exits[0] = 1;
		else
			exits[0] = 0;

		if (downOpen)
			exits[1] = 1;
		else
			exits[1] = 0;

		if (leftOpen)
			exits[2] = 1;
		else
			exits[2] = 0;

		if (rightOpen)
			exits[3] = 1;
		else
			exits[3] = 0;

		return exits;
	}

	public void ChangeMap(int[,] newMap)
	{
		if (newMap.GetLength(0) == map.GetLength(0) && newMap.GetLength(1) == map.GetLength(1))
		{
			for (int i = 0; i < map.GetLength(0); i++)
			{
				for (int j = 0; j < map.GetLength(1); j++)
				{
					map[i, j] = newMap[i, j];
				}
			}
		}
	}

	public void OpenWallsInMap()
	{
		int midWidth = width / 2;
		int midHeight = height / 2;

		int startCutUp = midHeight - 1;
		int startCutDown = midHeight + 1;
		int startCutLeft = midWidth + 1;
		int startCutRight = midWidth - 1;

		if ((int)Category < 10)
		{
			startCutUp = height - 1;
			startCutDown = 0;
			startCutLeft = 0;
			startCutRight = width - 1;
		}

		if (upOpen)
		{
			for (int i = startCutUp; i < height; i++)
			{
				map[midWidth - 1, i] = 0;
				map[midWidth, i] = 0;
				map[midWidth + 1, i] = 0;
			}
		}

		if (downOpen)
		{
			for (int i = startCutDown; i >= 0; i--)
			{
				map[midWidth - 1, i] = 0;
				map[midWidth, i] = 0;
				map[midWidth + 1, i] = 0;
			}
		}

		if (leftOpen)
		{
			for (int i = startCutLeft; i >= 0; i--)
			{
				map[i, midHeight - 1] = 0;
				map[i, midHeight] = 0;
				map[i, midHeight + 1] = 0;
			}
		}

		if (rightOpen)
		{
			for (int i = startCutRight; i < width; i++)
			{
				map[i, midHeight - 1] = 0;
				map[i, midHeight] = 0;
				map[i, midHeight + 1] = 0;
			}
		}
	}

	public int Width { get => width; }
	public int Height { get => height; }
	public bool OnPath { get => onPath; set => onPath = value; }
	public bool UpOpen { get => upOpen; set => upOpen = value; }
	public bool DownOpen { get => downOpen; set => downOpen = value; }
	public bool LeftOpen { get => leftOpen; set => leftOpen = value; }
	public bool RightOpen { get => rightOpen; set => rightOpen = value; }
	public int[,] Map { get => map; }
	public RoomCategory Category { get => category; set => category = value; }
}
