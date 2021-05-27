using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
	[Header("Tilemap")]
	[Tooltip("The Tilemap to draw onto")]
	public Tilemap tilemap;
	[Tooltip("The Tile to draw (use a RuleTile for best results)")]
	public TileBase tile;

	[Header("Custom elements")]
	[Tooltip("Game object under whom will Instantiate objects")]
	public Transform objectsParent;
	[Tooltip("Spawn prefab")]
	public GameObject spawn;
	[Tooltip("Teleport prefab")]
	public GameObject teleport;


	[Tooltip("Width of our map")]
	[Range(7, 40)]
	public int roomWidth = 14;
	[Tooltip("Height of our map")]
	[Range(7, 30)]
	public int roomHeight = 8;

	[Tooltip("Width of our map")]
	public int horizontalRooms = 6;
	[Tooltip("Height of our map")]
	public int verticalRooms = 3;

	[Tooltip("Height of our map")]
	public int maxPathLength = 15;

	[Tooltip("The settings of our map")]
	[Range(0f, 100f)]
	public float fillAmount;

	[Range(0, 10)]
	public int smoothAmount;

	[ExecuteInEditMode]
	public void GenerateMap()
	{
		ClearMap();
		Room[,] rooms = RoomFunctions.CreateRooms(horizontalRooms, verticalRooms, roomWidth, roomHeight);

		for (int i = 0; i < horizontalRooms; i++)
		{
			for (int j = 0; j < verticalRooms; j++)
			{
				int[,] map = RoomFunctions.GenerateCellularAutomata(roomWidth, roomHeight, fillAmount);
				map = RoomFunctions.SmoothMooreCellularAutomata(map, smoothAmount);
				rooms[i, j].ChangeMap(map);
			}
		}

		RoomFunctions.CreatePathInRooms(rooms, maxPathLength);
		RoomFunctions.RenderRooms(rooms, tilemap, tile);
	}

	public void ClearMap()
	{
		tilemap.ClearAllTiles();
		foreach (Transform child in objectsParent)
		{
			Destroy(child.gameObject);
		}
	}

	private void Awake()
	{
		RoomFunctions.objectsParent = objectsParent;
		RoomFunctions.teleport = teleport;
		RoomFunctions.spawn = spawn;

		ClearMap();
		GenerateMap();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.N))
		{
			ClearMap();
			GenerateMap();
		}
	}

}

[CustomEditor(typeof(RoomGenerator))]
public class RoomGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		//Reference to our script
		RoomGenerator levelGen = (RoomGenerator)target;

		if (GUILayout.Button("Generate"))
		{
			levelGen.GenerateMap();
		}

		if (GUILayout.Button("Clear"))
		{
			levelGen.ClearMap();
		}
	}
}
