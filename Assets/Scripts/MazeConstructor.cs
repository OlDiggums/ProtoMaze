using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Random = UnityEngine.Random;

public class MazeConstructor : MonoBehaviour
{
    [SerializeField] public Tilemap tileMapFloor;
    [SerializeField] public Tilemap tileMapWall;
    [SerializeField] public TileBase wallPrefab;
    [SerializeField] public TileBase floorPrefab;
    [SerializeField] public GameObject endPortal;
    [SerializeField] public Player user;
    [SerializeField] public int mazeWidth;
    [SerializeField] public int mazeHeight;
    

    private GameObject endPortalCreated;
    private MazeAlgorithm mazealgorithm;


    [SerializeField] public int[,] mazeGrid;

    [SerializeField] public char[,] mazeSolution;

    
    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }

    void Awake()
    {
        mazealgorithm = new MazeAlgorithm(mazeHeight,mazeWidth);
        GenerateNewMaze();
    }

    public void GenerateNewMaze()
    {
        CreateMaze(mazeHeight,mazeWidth);
        DestroyMaze();
        FindGoalPosition();
        GenerateRoom();
        PlaceGoalTrigger();
        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        user.transform.position = new Vector2(1.5f,1.5f);
        user.notAutosolving = true;
    }

    //------------------------------------------------------------------------//
    private void CreateMaze(int mazeHeight, int mazeWidth)
    {
        
        mazeGrid = mazealgorithm.ResetMaze();

    }
    private void DestroyMaze()
    {
        tileMapFloor.ClearAllTiles();
        tileMapWall.ClearAllTiles();
    }
    private void FindGoalPosition()
    {
        int rMax = mazeGrid.GetUpperBound(0);
        int cMax = mazeGrid.GetUpperBound(1);

        bool positionFound = false;

        while (!positionFound)
        {
            //I dont want the position to be close to the entrance, so it will either be atleast half the height of the
            // maze, or half the width. This requires atleast some exploration

            int minHeight = Random.value < 0.5 ? 2 : rMax / 2;
            int minWidth = minHeight == rMax / 2 ? 2 : cMax / 2;

            int testHeight = Random.Range(minHeight, rMax-2);
            int testWidth = Random.Range(minWidth, cMax-2);

            if (mazeGrid[testHeight, testWidth] != 1)
            {
                goalRow = testHeight;
                goalCol = testWidth;
                positionFound = true;
            }

        }
        
        mazeSolution = mazealgorithm.GenerateSolution(goalRow,goalCol);
        user.mazeSolution = mazeSolution;

    }
    private void GenerateRoom()
    {
        int rMax = mazeGrid.GetUpperBound(0);
        int cMax = mazeGrid.GetUpperBound(1);
        
        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                var blockType = mazeGrid[i, j];
                
                if (blockType == 1)
                {
                    tileMapWall.SetTile(new Vector3Int(i, j, 0),wallPrefab);
                }
                else
                {
                    tileMapFloor.SetTile(new Vector3Int(i, j, 0),floorPrefab);
                }

            }
            
        }
    }
    private void PlaceGoalTrigger()
    {
        GameObject endPortalCreated = (GameObject) Instantiate(endPortal, new Vector3(goalRow+.5f, goalCol+.5f, 0), Quaternion.identity);
        endPortalCreated.name = "Portal";
        endPortalCreated.tag = "Generated";

    }





}
