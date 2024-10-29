using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreBoard : MonoBehaviour
{
    public static scoreBoard inst;

    [SerializeField]
    TextMeshPro[] score_text;

    [SerializeField]
    GameObject overMenu;

    [SerializeField]
    TMP_Text scTx;

    void Awake()
    {
        inst = this;
    }

    public void Over()
    {
        overMenu.SetActive(true);
        scTx.text = "スコア: " + SPM.convSpr(game.inst.getScore.ToString());
    }

    public void Draw()
    {
        for (int i = 0; i < score_text.Length; i++)
            score_text[i].text = "スコア: " + SPM.convSpr(game.inst.getScore.ToString());
    }
}
