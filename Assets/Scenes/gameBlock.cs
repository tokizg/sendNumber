using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameBlock : MonoBehaviour
{
    [SerializeField]
    int Number;
    public int getNum
    {
        get { return Number; }
    }
    public Vector2Int Position;

    public void init(int n, Vector2Int p)
    {
        Number = n;
        Position = p;
    }

    // Start is called before the first frame update
    void Start() { }

    public Vector2Int move(Vector2Int dir)
    {
        Vector2Int futurePos = Position + dir;
        if (!gameField.instance.isInside(futurePos))
            return Position;
        int futBlNum = gameField.instance.getBlock(futurePos);
        if (futBlNum == 0)
        {
            Position = futurePos;
            move(dir);
        }
        return Position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)Position;
    }
}
