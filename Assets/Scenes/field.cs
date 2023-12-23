/*using UnityEngine;
public class field : MonoBehaviour
{
    public static field inst;

    [SerializeField]
    Vector2Int size;

    block[][] blocks;

    [SerializeField]
    GameObject BlockObj;

    public int getBlock(Vector2Int pos)
    {
        int result;
        if (blocks[pos.x][pos.y] == null)
            result = 0;
        else
            result = blocks[pos.x][pos.y].num;
        return result;
    }

    public void Start()
    {
        inst = this;
        blocks = new block[size.x][];
        for (int x = 0; x < blocks.Length; x++)
            blocks[x] = new block[size.y];
        makeBlock(new Vector2Int(0, 0), 2);
        makeBlock(new Vector2Int(3, 0), 2);
    }

    public void Update()
    {
        int x_input = (int)Input.GetAxis("Horizontal");
        int y_input = (int)Input.GetAxis("Vertical");
        Vector2Int dir = Vector2Int.zero;
        if (x_input != 0)
            dir = Vector2Int.right * x_input;
        else if (y_input != 0)
            dir = Vector2Int.up * y_input;
        Debug.Log(dir);
        if (dir == Vector2Int.zero)
            return;

        for (int x = 0; x < blocks.Length; x++)
            for (int y = 0; y < blocks[x].Length; y++)
            {
                if (blocks[x][y] == null)
                    continue;
                blocks[x][y].move(dir);
            }
    }

    public void makeBlock(Vector2Int pos, int num)
    {
        blocks[pos.x][pos.y] = Instantiate(BlockObj, (Vector2)pos, Quaternion.identity)
            .GetComponent<block>();
        blocks[pos.x][pos.y].position = pos;
    }

    public void mergeBlock(Vector2Int pos)
    {
        blocks[pos.x][pos.y].upGrade();
    }

    public void deleteBlock(Vector2Int pos)
    {
        blocks[pos.x][pos.y] = null;
        return;
    }

    public bool isInside(Vector2Int pos)
    {
        return (pos.x >= 0 && pos.y >= 0 && pos.x < size.x && pos.y < size.y);
    }
}
*/