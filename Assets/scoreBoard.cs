using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreBoard : MonoBehaviour
{
    public static scoreBoard inst;
    [SerializeField]
    TextMeshPro score_text;

    void Awake()
    {
        inst = this;
    }

    public void Draw()
    {
        score_text.text = "スコア: "+SPM.convSpr(game.inst.getScore.ToString());
    }
}
