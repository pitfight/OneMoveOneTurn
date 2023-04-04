/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public enum EdgeGridNodeType
    {
        Center,
        Left,
        Right,
        Up,
        Down
    }

    public Grid<PathNode> Grid { get; private set; }
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public PathNode cameFromNode;

    public EdgeGridNodeType edgeGrid;

    public PathNode(Grid<PathNode> grid, int x, int y) {
        this.Grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;

        if (x == grid.GetWidth() - 1) edgeGrid = EdgeGridNodeType.Right;
        else if (x == 0) edgeGrid = EdgeGridNodeType.Left;
        else if (y == grid.GetHeight() - 1) edgeGrid = EdgeGridNodeType.Up;
        else if (y == 0) edgeGrid = EdgeGridNodeType.Down;
        else edgeGrid = EdgeGridNodeType.Center;
            
    }

    public void CalculateFCost() {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        Grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() {
        return x + "," + y;
    }

    public string GetInformation()
    {
        return x + "," + y + " / Is Walkeble: " + isWalkable + " / EdgeType: " + edgeGrid;
    }

    public bool IsNode(int x, int y) => x == this.x && y == this.y;
    public bool IsNode(PathNode pathNode) => pathNode.x == this.x && pathNode.y == this.y;

}
