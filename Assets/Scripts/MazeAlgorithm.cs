using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MazeAlgorithm
{
    public static int rows;
    public static int columns;
    public int[,] maze
    {
        get; private set;
    }
    public char[,] mazeSolution
    {
        get; private set;
    }

    public MazeAlgorithm(int inpRows, int inpCols)
    {
        rows = inpRows;
        columns = inpCols;
    }

    public int[,] ResetMaze()
    {
        MazePoint[,] gridded_maze = GenerateBlankMaze();
        gridded_maze = AddNeighborsToBlankMaze(gridded_maze);
        gridded_maze = GeneratePathways(gridded_maze);
        maze = ConvertMazeToArray(gridded_maze);

        return maze;
    }

    public char[,] GenerateSolution(int goal_x, int goal_y)
    {
        int r = 2 * rows + 1;
        int c = 2 * columns + 1;
        int cost_step = 1;
        int[,] delta = new int[4,2] {{-1,0},{0,-1},{1,0},{0,1}}; //up, left, down, right
        char[] delta_names = new char[4] {'^','<','v','>'};
        int[,] value = CreateIntArrayWithValue(9999);
        mazeSolution = CreateCharArrayWithValue(' ');

        bool change = true;
        while (change)
        {
            change = false;
            for (int x = 0; x < r; x++)
            {
                for (int y = 0; y < c; y++)
                {
                    if (goal_x==x & goal_y==y)
                    {
                        if (value[x, y] > 0)
                        {
                            value[x, y] = 0;
                            mazeSolution[x, y] = '*';
                            change = true;
                        }
                    }else if(maze[x,y]==0)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            var x2 = x + delta[i,0];
                            var y2 = y + delta[i, 1];

                            if (x2>=0 & x2<r & y2>=0 & y2<c & maze[x2,y2]==0)
                            {
                                var v2 = value[x2, y2] + cost_step;
                                if (v2<value[x,y])
                                {
                                    change = true;
                                    value[x, y] = v2;
                                    mazeSolution[x, y] = delta_names[i];
                                }
                            }
                        }
                    }else if (maze[x, y] == 1)
                    {
                        mazeSolution[x, y] = '0';
                    }
                }
                
            }
        }

        return mazeSolution;
    }

    private static MazePoint[,] GeneratePathways(MazePoint[,] griddedMaze)
    {
        bool done = false;
        bool completed = false;
        int randomx = Random.Range(0, rows - 1);
        int randomy = Random.Range(0, columns - 1);
        MazePoint current = griddedMaze[randomx, randomy];
        Stack<MazePoint> visited = new Stack<MazePoint>();
        visited.Push(current);


        while (!done)
        {
            if (!completed)
            {
                griddedMaze[current.x, current.y].visited = true;
                bool gotNew = false;
                int temp = 10;

                while (!gotNew && !completed)
                {
                    int r = Random.Range(0, current.neighbors.Count);
                    MazePoint tempcurrent = current.neighbors[r];

                    if (!tempcurrent.visited)
                    {
                        visited.Push(current);
                        current = tempcurrent;
                        gotNew = true;
                    }

                    if (temp == 0)
                    {
                        temp = 10;
                        if (visited.Count == 0)
                        {
                            completed = true;
                            break;
                        }
                        else
                        {
                            current = visited.Pop();
                        }

                    }

                    temp -= 1;

                }

                if (!completed)
                {
                    griddedMaze = BreakWalls(griddedMaze, current, visited.Peek());
                }

                current.visited = true;
            }
            else
            {
                done = true;
            }
        }

        return griddedMaze;

    }

    private static MazePoint[,] BreakWalls(MazePoint[,] inpMaze, MazePoint a, MazePoint b)
    {
        if (a.y == b.y && a.x > b.x) {
            inpMaze[b.x,b.y].walls[1] = false;
            inpMaze[a.x,a.y].walls[3] = false;
        }
	    
        if(a.y==b.y && a.x<b.x) {
            inpMaze[a.x,a.y].walls[1] = false;
            inpMaze[b.x,b.y].walls[3] = false;
        }
	    
        if(a.x==b.x && a.y<b.y) {
            inpMaze[b.x,b.y].walls[0] = false;
            inpMaze[a.x,a.y].walls[2] = false;
        }
	    
        if(a.x==b.x && a.y>b.y) {
            inpMaze[a.x,a.y].walls[0] = false;
            inpMaze[b.x,b.y].walls[2] = false;
        }

        return inpMaze;
    }

    private static MazePoint[,] AddNeighborsToBlankMaze(MazePoint[,] inpMaze)
    {
        for(int i = 0; i<rows; i++) {
            for(int j = 0; j<columns;j++) {
                inpMaze[i,j].add_neighbors(inpMaze);
            }
        }

        return inpMaze;
    }

    private static MazePoint[,] GenerateBlankMaze()
    {
        MazePoint[,] outMaze = new MazePoint[rows,columns];
        for(int i = 0; i<rows; i++) {
            for(int j = 0; j<columns;j++) {
                outMaze[i,j] = new MazePoint(i,j);
            }
        }	
        return outMaze;        
    }

    private static int[,] ConvertMazeToArray(MazePoint[,] inp_maze)
    {
        int r = 2 * rows + 1;
        int c = 2 * columns + 1;
        int[,] arrayMaze = new int[r, c];

        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                arrayMaze[i, j] = 1;
            }
        }

        int i_dot = 0;
        int j_dot = 0;
        for (int i = 1; i < r; i+=2)
        {
            j_dot = 0;
            for (int j = 1; j < c; j+=2)
            {
                arrayMaze[i, j] = 0;
                if (!inp_maze[i_dot, j_dot].walls[0])
                {
                    arrayMaze[i, j-1] = 0;
                }
                
                if (!inp_maze[i_dot, j_dot].walls[1])
                {
                    arrayMaze[i+1, j] = 0;
                }
                
                if (!inp_maze[i_dot, j_dot].walls[2])
                {
                    arrayMaze[i, j+1] = 0;
                }
                
                if (!inp_maze[i_dot, j_dot].walls[3])
                {
                    arrayMaze[i-1, j] = 0;
                }
                


                j_dot += 1;
            }

            i_dot += 1;
        }
        
        
        


        return arrayMaze;
    }

    private static int[,] CreateIntArrayWithValue(int value)
    {
        int r = 2 * rows + 1;
        int c = 2 * columns + 1;
        int[,] simplearray = new int[r,c];

        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                simplearray[i, j] = value;
            }
            
        }

        return simplearray;
    }
    
    private static char[,] CreateCharArrayWithValue(char value)
    {
        int r = 2 * rows + 1;
        int c = 2 * columns + 1;
        char[,] simplearray = new char[r,c];

        for (int i = 0; i < r; i++)
        {
            for (int j = 0; j < c; j++)
            {
                simplearray[i, j] = value;
            }
            
        }

        return simplearray;
    }
}
