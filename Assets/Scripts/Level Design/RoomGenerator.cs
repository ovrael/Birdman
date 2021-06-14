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
	[SerializeField] private Tilemap tilemap;
	[Tooltip("The Tile to draw (use a RuleTile for best results)")]
	[SerializeField] private TileBase tile;

	[Header("Objects in map")]
	[SerializeField] private RoomPolishing roomPolishing;

	[Tooltip("Width of our map")]
	[Range(7, 40)]
	[SerializeField] private int roomWidth = 14;
	[Tooltip("Height of our map")]
	[Range(7, 30)]
	[SerializeField] private int roomHeight = 8;

	[Tooltip("Width of our map")]
	[SerializeField] private int horizontalRooms = 6;
	[Tooltip("Height of our map")]
	[SerializeField] private int verticalRooms = 3;

	[Tooltip("Height of our map")]
	[SerializeField] private int maxPathLength = 15;

	[Tooltip("The settings of our map")]
	[SerializeField] private bool deleteDefaultRooms;

	[Range(0f, 100f)]
	[SerializeField] private float fillAmount;

	[Range(0, 10)]
	[SerializeField] private int smoothAmount;

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

		if (deleteDefaultRooms)
			RoomFunctions.DeleteRoomsIsntOnPath(rooms);

		RoomFunctions.RenderRooms(rooms, tilemap, tile);
		roomPolishing.SetRoomsAndAddObjects(rooms);
	}

	public void ClearMap()
	{
		tilemap.ClearAllTiles();
		roomPolishing.ClearObjects();
	}

	private void Awake()
	{
		ClearMap();
		GenerateMap();
		// roomPolishing = null;
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
