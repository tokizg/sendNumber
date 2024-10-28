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

    [SerializeField]
    Animator anim;

    [SerializeField]
    int curLevel = 1;
    public readonly int maxLevel = 11;
    public int getLevel
    {
        get { return curLevel; }
    }

    public void levelUp()
    {
        if (curLevel < maxLevel)
        {
            curLevel++;
            soundManager.inst.ad_levelUp();
            anim.SetTrigger("level");
        }
    }

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

        makeGoal(1);
        makeBlock(1, new Vector2Int(2, 2), blockType.normal, game.inst.LFT_INFINITY);
    }

    public void makeGoal(int n = -1)
    {
        ReplaceBlock(
            n == -1 ? Random.Range(1, curLevel) : n,
            goalPos,
            blockType.goal,
            game.inst.LFT_INFINITY
        );
    }

    public void turnEnd()
    {
        for (int x = 0; x < blocks.Length; x++)
            for (int y = 0; y < blocks[x].Length; y++)
                if (blocks[x][y] != null)
                    blocks[x][y].addAge();
        moved = false;
    }

    public void makeBlock(int number, Vector2Int pos, blockType tp, int lft)
    {
        blocks[pos.x][pos.y] = Instantiate(blockObj, (Vector2)pos, Quaternion.identity)
            .GetComponent<gameBlock>();
        blocks[pos.x][pos.y].init(number, pos, tp, lft);
        blocks[pos.x][pos.y].addAge();
    }

    public bool isGameOver()
    {
        var empties = getEmpty();
        Debug.Log(empties.Length);
        if (empties.Length > 0)
            return false;
        Debug.Log("theres no empty");
        for (int x = 1; x < blocks.Length - 1; x++)
            for (int y = 1; y < blocks[x].Length - 1; y++)
            {
                if (blocks[x][y].getType == blockType.normal)
                {
                    if (canMove(x, y, x - 1, y) == true)
                        return false;
                    if (canMove(x, y, x + 1, y) == true)
                        return false;
                    if (canMove(x, y, x, y - 1) == true)
                        return false;
                    if (canMove(x, y, x, y + 1) == true)
                        return false;
                }
            }
        Debug.Log("GameOver!");
        return true;
    }

    bool canMove(int x, int y, int dx, int dy)
    {
        if (blocks[dx][dy].getNum == blocks[x][y].getNum)
            return true;
        return false;
    }

    public async void moveBlock(int dir)
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
            goaled = false;
        }
    }

    public async void moveBlockSub(Vector2Int pos, Vector2Int dir)
    {
        if (blocks[pos.x][pos.y] == null || blocks[pos.x][pos.y].getType != blockType.normal)
            return;
        var curBlock = blocks[pos.x][pos.y];
        Vector2Int newPos = await curBlock.move(dir);
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
