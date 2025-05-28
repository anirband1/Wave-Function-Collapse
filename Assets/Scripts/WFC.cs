using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WFC
{
    public static bool isCollapsed = false;

    static List<int>[,] grid;
    static int gridSize;

    static int debugCount = 0;

    public static List<int>[,] StartWFC(int gridSize, RoadTile[] roadTiles)
    {
        WFC.gridSize = gridSize;
        grid = Populate(gridSize, roadTiles.Length);

        while (!isCollapsed) Iterate(roadTiles);

        return grid;
    }

    static List<int>[,] Populate(int gridSize, int numTiles)
    {
        List<int>[,] _grid = new List<int>[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                _grid[x, y] = Enumerable.Range(0, numTiles).ToList(); ;
            }
        }

        return _grid;
    }

    static void Iterate(RoadTile[] roadTiles)
    {
        Vector2Int lowestEntropyCell = GetMinEntropyCoords();
        Collapse(lowestEntropyCell);
        Propagate(lowestEntropyCell, roadTiles);

        isCollapsed = isGlobalCollapsed();
    }

    static Vector2Int GetMinEntropyCoords()
    {
        int count = int.MaxValue;
        Vector2Int lowestEntropyCell = new Vector2Int();
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i, j].Count < count && grid[i, j].Count > 1)
                {
                    count = grid[i, j].Count;
                    lowestEntropyCell = new Vector2Int(i, j);
                }
            }
        }

        return lowestEntropyCell;
    }

    static bool isGlobalCollapsed()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (grid[i, j].Count != 1)
                {
                    return false;
                }
            }
        }
        return true;
    }

    static void Collapse(Vector2Int coords)
    {
        List<int> coordList = grid[coords.x, coords.y];
        int temp = Random.Range(0, coordList.Count - 1);
        int val = coordList[temp];

        coordList.Clear();
        coordList.Add(val);
    }

    static void Propagate(Vector2Int initCoords, RoadTile[] roadTiles)
    {
        bool BoundaryCheck(Vector2Int[] neighbours, int index)
        {
            // outside bounds
            return neighbours[index].x >= 0 && neighbours[index].x < gridSize && neighbours[index].y >= 0 && neighbours[index].y < gridSize;
        }

        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        stack.Push(initCoords);

        while (stack.Count > 0)
        {
            Vector2Int coords = stack.Pop();

            Vector2Int[] neighbours = new Vector2Int[4];
            neighbours[0] = new Vector2Int(coords.x + 1, coords.y);
            neighbours[1] = new Vector2Int(coords.x - 1, coords.y);
            neighbours[2] = new Vector2Int(coords.x, coords.y + 1);
            neighbours[3] = new Vector2Int(coords.x, coords.y - 1);

            for (int i = 0; i < 4; i++)
            {
                if (BoundaryCheck(neighbours, i))
                {
                    // * get list of possible tiles which go in this neighbour cell
                    List<int> possibleTiles = new List<int>();
                    foreach (int tile in grid[coords.x, coords.y])
                    {
                        possibleTiles.AddRange(roadTiles[tile].validNeighbour[i]);
                        possibleTiles = possibleTiles.Distinct().ToList();
                    }

                    // * check against the existigng neighbour possibilities
                    List<int> otherTiles = grid[neighbours[i].x, neighbours[i].y];
                    if (Constrain(possibleTiles, otherTiles)) stack.Push(neighbours[i]);

                }
            }

        }
    }


    static bool Constrain(List<int> thisList, List<int> otherList)
    {
        // * Any proto not valid is removed from superposition
        List<int> copyList = new List<int>(otherList);
        bool hasChanged = false;

        foreach (int possibleTile in copyList)
        {
            if (!thisList.Contains(possibleTile))
            {
                otherList.Remove(possibleTile);
                hasChanged = true;
            }
        }

        return hasChanged; // return if tile has changed superposition
    }

    static void DebugPrint()
    {
        int[] tileCount = new int[12];
        for (int x = 0; x < gridSize; x++) for (int y = 0; y < gridSize; y++)
            {
                tileCount[grid[x, y].Count - 1]++;
                if (grid[x, y].Count < 12)
                {
                    Debug.Log(grid[x, y].Count + " -- " + x + " " + y);
                    // Debug.Log(
                    //     grid[x, y][0].ToString() + " " +
                    //     grid[x, y][1].ToString() + " " +
                    //     grid[x, y][2].ToString() + " " +
                    //     grid[x, y][3].ToString() + " " +
                    //     grid[x, y][4].ToString() + " " +
                    //     grid[x, y][5].ToString() + " " +
                    //     grid[x, y][6].ToString() + " "
                    // );
                }
            }


        Debug.Log(
            "\n" +
            "1 : " + tileCount[0] + "\n" +
            "2 : " + tileCount[1] + "\n" +
            "3 : " + tileCount[2] + "\n" +
            "4 : " + tileCount[3] + "\n" +
            "5 : " + tileCount[4] + "\n" +
            "6 : " + tileCount[5] + "\n" +
            "7 : " + tileCount[6] + "\n" +
            "8 : " + tileCount[7] + "\n" +
            "9 : " + tileCount[8] + "\n" +
            "10 : " + tileCount[9] + "\n" +
            "11 : " + tileCount[10] + "\n" +
            "12 : " + tileCount[11] + "\n"
        );

    }

    static void DebugPrintList(Vector2Int coords)
    {
        string dbugstr = "";
        for (int i = 0; i < grid[coords.x, coords.y].Count; i++)
        {
            dbugstr += grid[coords.x, coords.y][i].ToString() + ", ";
        }

        Debug.Log(dbugstr);
    }

}

public struct RoadTile
{
    public short rotY;
    public int[][] validNeighbour;

    public RoadTile(short _rotY, int[][] _validNeighbours)
    {
        rotY = _rotY;
        validNeighbour = _validNeighbours;
    }
}
