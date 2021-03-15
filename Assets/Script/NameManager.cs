using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    [SerializeField]
    PhotonView photon;
    [SerializeField]
    InputField namtetext;
    [SerializeField]
    Text nametextspace;
    [SerializeField]
    GameObject oya;
    // Start is called before the first frame update
    void Start()
    {

        oya = transform.root.gameObject;
        nametextspace = oya.transform.Find("MyCanvas/Text").GetComponent<Text>();      
        namtetext = GetComponent<InputField>();
        photon = transform.root.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void InputText()
    {
       
      
            nametextspace.text = namtetext.text;
     
        
    }

    [PunRPC]
    public void InputTextS()
    {
      
            gameObject.SetActive(false);
      
      
    }
}
