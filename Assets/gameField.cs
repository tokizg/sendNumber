using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameField : MonoBehaviour
{
    public static gameField inst;
    public gameBlock[][] blocks;
    public Vector2Int size;
    public Vector2Int goalPos;
    public Material[] colors;

    //ターンごとのフラグ
    bool moved = false;
    public bool isMoved
    {
        get { return moved; }
    }
    bool goaled = false;
    public bool isGoaled
    {
        get { return goaled; }
    }

    [SerializeField]
    GameObject blockObj;

    void Awake()
    {
        inst = this;
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
            makeBlock(0, new Vector2Int(x, 0), blockType.wall, game.inst.LFT_INFINITY);
            makeBlock(0, new Vector2Int(x, size.x - 1), blockType.wall, game.inst.LFT_INFINITY);
        }
        for (int y = 1; y < size.x - 1; y++)
        {
            makeBlock(0, new Vector2Int(0, y), blockType.wall, game.inst.LFT_INFINITY);
            makeBlock(0, new Vector2Int(size.x - 1, y), blockType.wall, game.inst.LFT_INFINITY);
        }
        makeGoal();
    }

    public void makeGoal()
    {
        ReplaceBlock(Random.Range(1, 7), goalPos, blockType.goal, game.inst.LFT_INFINITY);
    }

    public void turnEnd()
    {
        for (int x = 0; x < blocks.Length; x++)
            for (int y = 0; y < blocks[x].Length; y++)
                if (blocks[x][y] != null)
                    blocks[x][y].addAge();
        moved = false;
        goaled = false;
        if (isGameOver())
            game.inst.GameOver();
    }

    public void makeBlock(int number, Vector2Int pos, blockType tp, int lft)
    {
        blocks[pos.x][pos.y] = Instantiate(blockObj, (Vector2)pos, Quaternion.identity)
            .GetComponent<gameBlock>();
        blocks[pos.x][pos.y].init(number, pos, tp, lft);
        blocks[pos.x][pos.y].addAge();
    }

    bool isGameOver()
    {
        for (int x = 1; x < blocks.Length; x++)
            for (int y = 1; y < blocks[x].Length; y++)
                if (isGameOverSub(new Vector2Int(x, y)))
                {
                    Debug.Log("not GameOver!");

                    return false;
                }
        Debug.Log("GameOver!");
        return true;
    }

    bool isGameOverSub(Vector2Int pos)
    {
        if (canMove(pos + Vector2Int.right, pos))
            return true;
        if (canMove(pos - Vector2Int.right, pos))
            return true;
        if (canMove(pos + Vector2Int.up, pos))
            return true;
        if (canMove(pos - Vector2Int.up, pos))
            return true;
        return false;
    }

    bool canMove(Vector2Int toPos, Vector2Int fromPos)
    {
        Debug.Log(toPos);
        return blocks[fromPos.x][fromPos.y] == null
            || blocks[toPos.x][toPos.y] == null
            || getBlock(toPos).getNum == getBlock(fromPos).getNum;
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
        if (moved)
        {
            soundManager.inst.ad_move();
        }
        if (isGoaled)
        {
            soundManager.inst.ad_goal();
        }
    }

    public void moveBlockSub(Vector2Int pos, Vector2Int dir)
    {
        if (blocks[pos.x][pos.y] == null || blocks[pos.x][pos.y].getType != blockType.normal)
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
        if (newPos == goalPos)
        {
            game.inst.goal(curBlock.getNum);
            goaled = true;
        }
        return;
    }

    public void deleteBlock(Vector2Int pos)
    {
        blocks[pos.x][pos.y].dest();
    }

    public void ReplaceBlock(int n, Vector2Int pos, blockType tp, int lft)
    {
        deleteBlock(pos);
        makeBlock(n, pos, tp, lft);
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
