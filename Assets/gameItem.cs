using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class gameItem
{
    public virtual void use()
    {
        return;
    }
}

public class blockPlace : gameItem
{
    public override void use()
    {
        game.inst.curMode = mode.selectPos;
        game.inst.StartCoroutine(enumerator());
        return;
    }

    IEnumerator enumerator()
    {
        yield return new WaitUntil(() => game.inst.curMode == mode.play);
        gameField.inst.makeBlock(0, game.inst.selectedPosition, blockType.wall, 6);
        yield return 0;
    }
}

public class blockDest : gameItem
{
    public override void use()
    {
        game.inst.curMode = mode.selectBlk;
        game.inst.StartCoroutine(enumerator());
        return;
    }

    IEnumerator enumerator()
    {
        yield return new WaitUntil(() => game.inst.curMode == mode.play);
        gameField.inst.deleteBlock(game.inst.selectedPosition);
        yield return 0;
    }
}
public class blockStopper : gameItem
{
    public override void use()
    {
        game.inst.curMode = mode.selectPos;
        game.inst.StartCoroutine(enumerator());
    }
    IEnumerator enumerator()
    {
        yield return new WaitUntil(() => game.inst.curMode == mode.play);
        gameField.inst.makeBlock(0, game.inst.selectedPosition, blockType.stopper, game.inst.LFT_INFINITY);
        yield return 0;
    }
}
public class restoreGoal : gameItem
{
    public override void use()
    {
        gameField.inst.makeGoal();   
    }
}