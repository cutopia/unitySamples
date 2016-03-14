using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class GameManagerScript : MonoBehaviour {

	public class IntVector2 {
		public int x;
		public int y;
		public IntVector2(int parmX, int parmY) {
			x = parmX;
			y = parmY;
		}
	};
	public int dim = 21;
	public int hallWidth = 1;
	public GameObject[] wallTiles;
	private Transform boardHolder;
	private int[,] _mazeMap;

	private const int PATHWAY = 0;
	private const int WALL = 1;

	// Use this for initialization
	void Awake () {

		startMazeGeneration ();
	}

	// Update is called once per frame
	void Update () {
	}
	
	void startMazeGeneration() {

		_mazeMap = new int[dim,dim];
		// initialize maze to all walls.
		for (int i = 0; i < dim; i++) {
			for (int j = 0; j < dim; j++) {
				_mazeMap[i,j] = WALL;
			}
		}
		// recursively carve out the pathway.
		generateMazeStep (1, 1);

		renderMaze ();
	}

	void renderMaze() {

		boardHolder = new GameObject ("board").transform;

		for (int i = 0; i < dim; i++) {
			for (int j = 0; j < dim; j++) {
				if (_mazeMap[i,j] == WALL) {
					GameObject wall = Instantiate(wallTiles[Random.Range (0,wallTiles.Length)], new Vector3(i,j,0f), Quaternion.identity) as GameObject;
					wall.transform.SetParent(boardHolder);
				}
			}
		}
	}

	void generateMazeStep(int x, int y) {

		List<IntVector2> validDirs;
		_mazeMap[x,y] = PATHWAY;
		validDirs = findValidDirections (x, y);
		while (validDirs.Count > 0) {
			IntVector2 chosenDir = validDirs [Random.Range (0, validDirs.Count)];

			// set the path between the current point and the chosen direction to pathway.
			for (int m = Mathf.Min (x, chosenDir.x); m < Mathf.Max (x,chosenDir.x); m++) {
				_mazeMap [m, y] = PATHWAY;
			}
			for (int m = Mathf.Min (y, chosenDir.y); m < Mathf.Max (y,chosenDir.y); m++) {
				_mazeMap [x, m] = PATHWAY;
			}

			generateMazeStep (chosenDir.x, chosenDir.y);
			// see if there are any other ways we can go.
			validDirs = findValidDirections (x, y);
		}
	}

	private List<IntVector2> findValidDirections(int i, int j) {
		
		List<IntVector2> validDirections = new List<IntVector2>();
		if ((i - 2) > 0) {
			if (_mazeMap[i - 2, j] == WALL) {
				validDirections.Add(new IntVector2(i - 2, j));
			}
		}
		if ((j - 2) > 0) {
			if (_mazeMap[i, j - 2] == WALL) {
				validDirections.Add(new IntVector2(i, j - 2));
			}
		} 
		if ((j + 2) < (dim - 1)) {
			if (_mazeMap[i, j + 2] == WALL) {
				validDirections.Add(new IntVector2(i, j + 2));
			}
		}
		if ((i + 2) < (dim - 1)) {
			if (_mazeMap[i + 2, j] == WALL) {
				validDirections.Add(new IntVector2(i + 2, j));
			}
		}
		return validDirections;
	}
}
