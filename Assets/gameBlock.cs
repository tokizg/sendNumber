using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public enum blockType
{
    normal,
    goal,
    wall,
    chest,
    stopper
}

public class gameBlock : MonoBehaviour
{
    int color = 1;

    [SerializeField]
    int Number;
    SpriteRenderer spr;

    public blockType type;
    public blockType getType
    {
        get { return type; }
    }

    [SerializeField]
    Transform wallBlock;

    [SerializeField]
    GameObject trail;

    [SerializeField]
    GameObject effectFagment;

    [SerializeField]
    GameObject dissapEffect;

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
    int lifeTime;
    public int getLft
    {
        get { return lifeTime; }
    }

    [SerializeField]
    TextMeshPro numText;

    [SerializeField]
    TextMeshPro lftText;

    public async void init(int n, Vector2Int p, blockType tp, int lft)
    {
        if (spr == null)
            spr = GetComponent<SpriteRenderer>();
        this.type = tp;

        Number = n;
        Position = p;
        Age = 0;
        lifeTime = lft;

        color = n;

        if (type == blockType.wall)
        {
            wallBlock.gameObject.SetActive(true);
        }
        else
        {
            wallBlock.gameObject.SetActive(false);
        }
        Draw();
        transform.localScale = Vector3.zero;
        while (transform.localScale.x < 1f)
        {
            transform.localScale += Vector3.one * Time.deltaTime * 10f;
            await UniTask.Yield();
        }
        transform.localScale = Vector3.one;
    }

    public async void addAge()
    {
        Age++;

        if (lifeTime != -1)
        {
            lifeTime--;
            if (lifeTime == 0)
            {
                gameField.inst.deleteBlock(this.Position);
            }
        }
        await Draw();
    }

    // Start is called before the first frame update
    void Start() { }

    public async UniTask<Vector2Int> move(Vector2Int dir)
    {
        Vector2Int futurePos = Position + dir;
        if (!gameField.inst.isInside(futurePos))
            return Position;
        gameBlock futBl = gameField.inst.getBlock(futurePos);
        if (futBl == null)
        {
            Position = futurePos;
            await move(dir);
        }
        else if (
            futBl.getType == blockType.normal && futBl.getNum == this.Number && futBl.getAge > 0
        )
        {
            init(Number + 1, Position, blockType.normal, game.inst.LFT_INFINITY);
            gameField.inst.deleteBlock(futurePos);
            Position = futurePos;
        }
        else if (
            futBl.getType == blockType.goal
            && futBl.getNum == this.Number
            && !gameField.inst.isGoaled
        )
        {
            gameField.inst.deleteBlock(futurePos);
            Position = futurePos;
        }
        else if (futBl.getType == blockType.chest && futBl.getNum == this.Number)
        {
            gameField.inst.deleteBlock(futurePos);
            Position = futurePos;
            game.inst.openChest();
        }
        else if (futBl.getType == blockType.stopper)
        {
            gameField.inst.deleteBlock(futurePos);
            Position = futurePos;
        }
        await Draw();
        if (futurePos - dir != Position)
            DrawTrail(dir);

        return Position;
    }

    public void dest()
    {
        if (this.type == blockType.normal)
        {
            GameObject obj = Instantiate(effectFagment, (Vector2)Position, Quaternion.identity);
            Destroy(obj, 3f);
        }
        else if (this.type == blockType.chest)
        {
            GameObject obj = Instantiate(dissapEffect, (Vector2)Position, Quaternion.identity);
            Destroy(obj, 0.45f);
        }
        Destroy(gameObject);
    }

    public Vector2 startPos;
    public float lerpStarted;
    public float distance;
    public float lerpSpeed = 16f;

    async UniTask Draw()
    {
        int n = Number;
        string SpriteText = Mathf.Pow(2, n).ToString();
        switch (getType)
        {
            case blockType.normal:
                numText.text = SPM.convSpr(SpriteText);
                this.spr.material = gameField.inst.colors[0];
                this.spr.material = gameField.inst.colors[
                    (n >= gameField.inst.colors.Length) ? gameField.inst.colors.Length - 1 : n
                ];
                break;
            case blockType.goal:
                numText.text = SPM.convSpr(SpriteText);
                break;
            case blockType.chest:
                spr.sprite = game.inst.blockSpr[4];
                numText.text = Mathf.Pow(2, n).ToString();
                this.spr.material = gameField.inst.colors[8];
                break;
            case blockType.stopper:
                spr.sprite = game.inst.blockSpr[5];
                break;
            default:
                numText.text = "";
                break;
        }
        if (lifeTime != -1)
        {
            lftText.text = lifeTime.ToString();
        }
        else
        {
            lftText.text = "";
        }
        transform.position = (Vector2)Position;

        return;
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
