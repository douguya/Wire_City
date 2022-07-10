using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Material Display_Material;
    [SerializeField] Texture[] Display_Albedo = new Texture[9];
    float Timer=0;
    int Texture_Num=1;
    void Start()
    {
        Display_Material.mainTexture=Display_Albedo[0];
    }

    // Update is called once per frame
    void Update()
    {

        Timer+=Time.deltaTime;
        if (Timer>3)
        {
            
            Display_Material.mainTexture=Display_Albedo[Texture_Num];
            Timer=0;
            Texture_Num+=1;
            if (Texture_Num==9)
            {
                Texture_Num=0;
            }
        }


    }
}
