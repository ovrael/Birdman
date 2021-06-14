using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CategoryChance
{
	public RoomCategory category;
	public int value;
}

public class RoomPolishing : MonoBehaviour
{
	[Header("Parent")]
	[SerializeField] private Transform objectsParent;

	[Header("SpawnObject")]
	[SerializeField] private GameObject spawn;
	[Tooltip("Best values are (0f, -0.4f)")]
	[SerializeField] private Vector2 spawnOffset;       // Vector2(0f, -0.4f)

	[Header("TeleportObject")]
	[SerializeField] private GameObject teleport;
	[Tooltip("Best values are (0f, +0.2f)")]
	[SerializeField] private Vector2 teleportOffset;    // Vector2(0f, +0.2f)

	[Header("Spawners")]
	[SerializeField] private GameObject[] spawners;

	[Header("Chances to create specific room. % is calculed base on value / sum of chances.")]
	[SerializeField] private CategoryChance[] categoryChances;
	private int[] chancesRoulette;

	private int halfRoomWidth;
	private int halfRoomHeight;

	private int enemyRooms = 0;
	private int clearRooms = 0;

	private void Start()
	{
		//if (!CheckCategories())
		//{
		//	throw new Exception("Wrong categories in field categoryChances. Must be between MinCategory and MaxCategory. Categories cannot be repeated!");
		//}

		//chancesRoulette = MergeCategoryChancesIntoTable();
	}

	private bool CheckCategories()
	{
		// also have to add check for multiple chosen categories
		bool goodCategories = true;
		for (int i = 0; i < categoryChances.Length; i++)
		{
			int categoryNumber = (int)categoryChances[i].category;
			if (categoryNumber <= (int)RoomCategory.MinCategory || categoryNumber >= (int)RoomCategory.MaxCategory)
			{
				goodCategories = false;
				break;
			}

			for (int j = i + 1; j < categoryChances.Length; j++)
			{
				if ((int)categoryChances[j].category == categoryNumber)
				{
					goodCategories = false;
					break;
				}
			}

		}
		return goodCategories;
	}

	private int[] MergeCategoryChancesIntoTable()
	{
		int sumOfWeights = 0;
		for (int i = 0; i < categoryChances.Length; i++)
		{
			//Debug.LogWarning("i: " + i + " category: " + categoryChances[i].category + " value: " + categoryChances[i].value);
			sumOfWeights += categoryChances[i].value;
		}

		int[] chances = new int[sumOfWeights];

		int index = 0;
		for (int i = 0; i < categoryChances.Length; i++)
		{
			for (int j = 0; j < categoryChances[i].value; j++)
			{
				chances[index] = i;
				index++;
			}
		}

		Debug.LogWarning("chances.length " + chances.Length);
		for (int i = 0; i < chances.Length; i++)
		{
			Debug.LogWarning("Chances[i]: " + chances[i]);
		}

		return chances;
	}

	public void ClearObjects()
	{
		foreach (Transform child in objectsParent)
		{
			Destroy(child.gameObject);
		}
	}

	private GameObject RandomSpawner()
	{
		int spawnerIndex = UnityEngine.Random.Range(0, spawners.Length);
		GameObject returnSpawner = spawners[spawnerIndex];
		return returnSpawner;
	}

	private void AddObject(Vector2 position, RoomCategory roomCategory)
	{
		GameObject objectToAdd = null;
		Vector2 offset = new Vector2(0, 0);

		switch (roomCategory)
		{
			case RoomCategory.Spawn:
				objectToAdd = spawn;
				offset = spawnOffset;
				break;
			case RoomCategory.Teleport:
				objectToAdd = teleport;
				offset = teleportOffset;
				break;
			case RoomCategory.Enemies:
				objectToAdd = RandomSpawner();
				break;
			default:
				break;
		}

		if (objectToAdd != null)
		{
			float posX = position.x + halfRoomWidth + offset.x;
			float posY = position.y + halfRoomHeight + offset.y;
			Vector3 spawnPosition = new Vector3(posX, posY, 0);
			Instantiate(objectToAdd, spawnPosition, Quaternion.identity, objectsParent);
		}
		else
		{
			Debug.LogError("Can't add object to room in category: " + roomCategory);
		}
	}

	private void AssignRoomCategory(Room room)
	{
		int rouletteIndex = UnityEngine.Random.Range(0, chancesRoulette.Length);

		CategoryChance chosenCategory = categoryChances[chancesRoulette[rouletteIndex]];
		RoomCategory category = chosenCategory.category;

		if (category == RoomCategory.Clear)
			clearRooms++;
		else
			enemyRooms++;

		room.Category = category;
	}

	public void SetRoomsAndAddObjects(Room[,] rooms)
	{
		if (!CheckCategories())
		{
			throw new Exception("Wrong categories in field categoryChances. Must be between MinCategory and MaxCategory. Categories cannot be repeated!");
		}

		if (chancesRoulette == null)
			chancesRoulette = MergeCategoryChancesIntoTable();

		int roomWidth = rooms[0, 0].Width;
		int roomHeight = rooms[0, 0].Height;
		halfRoomWidth = roomWidth / 2;
		halfRoomHeight = roomHeight / 2;

		Vector2Int position = new Vector2Int(0, 0);

		for (int i = 0; i < rooms.GetLength(1); i++)
		{
			for (int j = 0; j < rooms.GetLength(0); j++)
			{
				if (rooms[j, i].OnPath && rooms[j, i].Category == RoomCategory.Default)
				{
					AssignRoomCategory(rooms[j, i]);
				}

				// Debug.Log("Dla pokoju: " + j + ", " + i + " wylosowano: " + rooms[j, i].Category.ToString());

				AddObject(position, rooms[j, i].Category);
				position.x += roomWidth;
			}
			position.x = 0;
			position.y += roomHeight;
		}
		Debug.Log("Clear rooms: " + clearRooms + " and enemyRooms: " + enemyRooms);
	}
}
