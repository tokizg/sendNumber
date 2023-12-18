using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameBlock : MonoBehaviour
{
    int color = 1;

    [SerializeField]
    int Number;
    SpriteRenderer spr;
    public int getNum
    {
        get { return Number; }
    }
    public Vector2Int Position;

    [SerializeField]
    TextMesh text;

    public void init(int n, Vector2Int p)
    {
        Number = n;
        Position = p;
    }

    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        if (Number == -1)
            this.spr.material = gameField.instance.colors[0];
        else
            this.spr.material = gameField.instance.colors[color];
    }

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
        else if (futBlNum == this.Number)
        {
            this.Number *= 2;
            color++;
            this.spr.material = gameField.instance.colors[
                (color < gameField.instance.colors.Length)
                    ? color
                    : gameField.instance.colors.Length - 1
            ];
            gameField.instance.deleteBlock(futurePos);
            Position = futurePos;
        }

        return Position;
    }

    public void dest()
    {
        Destroy(gameObject);
    }

    public Vector2 startPos;
    public float lerpStarted;
    public float distance;
    public float lerpSpeed = 16f;

    void Update()
    {
        text.text = Number.ToString();
        if (
            Time.time - lerpStarted < distance
            && (Vector2)transform.position - (Vector2)Position != Vector2.zero
        )
        {
            transform.position = Vector2.Lerp(
                startPos,
                (Vector2)Position,
                (Time.time - lerpStarted) / (distance / lerpSpeed)
            );
            if ((Time.time - lerpStarted) / (distance / lerpSpeed) > 1)
                transform.position = (Vector2)Position;
        }
        else if ((Vector2)transform.position - (Vector2)Position != Vector2.zero)
        {
            startPos = transform.position;
            lerpStarted = Time.time;
            distance = ((Vector2)transform.position - (Vector2)Position).magnitude;
        }
    }
}
