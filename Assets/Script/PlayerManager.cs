using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerManager : MonoBehaviour
{
    private Rigidbody rigidbody;
    private GameObject photoncountmanager;
    private PhotonView countphoton;
    private PhotonView myphoton;
    public static float redwincount;
    public static float bluewincount;
    public float speed = 1;
    public GameObject b;
    public GameObject sunabokori;
    private bool isjumping;
    public float movespeed;
    public AudioSource audio;
    public Camera Camra;
    public GameObject ballps;
    public GameObject PreBall;
    public GameObject GunAudio;
    public float shell;
    public AudioClip riro, riro2;
    private float HP = 5;
    private int count;
    private int bluecount;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Camera camera;
    [SerializeField]
    Text hptext;
    // Start is called before the first frame update
    private void Awake()
    {
        //自分のフォトンビューを取得
        this.myphoton = GetComponent<PhotonView>();
        if (myphoton.isMine)
        {
            b = transform.Find("WZ1.000/Body/a").gameObject;
            GameObject camPrefab = (GameObject)Resources.Load("MainCamera");
            Vector3 CameraPosition = new Vector3(0.9818258f, 0.9f, -1.949493f);
            //カメラを生成
            GameObject cam = Instantiate(camPrefab, transform.position + CameraPosition, transform.rotation);
            //カメラを自分の子オブジェクトにする
            cam.transform.parent = gameObject.transform;
            //カメラの名前をcamに変更
            cam.name = "cam";
            camera = transform.Find("cam").GetComponent<Camera>();
            transform.Find("Canvas").GetComponent<Canvas>().worldCamera = camera;
            transform.Find("Canvas").GetComponent<Canvas>().planeDistance = 1f;
            hptext = transform.Find("Canvas/Text").GetComponent<Text>();
        }
        else
        {
            //自分以外のキャンバスを非表示にする
            transform.Find("Canvas").gameObject.SetActive(false);
        }


    }
    void Start()
    { 
        if (myphoton.isMine)
        {          　
            rigidbody = GetComponent<Rigidbody>();
            photoncountmanager = GameObject.Find("winmanager");
            countphoton = photoncountmanager.GetComponent<PhotonView>();
            Camra = GetComponentInChildren<Camera>();
        }    
    }

    // Update is called once per frame
    void Update()
    {       
        if (myphoton.isMine)
        {
            //ジャンプの処理
            if (Input.GetKeyDown(KeyCode.Space) && isjumping == false)
            {
                rigidbody.velocity = Vector3.up * movespeed;
                isjumping = true;
            }
            hptext.text = "HP:" + HP;
            //カメラを固定する処理
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            //カメラの固定を解除する処理
            if (Input.GetKeyDown(KeyCode.E))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            //GetComponentを用いてAnimatorコンポーネントを取り出す.
            Animator animator = GetComponent<Animator>();
            //あらかじめ設定していたintパラメーター「trans」の値を取り出す.
            int trans = animator.GetInteger("trans");
            //前身する処理
            if (Input.GetKey(KeyCode.W))
            {
                trans = 1;
                sunabokori.SetActive(true);
                Vector3 velocity = gameObject.transform.rotation * new Vector3(0, 0, speed);
                gameObject.transform.position += velocity * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                sunabokori.SetActive(false);
                trans = 0;
            }
            //後退するコード
            if (Input.GetKey(KeyCode.S))
            {
                trans = 2;
                Vector3 velocity = gameObject.transform.rotation * new Vector3(0, 0, -speed);
                gameObject.transform.position += velocity * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                trans = 0;
            }
            //右に行くコード
            if (Input.GetKey(KeyCode.D))
            {
                trans = 3;
                Vector3 velocity = gameObject.transform.rotation * new Vector3(speed, 0, 0);
                gameObject.transform.position += velocity * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                trans = 0;
            }
            //左に行くコード
            if (Input.GetKey(KeyCode.A))
            {
                trans = 4;
                Vector3 velocity = gameObject.transform.rotation * new Vector3(-speed, 0, 0);
                gameObject.transform.position += velocity * Time.deltaTime;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                trans = 0;
            }
            //射撃用コード
            if (Input.GetKey(KeyCode.Mouse0) && shell > 0)
            {
                GunAudio.SetActive(true);
                if (shell > 0)
                {
                    shell -= 1;
                }             
                myphoton.RPC("CreateShell", PhotonTargets.AllBuffered, PhotonNetwork.AllocateViewID());
            }
            //リロードするコード
            if (Input.GetKeyDown(KeyCode.R) && shell < 30)
            {
                shell = 30;
                audio.PlayOneShot(riro);
                StartCoroutine(rirodo());
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                GunAudio.SetActive(false);
            }
            //銃を覗くコード
            if (Input.GetKey(KeyCode.Mouse1))
            {
                Camra.fieldOfView = 35;
                //Debug.Log("a");
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                Camra.fieldOfView = 60;
            }
            //intパラメーターの値を設定する.
            animator.SetInteger("trans", trans);
            //青の勝利条件
            if (HP < 0 && this.gameObject.name == "Player")
            {
                transform.position = new Vector3(6, 0.299f, -25);
                HP = 5;
                countphoton.RPC("SetText01", PhotonTargets.AllBuffered, count);
                Debug.Log("Blue");
                count += 1;
                // myphoton.RPC("SetText", PhotonTargets.AllBuffered, redwincount++);
                if (count >= 10)
                {
                    countphoton.RPC("BlueWin", PhotonTargets.AllBuffered, "Blue is Win");
                    Debug.Log("Blue is Win");
                }
            }
            //赤の勝利条件
            if (HP < 0 && this.gameObject.name == "Player2")
            {
                transform.position = new Vector3(10.3f, 1.437304f, -77);
                HP = 5;
                countphoton.RPC("SetText", PhotonTargets.AllBuffered, bluecount);
                Debug.Log("Red");
                //myphoton.RPC("SetText", PhotonTargets.AllBuffered, bluewincount++);
                bluecount += 1;
                if (bluecount >= 10)
                {
                    countphoton.RPC("RedWin", PhotonTargets.AllBuffered, "Red is Win");
                    Debug.Log("Red is Win");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (myphoton.isMine)
        {
            //体力を減らす処理
            if (this.gameObject.name == "Player" && collider.gameObject.tag == "Blue" && myphoton.isMine)
            {
                HP -= 1;
            }
            if (this.gameObject.name == "Player2" && collider.gameObject.tag == "Red" && myphoton.isMine)
            {
                HP -= 1;
            }
        }
   
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ジャンプ処理
        if(collision.gameObject.tag == "Graund")
        {
            isjumping = false;
        }
    }
    float time;
    //玉を発射と同期するコード
    [PunRPC]
    void CreateShell(int viewID)
    {
        
        GameObject ball = Instantiate(PreBall);
        ball.transform.position = ballps.transform.position;
        Rigidbody rbody = ball.GetComponent<Rigidbody>();
        rbody.AddForce(ballps.transform.forward * 500);
        
      //  ball.transform.parent = gameObject.transform;
        var _photonView = ball.gameObject.AddComponent<PhotonView>();
        var _photonTransformView = ball.gameObject.AddComponent<PhotonTransformView>();
        var _photonRigidbodyView = ball.gameObject.AddComponent<PhotonRigidbodyView>();

        //PhotonView の ObservedComponents リストを初期化
        _photonView.ObservedComponents = new List<Component>();

        //PhotonView に ViewID を設定
        _photonView.viewID = viewID;

        //到達保証の設定
        //詳しくは https://support.photonengine.jp/hc/ja/articles/224763767-PUN%E3%81%A7%E5%88%B0%E9%81%94%E4%BF%9D%E8%A8%BC%E3%81%AE%E8%A8%AD%E5%AE%9A%E3%82%92%E8%A1%8C%E3%81%86
        _photonView.synchronization = ViewSynchronization.ReliableDeltaCompressed;

        //PhotonTransformView の設定
        //位置の同期を有効にする
        _photonTransformView.m_PositionModel.SynchronizeEnabled = true;

        //回転の同期を有効にする
        _photonTransformView.m_RotationModel.SynchronizeEnabled = true;

        //リストに追加して同期対象に加える
        _photonView.ObservedComponents.Add(_photonTransformView);
        _photonView.ObservedComponents.Add(_photonRigidbodyView);
    }
    //リロードのサウンド処理
    IEnumerator rirodo()
    {
        yield return new WaitForSeconds(0.6f);
        audio.PlayOneShot(riro2);
    }
}
