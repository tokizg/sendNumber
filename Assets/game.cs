using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum mode
{
    play,
    selectPos,
    pause
};

public class game : MonoBehaviour
{
    public readonly int LFT_INFINITY = -1;

    public static game inst;

    public Sprite[] blockSpr;

    public static gameItem[] items = { new blockPlace() };
    public Sprite[] itemSpr;
    public string[] descs = { "していした いちに かべを せっちする.\nかべは ３ターンで しょうめつ する" };

    public int selectedSlot;

    [SerializeField]
    Transform selector;

    [SerializeField]
    Transform posSelector;
    public int maxItemCount;
    public List<int> itemInventory;
    public List<SpriteRenderer> sprrdr;

    [SerializeField]
    TextMeshPro itemDesc;

    public mode curMode = mode.play;

    void Awake()
    {
        inst = this;
    }

    public Vector2Int selectedPosition;

    [SerializeField]
    int score = 0;
    public int getScore
    {
        get { return score; }
    }

    void Start()
    {
        gameField.inst.init();
        makeRandomBlock();
        makeRandomBlock();
        goal(0);
    }

    // Update is called once per frame
    void Update()
    {
        switch (curMode)
        {
            case mode.play:
                posSelector.position = Vector2.up * 100; //果ての Fry Away
                int x_ipt = (int)Input.GetAxisRaw("Horizontal"),
                    y_ipt = (int)Input.GetAxisRaw("Vertical"),
                    dir = 0;
                if (Input.GetButtonDown("Horizontal"))
                    dir = 1 * x_ipt;
                else if (Input.GetButtonDown("Vertical"))
                    dir = 2 * y_ipt;
                else if (Input.GetButtonDown("changeSlot"))
                {
                    selectedSlot = Mathf.Clamp(
                        selectedSlot + (int)Input.GetAxisRaw("changeSlot"),
                        0,
                        itemInventory.Count - 1
                    );
                }
                else if (Input.GetButtonDown("useItem") && itemInventory.Count > 0)
                {
                    gameItem curItem = items[itemInventory[selectedSlot]];
                    curItem.use();
                    itemInventory.RemoveAt(selectedSlot);
                    selectedSlot = Mathf.Clamp(selectedSlot, 0, itemInventory.Count - 1);
                }
                else if (Input.GetButtonDown("destItem") && itemInventory.Count > 0)
                {
                    itemInventory.RemoveAt(selectedSlot);
                    selectedSlot = Mathf.Clamp(selectedSlot, 0, itemInventory.Count - 1);
                }
                if (dir != 0)
                {
                    gameField.inst.moveBlock(dir);
                    if (gameField.inst.isMoved)
                    {
                        gameField.inst.turnEnd();
                        makeRandomBlock();
                    }
                }
                break;
            case mode.selectPos:
                Vector3 mousePosition = Input.mousePosition;
                Vector2 worldPosition =
                    (Vector2)Camera.main.ScreenToWorldPoint(mousePosition) + Vector2.one * 0.5f;
                var ret = new Vector2Int((int)worldPosition.x, (int)worldPosition.y);
                posSelector.position = (Vector2)ret;
                if (Input.GetMouseButtonDown(0))
                {
                    if (gameField.inst.isInside(ret) && gameField.inst.blocks[ret.x][ret.y] == null)
                    {
                        selectedPosition = ret;
                        curMode = mode.play;
                    }
                }
                break;
        }
        DrawInv();
        return;
    }

    public void openChest()
    {
        if (itemInventory.Count < maxItemCount)
        {
            itemInventory.Add(Random.Range(0, items.Length - 1));
            soundManager.inst.ad_getItem();
        }
    }

    void makeRandomBlock()
    {
        TStuck<Vector2Int> stuck = new TStuck<Vector2Int>();
        Vector2Int newBlPos;
        stuck = gameField.inst.getEmpty();
        int k = Random.Range(0, 20);
        if (stuck.Length > 0)
        {
            int r = Random.Range(0, stuck.Length),
                n = Random.Range(1, 2);
            newBlPos = stuck.get(r);

            if (k == 19)
                gameField.inst.makeBlock(
                    Random.Range(1, 4),
                    newBlPos,
                    blockType.chest,
                    Random.Range(4, 11)
                );
            else
                gameField.inst.makeBlock(n, newBlPos, blockType.normal, LFT_INFINITY);
        }
    }

    public void goal(int n)
    {
        score += (int)Mathf.Pow(2, n);
        gameField.inst.makeGoal();
        scoreBoard.inst.Draw();
    }

    public void DrawInv()
    {
        if (itemInventory.Count == 0)
        {
            selectedSlot = 0;
            selector.position = Vector2.up * 100; //果ての FRY AWAY
            itemDesc.text = "<color=#777777>あいてむを もっていない";
        }
        else
        {
            selector.position = (Vector2)new Vector2Int(8 + selectedSlot, 3); //深夜テンション Hard Cording
            itemDesc.text = descs[itemInventory[selectedSlot]];
        }
        int i = 0;
        while (i < maxItemCount)
        {
            if (i < itemInventory.Count)
                sprrdr[i].sprite = itemSpr[itemInventory[i]];
            else
                sprrdr[i].sprite = null;
            i++;
        }
    }
}

public class TStuck<T>
{
    T[] elt;

    public TStuck()
    {
        elt = new T[0];
    }

    public void push(T addElt)
    {
        T[] newElts = new T[elt.Length + 1];
        for (int i = 0; i < elt.Length; i++)
            newElts[i] = elt[i];
        newElts[newElts.Length - 1] = addElt;
        elt = newElts;
    }

    public T get(int idx)
    {
        return elt[idx];
    }

    public int Length
    {
        get { return elt.Length; }
    }

    public void dispTStuck()
    {
        Debug.Log("--- Display Elements:");

        for (int i = 0; i < elt.Length; i++)
            Debug.Log(elt[i]);

        Debug.Log("END --- ");
    }
}

public class SPM
{
    public static string convSpr(string str)
    {
        string ret = "";
        for (int i = 0; i <= str.Length - 1; i++)
        {
            switch (str[i])
            {
                case ' ':
                    ret += " ";
                    break;
                default:
                    ret += "<sprite=" + (System.Convert.ToInt32(str[i]) - 48) + ">";

                    break;
            }
        }
        return ret;
    }
}
