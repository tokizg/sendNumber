using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game : MonoBehaviour
{


    void Start()
    {
        gameField.instance.init();
        makeRandomBlock();
        makeRandomBlock();
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
            gameField.instance.moveBlock(dir);
        else
            return;
        makeRandomBlock();
    }

    void makeRandomBlock()
    {
        TStuck<Vector2Int> stuck = new TStuck<Vector2Int>();
        Vector2Int newBlPos;
        stuck = gameField.instance.getEmpty();
        if (stuck.Length > 0)
        {
            int r = Random.Range(0, stuck.Length);
            newBlPos = stuck.get(r);
            gameField.instance.makeBlock(2, newBlPos);
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
