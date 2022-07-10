using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SoundManager_Wire : MonoBehaviour
{
    public AudioClip Wire_Firing;//����
    public AudioClip Wire_Landing;//���e
    public AudioClip Wire_Wind;//�������
    public AudioClip Wire_Release;//����
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Play_Wire_Firing()//���C���[�̎ˏo
    {

        audioSource.PlayOneShot(Wire_Firing);
    }


    public void Play_Wire_Landing()//���C���[�̒��e
    {
        Invoke("Delay_Wire_Landing", 0.05f);
        
    }
    public void Delay_Wire_Landing()//���C���[�̒��e�̒x���Đ�
    {
        audioSource.PlayOneShot(Wire_Landing);
    }



    public void Play_Wire_Wind()//���C���[�̊������
    {
        audioSource.loop=true;
        Invoke("Delay_Play_Wire_Wind", 0.09f);
        
    }
    public void Delay_Play_Wire_Wind()//���C���[�̊������̒x���Đ�
    {
        audioSource.Play();
       
    }


    public void Play_Wire_Release()//���C���[�̉���
    {
        audioSource.PlayOneShot(Wire_Release);
    }


    



    public void Stop_Sound()//�Đ��I��
    {
        audioSource.Stop();
        audioSource.loop=false;
    }
}
