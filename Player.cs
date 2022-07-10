using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //プレイヤーに直接関係するもの
    private Rigidbody rb;//プレイヤーのリジットボディ
    [SerializeField] private float speed;//プレイヤーの加速度
    [SerializeField] private float LimitSpeed;//プレイヤーの速度制限
    [SerializeField] private GameObject PlayerCameraPoint;//カメラの支点オブジェクト
    [SerializeField] private GameObject PlayerCamera;//カメラのオブジェクト

    float Mouse_conversion_X;//マウスによるカメラの移動用（実際はプレイヤーの向きを変える）
    float Mouse_conversion_Y;//マウスによるカメラの移動用
    private Vector3[] mouse = new Vector3[2];  //変換前のマウスの位置


    //ワイヤーに関するもの
    [SerializeField] private GameObject Wire_shot_shell;//ワイヤーショットのプレハブ
    [SerializeField] private GameObject Wire_shot_Point;//ワイヤーショットの射出地点
    [SerializeField] private int Wire_Distance;//ワイヤーの射程距離
    [SerializeField] private int Wire_Cut_Distance;//ワイヤー自切距離
    [SerializeField] private float Wire_TimeLimit;// ワイヤー自切時間

    private GameObject Generated_Wire_shot;//ワイヤーの先端のオブジェクト
    public bool Wire_On = false;//ワイヤーの射出開始
    LineRenderer Wire_Line;//ワイヤーを描画するレンダラー
    private float[] Wire_to_PlayerDistance = new float[3];

    public bool Wire_Go = false;//ワイヤーの発射判定
    public bool Wire_farst = false;//ワイヤーの発射判定その２　着弾後一度だけ作動させるためのもの
    public bool Wire_Distance_far = false;
    public int Wire_force;//牽引の力
    private Vector3 Add_Wire_force;//Y方向の牽引力調整用
    public float Wire_Timer;// ワイヤー自切用

    RaycastHit hit;//RaycastHitを作成



    //UIに関するもの
    public UIManager UIManager;//キャンバスグループのこと
 

    // Start is called before the first frame update

    public Player_SoundManager_Wire Wire_Sound;//ワイヤーに関するサウンド
    private bool Player_CamMove; //プレイヤーの走査許可
    



    void Start()
    {
        Cursor.visible = false;
      //  Cursor.lockState = CursorLockMode.None;
        rb = GetComponent<Rigidbody>();//リジットボディの取得
        mouse[0]=Input.mousePosition;//mouseの座標の取得
        Wire_Line=GetComponent<LineRenderer>();//ラインレンダラーの取得
        Wire_Line.positionCount = 2;//ラインレンダラーの調点数指定
        UIManager=GameObject.Find("Canvas").GetComponent<UIManager>();//UIManagerとの連携
        Wire_Sound=GameObject.Find("Player_SoundManager_wire").GetComponent<Player_SoundManager_Wire>();//Player_SoundManager_Wireとの連携
    }

    // Update is called once per frame
  

    private void FixedUpdate()
    {

        Cursor_View();//カーソルが画面内の時に隠す

        if (Player_CamMove==true)//プレイヤーの走査許可
        {
            Turn_around();//マウスによる視点の移動
            Move();//プレイヤーの移動
        }
        Wire_Stay();//ワイヤーに関するもの 起動判定は関数の中で行う
        
    }

    public void Cursor_View()//カーソルの表示
    {
        if (UIManager.Pose_view==true)//
        {
            Cursor.visible = true;//隠す
            Player_CamMove=false;
        }
        else
        {
            
            Cursor.visible = false;//表示
            Player_CamMove=true;
        }
      
    }

   


    public void Turn_around()//マウスによる視点の移動
    {


            mouse[1]=Input.mousePosition;//mouseの座標の取得
            if (mouse[0] !=mouse[1])//マウスが移動しているかどうか
            {
                Mouse_conversion_X=MapCamera(mouse[1].x-mouse[0].x, 0, Screen.width, transform.rotation.y-180, transform.rotation.y+180);//マウスの横の移動距離をMap関数によって変換(画面の横のサイズから自分から見て±180°の範囲に限定)
                Mouse_conversion_Y=MapCamera(mouse[0].y-mouse[1].y, 0, Screen.height, -78, 81);//マウスの縦の移動距離をMap関数によって変換(画面の縦のサイズから-71°から81°の範囲に限定)
                this.transform.Rotate(new Vector3(0, Mouse_conversion_X, 0));//変換した値でプレイヤーの横の向きを変える
                PlayerCameraPoint.transform.Rotate(new Vector3(Mouse_conversion_Y, 0, 0));//変換した値でカメラ支点の縦の向きを変える

                mouse[0]=mouse[1];//マウス座標のリセット
            }
           


    }
    private void Move()//プレイヤーの移動
    {

        if (Input.GetKey(KeyCode.A))
        {
            if (Vector3.Dot(rb.velocity, -transform.right)<=LimitSpeed) //特定方向への速度が速度制限を超えている稼働kあ
            {
                rb.AddForce(-transform.right*speed);
            }
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (Vector3.Dot(rb.velocity, -transform.right)<=LimitSpeed)
            {
                rb.AddForce(transform.right*speed);
            }
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (Vector3.Dot(rb.velocity, transform.forward)<=LimitSpeed)
            {
                rb.AddForce(transform.forward*speed);
            }

        }

        if (Input.GetKey(KeyCode.S))
        {
            if (Vector3.Dot(rb.velocity, -transform.forward)<=LimitSpeed)
            {
                rb.AddForce(-transform.forward*speed);
            }
        }






        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0, 1, 0)*speed/2, ForceMode.Impulse);
        }
    }


    private void Wire_Stay()//ワイヤーに関するもの 
    {
        Ray ray = new Ray(PlayerCameraPoint.transform.position, PlayerCamera.transform.forward);//射程圏内かどうかの判定に使うRayの生成

        if (Physics.SphereCast(ray, 0.2f, out hit, Wire_Distance))//hitメソッドにrayを渡して、当たったかどうかを判定　ワイヤー先端部の範囲で判定をとる
        {
            //以下はワイヤー射程圏内での動作

            UIManager.Within_range=true;//照準を射程圏内を示すものに現行

            if (Player_CamMove==true)
            {
                if (Input.GetMouseButtonDown(0))//左クリック(ワイヤーの射出)
                {
                    Destroy(Generated_Wire_shot);//ワイヤー先端部の消去
                    Wire_farst=true;//ワイヤーの着弾判定その２　一度だけtrueにしたいためここで変更
                    Wire_On=true;//ワイヤーの発射開始
                    Wire_Line.enabled=true;//ラインレンダラーを有効にする
                    Wire_to_PlayerDistance[2]=Vector3.Distance(transform.position, hit.point);//目標地点とプレイヤーの距離を格納(自切処理で使用)
                    Generated_Wire_shot=(GameObject)Instantiate(Wire_shot_shell, Wire_shot_Point.transform.position, PlayerCamera.transform.rotation);//ワイヤーの先端部の生成と格納

                    Generated_Wire_shot.GetComponent<Wire_Shot_Shell>().targetPosition=hit.point;//ワイヤーの先端部の目標地点をRayの着弾地点へ設定
                    Wire_Sound.Play_Wire_Firing();
                }
            }
        }
        else//照準の先にオブジェクトがない
        {
            UIManager.Within_range=false;//照準を射程圏外を示すものに現行
        }



        if (Wire_On)//ワイヤーの射出開始
        {
      

            Wire_Line.SetPosition(0, Wire_shot_Point.transform.position);//ワイヤーの始点を設定(ワイヤーの射出ポイント)
            Wire_Line.SetPosition(1, Generated_Wire_shot.transform.position);//ワイヤーの終点を設定(ワイヤーの先端)

            if (Wire_Go==true)//ワイヤーによるけん引開始
            {


                if (Wire_farst==true)//けん引開始時の処理
                {
                    Wire_Sound.Play_Wire_Landing();
                    
                    Wire_to_PlayerDistance[0]=Vector3.Distance(transform.position, Generated_Wire_shot.transform.position);//プレイヤーとワイヤーの先端との距離
                    rb.velocity=new Vector3(rb.velocity.x, rb.velocity.y/2, rb.velocity.z);//落下or上昇速度の減衰
                    Wire_farst=false;
                    Wire_Sound.Play_Wire_Wind();
                }

                Add_Wire_force=(Generated_Wire_shot.transform.position-transform.position).normalized*Wire_force;
                rb.AddForce(new Vector3(Add_Wire_force.x, Add_Wire_force.y*1.3f, Add_Wire_force.z));//目標地点へプレイヤーを牽引 Y方向へ強めに設定
                Wire_to_PlayerDistance[1]=Vector3.Distance(transform.position, Generated_Wire_shot.transform.position);
                if (Wire_to_PlayerDistance[0]!= Wire_to_PlayerDistance[1])//ワイヤーとの距離に変化があった時
                {
                    if (Wire_to_PlayerDistance[0]<= Wire_to_PlayerDistance[1])//ワイヤーとの距離が縮まっている時
                    {
                        Wire_Distance_far=true;//ワイヤーから遠のいている
                    }
                    else
                    {
                        Wire_Distance_far=false;//ワイヤーに近づいている
                    }

                    Wire_to_PlayerDistance[0]=Vector3.Distance(transform.position, Generated_Wire_shot.transform.position);

                }


                
                //以下 ワイヤー地点が自分の後ろにあり、自分がワイヤーから遠のいていて、なおかつその距離が一定以上の場合、ワイヤーを自切
                if (Vector3.Dot(Generated_Wire_shot.transform.position-transform.position, transform.forward)<=0.3)//内積による前後判定　後ろの場合
                {
                    if (Wire_Distance_far==true) {
                        if (Wire_to_PlayerDistance[0]>=15*Map(Wire_to_PlayerDistance[2], 0, Wire_Distance, 0f, 1.3f))//ワイヤーの先端部との距離が一定を超えたらワイヤーを自切
                        {
                            if (Wire_On==true&&Wire_Go==true&&Wire_farst==false)//ワイヤー牽引中かどうか
                            {
                                Wire_Cut();
                            }
                        }
                    }


                }
               

                if (Wire_to_PlayerDistance[0]<5)
                {
                    Wire_Timer+=Time.deltaTime;
                    if (Wire_Timer>=Wire_TimeLimit*Map(Wire_to_PlayerDistance[2], 0, Wire_Distance, 0.7f, 1.1f)) //ワイヤーの距離が一定以下の時がしばらく続いたら自切るする
                    {
                        if (Wire_On==true&&Wire_Go==true&&Wire_farst==false)
                        {
                            Wire_Cut();
                        }
                    }
                }
                else
                {
                    Wire_Timer=0;
                }
              


            }
            if (Input.GetMouseButtonDown(1))//右　クリック(ワイヤーの自動切り離し)
            {
                if (Wire_On==true&&Wire_Go==true&&Wire_farst==false)
                {
                    Wire_Cut();//ワイヤーの切り離し
                }
            }
        }
      


    }

    public void Wire_Cut()//ワイヤーの切り離し
    {
        Destroy(Generated_Wire_shot);//ワイヤー先端部の消去
        Wire_Timer=0;
        Wire_On=false;//以下全てのフラグの初期化
        Wire_Line.enabled=false;
        Wire_Go=false;
        Wire_Distance_far=false;
        Wire_Sound.Stop_Sound();
        Wire_Sound.Play_Wire_Release();


    }

    public void Respawn()//プレイヤーを初期位置へ戻す
    {
        transform.position=new Vector3(196.7244f, 271.4032f, -194.0423f);
        GetComponent<Rigidbody>().velocity=Vector3.zero;
    }



    public float Map(float value, float R_min, float R_max, float V_min, float V_max)
    {
            /*
            float Rrenge = (R_max-R_min);
            float convartR = (value-R_min);
            float Rratio = Rrenge/ convartR;

            float Vrenge = (V_max-V_min);
            float VDelta = (Vrenge/Rratio);


            */

            return V_min+ (V_max-V_min)/((R_max-R_min)/(value-R_min));//valueをV_minからV_Maxの範囲からR_minからR_maxの範囲にする

    }
    public float MapCamera(float value, float V_min, float V_max, float R_min, float R_max) //カメラ用の変換関数 
    {
       
        return value*((R_max-R_min)/(V_max-V_min));//valueをV_minからV_Maxの範囲の差分からR_minからR_maxの差分の範囲に変換するにする
    }



}
