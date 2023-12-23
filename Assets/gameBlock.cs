using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameBlock : MonoBehaviour
{
    int color = 1;

    [SerializeField]
    int Number;
    SpriteRenderer spr;

    [SerializeField]
    GameObject trail;

    [SerializeField]
    int Age = 0;
    public int getNum
    {
        get { return Number; }
    }
    public int getAge
    {
        get { return Age; }
    }
    public Vector2Int Position;

    [SerializeField]
    TextMeshPro text;

    public void init(int n, Vector2Int p)
    {
        if (spr == null)
            spr = GetComponent<SpriteRenderer>();

        Number = n;
        Position = p;
        Age = 0;

        color = n;
        Draw();
    }

    public void addAge()
    {
        Age++;
    }

    // Start is called before the first frame update
    void Start() { }

    public Vector2Int move(Vector2Int dir)
    {
        Vector2Int futurePos = Position + dir;
        if (!gameField.inst.isInside(futurePos))
            return Position;
        gameBlock futBlNum = gameField.inst.getBlock(futurePos);
        if (futBlNum == null)
        {
            Position = futurePos;
            move(dir);
        }
        else if (futBlNum.getNum == this.Number && futBlNum.getAge > 0)
        {
            init(Number + 1, Position);
            gameField.inst.deleteBlock(futurePos);
            Position = futurePos;
        }
        else if (futBlNum.getNum == -this.Number)
        {
            gameField.inst.deleteBlock(futurePos);
            Position = futurePos;
        }
        Draw();
        if (futurePos - dir != Position)
            DrawTrail(dir);

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

    void Draw()
    {
        int n = Number;
        if (n < 0)
            n = -n;
        string SpriteText = Mathf.Pow(2, n).ToString();
        text.text = SPM.convSpr(SpriteText);
        if (Number < 0)
            this.spr.material = gameField.inst.colors[0];
        else
        {
            this.spr.material = gameField.inst.colors[
                (n >= gameField.inst.colors.Length) ? gameField.inst.colors.Length - 1 : n
            ];
            text.color = gameField.inst.colors[
                (n >= gameField.inst.colors.Length) ? gameField.inst.colors.Length - 1 : n
            ].color;
        }
        transform.position = (Vector2)Position;
        /*
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
        */
    }

    public void DrawTrail(Vector2Int dir)
    {
        GameObject obj = Instantiate(trail);
        obj.transform.position = (Vector2)(Position - dir);
        obj.transform.right = -(Vector2)dir;
        Destroy(obj, 0.4f);
    }
}
