using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NickNameManager : MonoBehaviour
{
    private GameObject namePlate;   //　名前を表示しているプレート
    public Text nameText;   //　名前を表示するテキスト

    void Start()
    {
        namePlate = nameText.transform.parent.gameObject;
    }

 
    [PunRPC]
    void SetName(string name)
    {
        nameText.text = name;
    }
}
