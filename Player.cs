using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //�v���C���[�ɒ��ڊ֌W�������
    private Rigidbody rb;//�v���C���[�̃��W�b�g�{�f�B
    [SerializeField] private float speed;//�v���C���[�̉����x
    [SerializeField] private float LimitSpeed;//�v���C���[�̑��x����
    [SerializeField] private GameObject PlayerCameraPoint;//�J�����̎x�_�I�u�W�F�N�g
    [SerializeField] private GameObject PlayerCamera;//�J�����̃I�u�W�F�N�g

    float Mouse_conversion_X;//�}�E�X�ɂ��J�����̈ړ��p�i���ۂ̓v���C���[�̌�����ς���j
    float Mouse_conversion_Y;//�}�E�X�ɂ��J�����̈ړ��p
    private Vector3[] mouse = new Vector3[2];  //�ϊ��O�̃}�E�X�̈ʒu


    //���C���[�Ɋւ������
    [SerializeField] private GameObject Wire_shot_shell;//���C���[�V���b�g�̃v���n�u
    [SerializeField] private GameObject Wire_shot_Point;//���C���[�V���b�g�̎ˏo�n�_
    [SerializeField] private int Wire_Distance;//���C���[�̎˒�����
    [SerializeField] private int Wire_Cut_Distance;//���C���[���؋���
    [SerializeField] private float Wire_TimeLimit;// ���C���[���؎���

    private GameObject Generated_Wire_shot;//���C���[�̐�[�̃I�u�W�F�N�g
    public bool Wire_On = false;//���C���[�̎ˏo�J�n
    LineRenderer Wire_Line;//���C���[��`�悷�郌���_���[
    private float[] Wire_to_PlayerDistance = new float[3];

    public bool Wire_Go = false;//���C���[�̔��˔���
    public bool Wire_farst = false;//���C���[�̔��˔��肻�̂Q�@���e���x�����쓮�����邽�߂̂���
    public bool Wire_Distance_far = false;
    public int Wire_force;//�����̗�
    private Vector3 Add_Wire_force;//Y�����̌����͒����p
    public float Wire_Timer;// ���C���[���ؗp

    RaycastHit hit;//RaycastHit���쐬



    //UI�Ɋւ������
    public UIManager UIManager;//�L�����o�X�O���[�v�̂���
 

    // Start is called before the first frame update

    public Player_SoundManager_Wire Wire_Sound;//���C���[�Ɋւ���T�E���h
    private bool Player_CamMove; //�v���C���[�̑�������
    



    void Start()
    {
        Cursor.visible = false;
      //  Cursor.lockState = CursorLockMode.None;
        rb = GetComponent<Rigidbody>();//���W�b�g�{�f�B�̎擾
        mouse[0]=Input.mousePosition;//mouse�̍��W�̎擾
        Wire_Line=GetComponent<LineRenderer>();//���C�������_���[�̎擾
        Wire_Line.positionCount = 2;//���C�������_���[�̒��_���w��
        UIManager=GameObject.Find("Canvas").GetComponent<UIManager>();//UIManager�Ƃ̘A�g
        Wire_Sound=GameObject.Find("Player_SoundManager_wire").GetComponent<Player_SoundManager_Wire>();//Player_SoundManager_Wire�Ƃ̘A�g
    }

    // Update is called once per frame
  

    private void FixedUpdate()
    {

        Cursor_View();//�J�[�\������ʓ��̎��ɉB��

        if (Player_CamMove==true)//�v���C���[�̑�������
        {
            Turn_around();//�}�E�X�ɂ�鎋�_�̈ړ�
            Move();//�v���C���[�̈ړ�
        }
        Wire_Stay();//���C���[�Ɋւ������ �N������͊֐��̒��ōs��
        
    }

    public void Cursor_View()//�J�[�\���̕\��
    {
        if (UIManager.Pose_view==true)//
        {
            Cursor.visible = true;//�B��
            Player_CamMove=false;
        }
        else
        {
            
            Cursor.visible = false;//�\��
            Player_CamMove=true;
        }
      
    }

   


    public void Turn_around()//�}�E�X�ɂ�鎋�_�̈ړ�
    {


            mouse[1]=Input.mousePosition;//mouse�̍��W�̎擾
            if (mouse[0] !=mouse[1])//�}�E�X���ړ����Ă��邩�ǂ���
            {
                Mouse_conversion_X=MapCamera(mouse[1].x-mouse[0].x, 0, Screen.width, transform.rotation.y-180, transform.rotation.y+180);//�}�E�X�̉��̈ړ�������Map�֐��ɂ���ĕϊ�(��ʂ̉��̃T�C�Y���玩�����猩�ā}180���͈̔͂Ɍ���)
                Mouse_conversion_Y=MapCamera(mouse[0].y-mouse[1].y, 0, Screen.height, -78, 81);//�}�E�X�̏c�̈ړ�������Map�֐��ɂ���ĕϊ�(��ʂ̏c�̃T�C�Y����-71������81���͈̔͂Ɍ���)
                this.transform.Rotate(new Vector3(0, Mouse_conversion_X, 0));//�ϊ������l�Ńv���C���[�̉��̌�����ς���
                PlayerCameraPoint.transform.Rotate(new Vector3(Mouse_conversion_Y, 0, 0));//�ϊ������l�ŃJ�����x�_�̏c�̌�����ς���

                mouse[0]=mouse[1];//�}�E�X���W�̃��Z�b�g
            }
           


    }
    private void Move()//�v���C���[�̈ړ�
    {

        if (Input.GetKey(KeyCode.A))
        {
            if (Vector3.Dot(rb.velocity, -transform.right)<=LimitSpeed) //��������ւ̑��x�����x�����𒴂��Ă���ғ�k��
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


    private void Wire_Stay()//���C���[�Ɋւ������ 
    {
        Ray ray = new Ray(PlayerCameraPoint.transform.position, PlayerCamera.transform.forward);//�˒��������ǂ����̔���Ɏg��Ray�̐���

        if (Physics.SphereCast(ray, 0.2f, out hit, Wire_Distance))//hit���\�b�h��ray��n���āA�����������ǂ����𔻒�@���C���[��[���͈̔͂Ŕ�����Ƃ�
        {
            //�ȉ��̓��C���[�˒������ł̓���

            UIManager.Within_range=true;//�Ə����˒��������������̂Ɍ��s

            if (Player_CamMove==true)
            {
                if (Input.GetMouseButtonDown(0))//���N���b�N(���C���[�̎ˏo)
                {
                    Destroy(Generated_Wire_shot);//���C���[��[���̏���
                    Wire_farst=true;//���C���[�̒��e���肻�̂Q�@��x����true�ɂ��������߂����ŕύX
                    Wire_On=true;//���C���[�̔��ˊJ�n
                    Wire_Line.enabled=true;//���C�������_���[��L���ɂ���
                    Wire_to_PlayerDistance[2]=Vector3.Distance(transform.position, hit.point);//�ڕW�n�_�ƃv���C���[�̋������i�[(���؏����Ŏg�p)
                    Generated_Wire_shot=(GameObject)Instantiate(Wire_shot_shell, Wire_shot_Point.transform.position, PlayerCamera.transform.rotation);//���C���[�̐�[���̐����Ɗi�[

                    Generated_Wire_shot.GetComponent<Wire_Shot_Shell>().targetPosition=hit.point;//���C���[�̐�[���̖ڕW�n�_��Ray�̒��e�n�_�֐ݒ�
                    Wire_Sound.Play_Wire_Firing();
                }
            }
        }
        else//�Ə��̐�ɃI�u�W�F�N�g���Ȃ�
        {
            UIManager.Within_range=false;//�Ə����˒����O���������̂Ɍ��s
        }



        if (Wire_On)//���C���[�̎ˏo�J�n
        {
      

            Wire_Line.SetPosition(0, Wire_shot_Point.transform.position);//���C���[�̎n�_��ݒ�(���C���[�̎ˏo�|�C���g)
            Wire_Line.SetPosition(1, Generated_Wire_shot.transform.position);//���C���[�̏I�_��ݒ�(���C���[�̐�[)

            if (Wire_Go==true)//���C���[�ɂ�邯����J�n
            {


                if (Wire_farst==true)//������J�n���̏���
                {
                    Wire_Sound.Play_Wire_Landing();
                    
                    Wire_to_PlayerDistance[0]=Vector3.Distance(transform.position, Generated_Wire_shot.transform.position);//�v���C���[�ƃ��C���[�̐�[�Ƃ̋���
                    rb.velocity=new Vector3(rb.velocity.x, rb.velocity.y/2, rb.velocity.z);//����or�㏸���x�̌���
                    Wire_farst=false;
                    Wire_Sound.Play_Wire_Wind();
                }

                Add_Wire_force=(Generated_Wire_shot.transform.position-transform.position).normalized*Wire_force;
                rb.AddForce(new Vector3(Add_Wire_force.x, Add_Wire_force.y*1.3f, Add_Wire_force.z));//�ڕW�n�_�փv���C���[������ Y�����֋��߂ɐݒ�
                Wire_to_PlayerDistance[1]=Vector3.Distance(transform.position, Generated_Wire_shot.transform.position);
                if (Wire_to_PlayerDistance[0]!= Wire_to_PlayerDistance[1])//���C���[�Ƃ̋����ɕω�����������
                {
                    if (Wire_to_PlayerDistance[0]<= Wire_to_PlayerDistance[1])//���C���[�Ƃ̋������k�܂��Ă��鎞
                    {
                        Wire_Distance_far=true;//���C���[���牓�̂��Ă���
                    }
                    else
                    {
                        Wire_Distance_far=false;//���C���[�ɋ߂Â��Ă���
                    }

                    Wire_to_PlayerDistance[0]=Vector3.Distance(transform.position, Generated_Wire_shot.transform.position);

                }


                
                //�ȉ� ���C���[�n�_�������̌��ɂ���A���������C���[���牓�̂��Ă��āA�Ȃ������̋��������ȏ�̏ꍇ�A���C���[������
                if (Vector3.Dot(Generated_Wire_shot.transform.position-transform.position, transform.forward)<=0.3)//���ςɂ��O�㔻��@���̏ꍇ
                {
                    if (Wire_Distance_far==true) {
                        if (Wire_to_PlayerDistance[0]>=15*Map(Wire_to_PlayerDistance[2], 0, Wire_Distance, 0f, 1.3f))//���C���[�̐�[���Ƃ̋��������𒴂����烏�C���[������
                        {
                            if (Wire_On==true&&Wire_Go==true&&Wire_farst==false)//���C���[���������ǂ���
                            {
                                Wire_Cut();
                            }
                        }
                    }


                }
               

                if (Wire_to_PlayerDistance[0]<5)
                {
                    Wire_Timer+=Time.deltaTime;
                    if (Wire_Timer>=Wire_TimeLimit*Map(Wire_to_PlayerDistance[2], 0, Wire_Distance, 0.7f, 1.1f)) //���C���[�̋��������ȉ��̎������΂炭�������玩�؂邷��
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
            if (Input.GetMouseButtonDown(1))//�E�@�N���b�N(���C���[�̎����؂藣��)
            {
                if (Wire_On==true&&Wire_Go==true&&Wire_farst==false)
                {
                    Wire_Cut();//���C���[�̐؂藣��
                }
            }
        }
      


    }

    public void Wire_Cut()//���C���[�̐؂藣��
    {
        Destroy(Generated_Wire_shot);//���C���[��[���̏���
        Wire_Timer=0;
        Wire_On=false;//�ȉ��S�Ẵt���O�̏�����
        Wire_Line.enabled=false;
        Wire_Go=false;
        Wire_Distance_far=false;
        Wire_Sound.Stop_Sound();
        Wire_Sound.Play_Wire_Release();


    }

    public void Respawn()//�v���C���[�������ʒu�֖߂�
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

            return V_min+ (V_max-V_min)/((R_max-R_min)/(value-R_min));//value��V_min����V_Max�͈̔͂���R_min����R_max�͈̔͂ɂ���

    }
    public float MapCamera(float value, float V_min, float V_max, float R_min, float R_max) //�J�����p�̕ϊ��֐� 
    {
       
        return value*((R_max-R_min)/(V_max-V_min));//value��V_min����V_Max�͈̔͂̍�������R_min����R_max�̍����͈̔͂ɕϊ�����ɂ���
    }



}
