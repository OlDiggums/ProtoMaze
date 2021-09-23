using System.Collections;
using System.Collections.Generic;

public class MazePoint
{
    public int x;
    public int y;
    public int f;
    public int g;
    public int h;
    public List<MazePoint> neighbors;
    public bool visited;
    public List<bool> walls;

    public MazePoint(int xPos,int yPos)
    {
        x = xPos;
        y = yPos;

        f = 0;
        g = 0;
        h = 0;
        neighbors = new List<MazePoint>();
        visited = false;
        walls = new List<bool>{true,true,true,true};;
    }
    public void add_neighbors(MazePoint[,] inpMaze)
    {
        int rows = inpMaze.GetLength(0);
        int cols = inpMaze.GetLength(1);

        if (x > 0)
        {
            neighbors.Add(inpMaze[x - 1,y]);
        }

        if (y > 0)
        {
            neighbors.Add(inpMaze[x,y - 1]);
        }

        if (x < rows - 1)
        {
            neighbors.Add(inpMaze[x + 1,y]);
        }

        if (y < cols - 1)
        {
            neighbors.Add(inpMaze[x,y + 1]);
        }
    }


}
