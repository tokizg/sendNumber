using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game : MonoBehaviour
{
    public static game inst;

    void Awake()
    {
        inst = this;
    }

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
        int x_ipt = (int)Input.GetAxisRaw("Horizontal"),
            y_ipt = (int)Input.GetAxisRaw("Vertical"),
            dir = 0;
        if (Input.GetButtonDown("Horizontal"))
            dir = 1 * x_ipt;
        else if (Input.GetButtonDown("Vertical"))
            dir = 2 * y_ipt;
        if (dir != 0)
            gameField.inst.moveBlock(dir);
        else
            return;
        if (gameField.inst.isMoved)
        {
            gameField.inst.turnEnd();
            makeRandomBlock();
        }
    }

    void makeRandomBlock()
    {
        TStuck<Vector2Int> stuck = new TStuck<Vector2Int>();
        Vector2Int newBlPos;
        stuck = gameField.inst.getEmpty();
        if (stuck.Length > 0)
        {
            int r = Random.Range(0, stuck.Length),
                n = 1;
            newBlPos = stuck.get(r);

            gameField.inst.makeBlock(n, newBlPos);
        }
    }

    public void goal(int n)
    {
        score += (int)Mathf.Pow(2, n);
        gameField.inst.makeGoal();
        scoreBoard.inst.Draw();
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
