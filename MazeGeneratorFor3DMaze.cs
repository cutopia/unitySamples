using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : MonoBehaviour {

	[SerializeField] public GameObject mazeWallPrefab;
	[SerializeField] public GameObject mazeFloorPrefab;
	public int dim = 10;
	public int cellWidth = 10;

	private float _halfCellWidth;

	private int[,] _mazeMap;
	private float _mazeDimension;
	private float _startingPoint;

	private const int VISITED = 1;
	private const int EASTOPEN = 2;
	private const int WESTOPEN = 4;
	private const int NORTHOPEN = 8;
	private const int SOUTHOPEN = 16;
	
	// Use this for initialization
	void Start () {

		Random.seed = (int)System.DateTime.UtcNow.Second;
		_halfCellWidth = cellWidth * 0.5f;
		_mazeMap = new int[dim,dim];

		generateCell (0, 0); // generate the maze.

		for (int j = 0; j < dim; j++) {
			for (int i = 0; i < dim; i++) {
				placeWalls(i,j);
			}
		}
	}

	// going from top left to bottom right
	// just drawing top (north) and left (west) walls so that we don't draw the same wall twice.
	private void placeWalls(int x,int y) {

		//			Renderer rend = seg.GetComponent<Renderer>();
		//			float cellWidth = rend.bounds.size.x;
		GameObject seg;
		GameObject floor = Instantiate (mazeFloorPrefab) as GameObject;
		floor.transform.SetParent (gameObject.transform);
		floor.transform.localPosition = new Vector3 (x * cellWidth, 0, y * cellWidth);
		if ((_mazeMap [x, y] & WESTOPEN) != WESTOPEN) {
			// west wall
			seg = Instantiate (mazeWallPrefab) as GameObject;
			seg.transform.SetParent (gameObject.transform);
			seg.transform.localPosition = new Vector3 (x * cellWidth, 10, (y * cellWidth) - _halfCellWidth);
		}
		if ((_mazeMap [x, y] & NORTHOPEN) != NORTHOPEN) {
			// north wall
			seg = Instantiate (mazeWallPrefab) as GameObject;
			seg.transform.SetParent (gameObject.transform);
			seg.transform.localEulerAngles = new Vector3 (0, 90, 0);		
			seg.transform.localPosition = new Vector3 ((x * cellWidth) - _halfCellWidth, 10, y * cellWidth);
		}
		if (x == dim - 1) {
			seg = Instantiate(mazeWallPrefab) as GameObject;
			seg.transform.SetParent(gameObject.transform);
			seg.transform.localEulerAngles = new Vector3 (0, 90, 0);		
			seg.transform.localPosition = new Vector3 (((x+1) * cellWidth) - _halfCellWidth, 10, y * cellWidth);
		}
		if (y == dim - 1) {
			seg = Instantiate(mazeWallPrefab) as GameObject;
			seg.transform.SetParent(gameObject.transform);
			seg.transform.localPosition = new Vector3 (x * cellWidth, 10, ((y+1) * cellWidth) - _halfCellWidth);
		}
	}

	private int getOppositeDir(int direction) {

		if (direction == SOUTHOPEN) {
			return NORTHOPEN;
		}
		if (direction == NORTHOPEN) {
			return SOUTHOPEN;
		}
		if (direction == EASTOPEN) {
			return WESTOPEN;
		}
		if (direction == WESTOPEN) {
			return EASTOPEN;
		}
		return 0;
	}

	private void generateCell(int x, int y) {

		_mazeMap [x, y] |= VISITED;

		List<Vector3> validDirs = findValidDirections (x, y);

		while (validDirs.Count > 0) {
			int index = Random.Range(0, validDirs.Count);
			Debug.Log("x= " + x + " y= " + y + " picked index " + index + " out of " + validDirs.Count);
			Vector3 nextDir = validDirs[index];
			validDirs.RemoveAt(index);
			if ((_mazeMap[(int)nextDir.x, (int)nextDir.y] & VISITED) != VISITED) {
				_mazeMap[x,y] |= (int)nextDir.z;
				_mazeMap[(int)nextDir.x, (int)nextDir.y] |= getOppositeDir((int)nextDir.z);
				generateCell((int)nextDir.x, (int)nextDir.y);
			}
		}
	}

	private List<Vector3> findValidDirections(int i, int j) {

		List<Vector3> validDirections = new List<Vector3>();
		if (i > 0) {
			if ((_mazeMap[i - 1, j] & VISITED) != VISITED) {
				validDirections.Add(new Vector3(i - 1, j, NORTHOPEN));
			}
		}
		if (j > 0) {
			if ((_mazeMap[i, j - 1] & VISITED) != VISITED) {
				validDirections.Add(new Vector3(i, j - 1, WESTOPEN));
			}
		} 
		if (j < dim - 1) {
			if ((_mazeMap[i, j + 1] & VISITED) != VISITED) {
				validDirections.Add(new Vector3(i, j + 1, EASTOPEN));
			}
		}
		if (i < dim - 1) {
			if ((_mazeMap[i + 1, j] & VISITED) != VISITED) {
				validDirections.Add(new Vector3(i + 1, j, SOUTHOPEN));
			}
		}
		Debug.Log ("i= " + i + " j= " + j + " has " + validDirections.Count + " valid directions.");
		return validDirections;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
