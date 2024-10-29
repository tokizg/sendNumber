using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

//1.インターフェースに「IPointerClickHandler」を追加します
public class hiperLink : MonoBehaviour
{
    const string URL = "https://unityroom.com/games/sendnumberr";

    public void OnPointerClick()
    {
        Application.OpenURL(URL); //""の中には開きたいWebページのURLを入力します
    }
}
