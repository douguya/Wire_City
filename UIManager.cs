using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //è∆èÄÇÃUIÇ…ä÷Ç∑ÇÈÇ‡ÇÃ
    [SerializeField] Image Aim;
    [SerializeField] Sprite[] Aim_Sprite=new Sprite[2];
    public bool Within_range;

    //ÉÅÉjÉÖÅ[âÊñ Ç…ä÷Ç∑ÇÈÇ‡ÇÃ
    [SerializeField] GameObject Pose;
     public  bool Pose_view = false;

    // Start is called before the first frame update
    void Start()
    {
        Pose_view=false;
        Pose.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {


        if (Pose_view==true)
        {
            Pose.SetActive(true);
        }
        else
        {
            Pose.SetActive(false);
        }


        if (Within_range==true)
        {
            Aim.sprite=Aim_Sprite[0];
        }
        else
        {
            Aim.sprite=Aim_Sprite[1];
        }



        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Pose_view=!Pose_view;
        }




    }

    


}
