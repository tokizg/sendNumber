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

[System.Serializable]
public class blockPlace : gameItem
{
    public override void use() {
        game.inst.curMode = mode.selectPos;
        game.inst.StartCoroutine(enumerator());
        Debug.Log("nuwa");
        return;
    }

    IEnumerator enumerator()
    {
        yield return new WaitUntil(() => game.inst.curMode == mode.play);
        gameField.inst.makeBlock(0, game.inst.selectedPosition, blockType.wall, 6);
        yield return 0;
    }
}
