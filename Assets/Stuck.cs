using UnityEngine;

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
