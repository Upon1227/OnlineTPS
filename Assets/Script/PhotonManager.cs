using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : Photon.PunBehaviour
{
    [SerializeField]
    GameObject loginUI;
    [SerializeField]
    private Dropdown roomlist;
    [SerializeField]
    private InputField roomname;
    //　ログアウトボタン
    [SerializeField]
    private GameObject logoutButton;
    //　プレイヤーの名前入力欄
    [SerializeField]
    private InputField playerName;

    void Start()
    { 
        // Photonに接続する(引数でゲームのバージョンを指定できる)
        PhotonNetwork.ConnectUsingSettings(null);
    }
    //　ロビーに入った時に呼ばれる
    public override void OnJoinedLobby()
    {
        Debug.Log("ロビーに入る");
        loginUI.SetActive(true);
    }
    //　部屋に入室した時に呼ばれるメソッド
    public override void OnJoinedRoom()
    {
        loginUI.SetActive(false);
        Debug.Log("入室");
        //　InputFieldに入力した名前を設定
        if (playerName.text != "")
        {
            PhotonNetwork.player.NickName = playerName.text;
        }
        else
        {
            PhotonNetwork.player.NickName = "DefaultPlayer";
        }
        //　InputFieldに入力した名前を設定
        PhotonNetwork.player.NickName = playerName.text;
        StartCoroutine("StePlayer", 0);
    }
    IEnumerator StePlayer(float time)
    {
        yield return new WaitForSeconds(time);
        if(PhotonNetwork.countOfPlayers == 1 || PhotonNetwork.countOfPlayers == 3)
        {
            transform.position = new Vector3(6, 1, -25);
            GameObject Player = PhotonNetwork.Instantiate("Player", transform.position, transform.rotation, 0);
            Player.name = "Player";
            Player.GetPhotonView().RPC("SetName", PhotonTargets.AllBuffered, PhotonNetwork.player.NickName);
        }
        if (PhotonNetwork.countOfPlayers == 2 || PhotonNetwork.countOfPlayers == 4)
        {
            transform.position = new Vector3(10.3f, 1.5f, -77);
            GameObject Player2 = PhotonNetwork.Instantiate("Player2", transform.position, transform.rotation, 0);
            Player2.name = "Player2";
            Player2.GetPhotonView().RPC("SetName", PhotonTargets.AllBuffered, PhotonNetwork.player.NickName);
        }
    }
    //　部屋の入室に失敗した
    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("入室に失敗");

        //　ルームオプションを設定
        RoomOptions ro = new RoomOptions()
        {
            //　ルームを見えるようにする
            IsVisible = false,
            //　部屋の入室最大人数
            MaxPlayers = 10
        };
        //　入室に失敗したらDefaultRoomを作成し入室
        PhotonNetwork.JoinOrCreateRoom("DefaultRoom", ro, TypedLobby.Default);
    }
    public override void OnReceivedRoomListUpdate()
    {
        Debug.Log("部屋更新");
        //　部屋情報を取得する
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        //　ドロップダウンリストに追加する文字列用のリストを作成
        List<string> list = new List<string>();

        //　部屋情報を部屋リストに表示
        foreach (RoomInfo room in rooms)
        {
            //　部屋が満員でなければ追加
            if (room.PlayerCount < room.MaxPlayers)
            {
                list.Add(room.Name);
            }
        }

        //　ドロップダウンリストをリセット
        roomlist.ClearOptions();

        //　部屋が１つでもあればドロップダウンリストに追加
        if (list.Count != 0)
        {
            roomlist.AddOptions(list);
        }
    }
    public void LoginGame()
    {
        //　ルームオプションを設定
        RoomOptions ro = new RoomOptions()
        {
            //　ルームを見えるようにする
            IsVisible = true,
            //　部屋の入室最大人数
            MaxPlayers = 10
        };

        if (roomname.text != "")
        {
            //　部屋がない場合は作って入室
            PhotonNetwork.JoinOrCreateRoom(roomname.text, ro, TypedLobby.Default);
        }
        else
        {
            //　部屋が存在すれば
            if (roomlist.options.Count != 0)
            {
                Debug.Log(roomlist.options[roomlist.value].text);
                PhotonNetwork.JoinRoom(roomlist.options[roomlist.value].text);
                //　部屋が存在しなければDefaultRoomという名前で部屋を作成
            }
            else
            {
                PhotonNetwork.JoinOrCreateRoom("DefaultRoom", ro, TypedLobby.Default);
            }
        }
    }
}
