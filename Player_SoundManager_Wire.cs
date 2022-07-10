using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SoundManager_Wire : MonoBehaviour
{
    public AudioClip Wire_Firing;//発射
    public AudioClip Wire_Landing;//着弾
    public AudioClip Wire_Wind;//巻き取り
    public AudioClip Wire_Release;//解除
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Play_Wire_Firing()//ワイヤーの射出
    {

        audioSource.PlayOneShot(Wire_Firing);
    }


    public void Play_Wire_Landing()//ワイヤーの着弾
    {
        Invoke("Delay_Wire_Landing", 0.05f);
        
    }
    public void Delay_Wire_Landing()//ワイヤーの着弾の遅延再生
    {
        audioSource.PlayOneShot(Wire_Landing);
    }



    public void Play_Wire_Wind()//ワイヤーの巻き取り
    {
        audioSource.loop=true;
        Invoke("Delay_Play_Wire_Wind", 0.09f);
        
    }
    public void Delay_Play_Wire_Wind()//ワイヤーの巻き取りの遅延再生
    {
        audioSource.Play();
       
    }


    public void Play_Wire_Release()//ワイヤーの解除
    {
        audioSource.PlayOneShot(Wire_Release);
    }


    



    public void Stop_Sound()//再生終了
    {
        audioSource.Stop();
        audioSource.loop=false;
    }
}
