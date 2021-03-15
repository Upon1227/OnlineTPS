using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinManager : MonoBehaviour
{
    public Text RedCount;
    public Text BlueCount;
    public Text RedText;
    public Text BlueText;
    public PhotonView photon;
    //赤チームのキルカウント
    [PunRPC]
    void SetText(int a)
    {
        a++;
        RedCount.text = "" + a;    
    }
    //青チームのキルカウント
    [PunRPC]
    void SetText01(int b)
    {
        b++;
        BlueCount.text = "" + b;
    }
    //赤チームが勝利した時のUI表示
    [PunRPC]
    void RedWin(string value)
    {
        RedText.text = value;
    }
    //青チームが勝利した時のUI表示
    [PunRPC]
    void BlueWin(string vlue1)
    {
        BlueText.text = vlue1;
    }

}
