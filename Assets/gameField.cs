using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameField : MonoBehaviour
{
    public static gameField instance;
    public gameBlock[][] blocks;
    public Vector2Int size;
    public Material[] colors;

    bool moved = false;
    public bool isMoved
    {
        get { return moved; }
    }

    [SerializeField]
    GameObject blockObj;

    void Awake()
    {
        instance = this;
    }

    public bool isInside(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.y >= 0 && pos.x < size.x && pos.y < size.y);
    }

    public void init()
    {
        blocks = new gameBlock[size.x][];
        for (int x = 0; x < blocks.Length; x++)
            blocks[x] = new gameBlock[size.x];
        for (int x = 0; x < size.x; x++)
        {
            makeBlock(-12, new Vector2Int(x, 0));
            makeBlock(-12, new Vector2Int(x, size.x - 1));
        }
        for (int y = 1; y < size.x - 1; y++)
        {
            makeBlock(-12, new Vector2Int(0, y));
            makeBlock(-12, new Vector2Int(size.x - 1, y));
        }
        /*
        makeBlock(5, new Vector2Int(1, 3));
        makeBlock(5, new Vector2Int(2, 3));
        makeBlock(5, new Vector2Int(3, 3));
        makeBlock(5, new Vector2Int(4, 3));
        */
        makeGoal();
    }

    public void makeGoal()
    {
        ReplaceBlock(Random.Range(-1,-11), new Vector2Int(3, 6));
    }

    public void turnEnd()
    {
        for (int x = 0; x < blocks.Length; x++)
            for (int y = 0; y < blocks[x].Length; y++)
                if (blocks[x][y] != null)
                    blocks[x][y].addAge();
        moved = false;
    }

    public void makeBlock(int number, Vector2Int pos)
    {
        blocks[pos.x][pos.y] = Instantiate(blockObj, (Vector2)pos, Quaternion.identity)
            .GetComponent<gameBlock>();
        blocks[pos.x][pos.y].init(number, pos);
        blocks[pos.x][pos.y].addAge();
    }

    public void moveBlock(int dir)
    {
        switch (dir)
        {
            case -1: //right
                for (int x = 0; x < blocks.Length - 1; x++)
                    for (int y = 0; y < blocks[x].Length; y++)
                        moveBlockSub(new Vector2Int(x, y), -Vector2Int.right);
                break;
            case 2: //up
                for (int y = blocks.Length - 2; y >= 0; y--)
                    for (int x = 0; x < blocks[y].Length; x++)
                        moveBlockSub(new Vector2Int(x, y), Vector2Int.up);
                break;
            case 1: //left
                for (int x = blocks.Length - 1; x >= 1; x--)
                    for (int y = 0; y < blocks[x].Length; y++)
                        moveBlockSub(new Vector2Int(x, y), Vector2Int.right);
                break;
            case -2: //down
                for (int y = 0; y < blocks.Length - 1; y++)
                    for (int x = 0; x < blocks[y].Length; x++)
                        moveBlockSub(new Vector2Int(x, y), -Vector2Int.up);
                break;
            default:
                break;
        }
    }

    public void moveBlockSub(Vector2Int pos, Vector2Int dir)
    {
        if (blocks[pos.x][pos.y] == null || blocks[pos.x][pos.y].getNum == -1)
            return;
        var curBlock = blocks[pos.x][pos.y];
        Vector2Int newPos = curBlock.move(dir);
        if (newPos != pos)
        {
            blocks[newPos.x][newPos.y] = curBlock;
            blocks[pos.x][pos.y] = null;
            if (!moved)
                moved = true;
        }
        return;
    }

    public void deleteBlock(Vector2Int pos)
    {
        blocks[pos.x][pos.y].dest();
    }

    public void ReplaceBlock(int n, Vector2Int pos)
    {
        deleteBlock(pos);
        makeBlock(n, pos);
    }

    public TStuck<Vector2Int> getEmpty()
    {
        TStuck<Vector2Int> result = new TStuck<Vector2Int>();
        for (int x = 0; x < blocks.Length; x++)
            for (int y = 0; y < blocks[x].Length; y++)
                if (blocks[x][y] == null)
                    result.push(new Vector2Int(x, y));
        return result;
    }

    public gameBlock getBlock(Vector2Int pos)
    {
        var targBl = blocks[pos.x][pos.y];
        return targBl;
    }

    public void dispBlock()
    {
        string str = "";
        for (int y = 0; y < blocks.Length; y++)
        {
            for (int x = 0; x < blocks[y].Length; x++)
            {
                if (blocks[x][y] == null)
                    str += "0,";
                else
                    str += blocks[x][y].getNum + ",";
            }
            str += "\n";
        }
        Debug.Log(str);
    }
}
