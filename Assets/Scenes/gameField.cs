using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameField : MonoBehaviour
{
    public static gameField instance;
    public gameBlock[][] blocks;
    public Vector2Int size;

    [SerializeField]
    GameObject blockObj;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        blocks = new gameBlock[size.x][];
        for (int x = 0; x < blocks.Length; x++)
            blocks[x] = new gameBlock[size.x];
        makeBlock(new Vector2Int(0, 0));
        makeBlock(new Vector2Int(3, 0));

        makeBlock(new Vector2Int(3, 1));

        makeBlock(new Vector2Int(2, 0));
        makeBlock(new Vector2Int(2, 2));
    }

    void makeBlock(Vector2Int pos)
    {
        blocks[pos.x][pos.y] = Instantiate(blockObj, (Vector2)pos, Quaternion.identity)
            .GetComponent<gameBlock>();
        blocks[pos.x][pos.y].init(2, pos);
    }

    public void moveBlock(int dir)
    {
        dispBlock();
        switch (dir)
        {
            case 0: //rigtht
                for (int x = 0; x < blocks.Length; x++)
                    for (int y = 0; y < blocks[x].Length; y++)
                        if (blocks[x][y] == null)
                            continue;
                        else
                            moveBlockSub(new Vector2Int(x, y), -Vector2Int.right);
                break;
            case 1: //up
                break;
            case 2: //left
                for (int x = blocks.Length - 1; x >= 0; x--)
                    for (int y = 0; y < blocks[x].Length; y++)
                        if (blocks[x][y] == null)
                            continue;
                        else
                            moveBlockSub(new Vector2Int(x, y), Vector2Int.right);
                break;
            case 3: //down
                break;
            default:
                break;
        }
        dispBlock();
    }

    public void moveBlockSub(Vector2Int pos, Vector2Int dir)
    {
        var curBlock = blocks[pos.x][pos.y];
        Vector2Int newPos = curBlock.move(dir);
        if (newPos != pos)
        {
            blocks[newPos.x][newPos.y] = curBlock;
            blocks[pos.x][pos.y] = null;
        }
        return;
    }

    public int getBlock(Vector2Int pos)
    {
        var targBl = blocks[pos.x][pos.y];
        if (targBl == null)
            return 0;
        return targBl.getNum;
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

    public bool isInside(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.y >= 0 && pos.x < size.x && pos.y < size.y);
    }

    // Update is called once per frame
    void Update() { }
}
