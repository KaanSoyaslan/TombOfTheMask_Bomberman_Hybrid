using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject PlayerSPRÝTE;
    public GameObject BulletSprite;
    public GameObject Effect;
    public GameObject Effect1;

    public int PlayerKeyboardSpeed = 5;

    public bool Col0;
    public bool Col1;
    public bool Col2;
    public bool Col3;

    public GameObject bomb;

    public bool HareketEdiyormuyum;

    public int Can = 3;

    public GameObject[] CanSptrites; //0 1 2


    public GameObject[] BombSprites;



    public int PlayerNUM;
    public GameObject CanvasBTNS;
    /// 0 MAVÝ
    /// 1 KIRMIZI
    /// 2 SARI
    /// 3 YEÞÝL
    /// 
    /// 

    public bool DamageControl;

    //oyuncular hayattamý
    public static bool isAliveP0;
    public static bool isAliveP1;
    public static bool isAliveP2;
    public static bool isAliveP3;



    bool botEnumarate;
    bool BombWait;
    float BotRuntime;


    public Sprite[] PlayerALLSprites;


    public int BombAmount;
    public int AllBombAmount;
    void Start()
    {
        Physics2D.IgnoreLayerCollision(12, 13, true); //bot alaný çarpmasýn die
        Physics2D.IgnoreLayerCollision(12, 15, true); //collider coine  çarpmasýn die
        Physics2D.IgnoreLayerCollision(13, 14, true);


        
        BotRuntime = 0.4f;  //bot zorluk

        DamageControl = false;

        isAliveP0 = true;
        isAliveP1 = true;
        isAliveP2 = true;
        isAliveP3 = true;

        if (PlayerPrefs.GetInt("playerActive" + PlayerNUM) == 1) //oyuncu aktifmi kontrol
        {
            gameObject.SetActive(true);

            BombAmount = 1;
        }
        else
        {
            if (PlayerNUM == 0)
            {
                isAliveP0 = false;
            }
            else if (PlayerNUM == 1)
            {
                isAliveP1 = false;
            }
            else if (PlayerNUM == 2)
            {
                isAliveP2 = false;
            }
            else if (PlayerNUM == 3)
            {
                isAliveP3 = false;
            }

            Can = -1;
            BombAmount = 0;

        }

        //oyuncu bot ise kontrolcü devre dýþý
        if (PlayerPrefs.GetInt("playerBOT" + PlayerNUM) == 1) 
        {
            CanvasBTNS.SetActive(false);
        }
        else if(PlayerPrefs.GetInt("playerActive" + PlayerNUM) == 1)
        {
            CanvasBTNS.SetActive(true);
        }
        else
        {
            CanvasBTNS.SetActive(false);
        }


        PlayerSPRÝTE.GetComponent<SpriteRenderer>().sprite = PlayerALLSprites[PlayerPrefs.GetInt("charP" + PlayerNUM)];

    }

    void Update()
    {
        for (int i = 0; i < CanSptrites.Length; i++)
        {
            if(Can >= i)
            {
                CanSptrites[i].SetActive(true);

            }
            else
            {
                CanSptrites[i].SetActive(false);
            }
        }

        for (int i = 0; i < BombSprites.Length; i++)
        {
            if (BombAmount > i)
            {
                BombSprites[i].SetActive(true);

            }
            else
            {
                BombSprites[i].SetActive(false);
            }
        }


        if(Can < 0) //ölmüþ agam
        {
            Instantiate(Effect, gameObject.transform.position, Quaternion.identity);
            CanvasBTNS.SetActive(false);
            gameObject.SetActive(false);

            if(PlayerNUM == 0)
            {
                isAliveP0 = false;
            }
            if(PlayerNUM == 1)
            {
                isAliveP1 = false;
            }
            if (PlayerNUM == 2)
            {
                isAliveP2 = false;
            }
            if (PlayerNUM == 3)
            {
                isAliveP3 = false;
            }


        }
        


        Col0 = ColliderControl.Col0;
        Col1 = ColliderControl.Col1;
        Col2 = ColliderControl.Col2;
        Col3 = ColliderControl.Col3;

       
        //atlanabilir duvar kontrolü
        if(PlayerNUM == 0 && ColliderControl.TouchP0)
        {
            Physics2D.IgnoreLayerCollision(6, 8, true);
        }

        else if(PlayerNUM == 0)
        {
            Physics2D.IgnoreLayerCollision(6, 8, false);
        }

        if (PlayerNUM == 1 && ColliderControl.TouchP1)
        {
            Physics2D.IgnoreLayerCollision(6, 9, true);
        }
        else if(PlayerNUM == 1)
        {
            Physics2D.IgnoreLayerCollision(6, 9, false);
        }

        if (PlayerNUM == 2 && ColliderControl.TouchP2)
        {
            Physics2D.IgnoreLayerCollision(6, 10, true);
        }
        else if (PlayerNUM == 2)
        {
            Physics2D.IgnoreLayerCollision(6, 10, false);
        }

        if (PlayerNUM == 3 && ColliderControl.TouchP3)
        {
            Physics2D.IgnoreLayerCollision(6, 11, true);
        }
        else if (PlayerNUM == 3)
        {
            Physics2D.IgnoreLayerCollision(6, 11, false);
        }

        //mermi koþusu
        if ((rb.velocity.x > 0.2 || rb.velocity.y > 0.2) || (rb.velocity.x < -0.2 || rb.velocity.y < -0.2))
        {
            PlayerSPRÝTE.SetActive(false);
            BulletSprite.SetActive(true);

            HareketEdiyormuyum = true;
        }
        else
        {
            PlayerSPRÝTE.SetActive(true);
            BulletSprite.SetActive(false);
            HareketEdiyormuyum = false;
        }



        if (HareketEdiyormuyum == false && PlayerPrefs.GetInt("playerBOT" + PlayerNUM) == 1 && botEnumarate == false)   //bot hareket
        {
            StartCoroutine(BotMoveTime());

            int rnd = Random.Range(0,4);

            if(rnd == 0)
            {
                UpP();
            }
            else if (rnd == 1)
            {
                DownP();
            }
            else if (rnd == 2)
            {
                LeftP();
            }
            else if (rnd == 3)
            {
                RightP();
            }
        }

        




        

    }

    //tüm oyuncularýn kontrolcüleri
    public void UpP0()
    {
        if (PlayerNUM == 0 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, PlayerKeyboardSpeed, 0);



            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
    public void DownP0()
    {
        if (PlayerNUM == 0 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, -PlayerKeyboardSpeed, 0);

            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));


            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }
    public void LeftP0()
    {
        if (PlayerNUM == 0 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(-PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }

    }
    public void RightP0()
    {
        if (PlayerNUM == 0 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

    }
    public void BombP0()
    {
        if (BombWait == false)
        {
            StartCoroutine(BMBwait());

        }

    }
    public void UpP1()
    {
        if (PlayerNUM == 1 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, PlayerKeyboardSpeed, 0);



            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
    public void DownP1()
    {
        if (PlayerNUM == 1 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, -PlayerKeyboardSpeed, 0);

            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));


            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }
    public void LeftP1()
    {
        if (PlayerNUM == 1 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(-PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }

    }
    public void RightP1()
    {
        if (PlayerNUM == 1 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

    }
    public void BombP1()
    {
        if (BombWait == false)
        {
            StartCoroutine(BMBwait());

        }
    }

    public void UpP2()
    {
        if (PlayerNUM == 2 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, PlayerKeyboardSpeed, 0);



            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
    public void DownP2()
    {
        if (PlayerNUM == 2 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, -PlayerKeyboardSpeed, 0);

            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));


            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }
    public void LeftP2()
    {
        if (PlayerNUM == 2 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(-PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }

    }
    public void RightP2()
    {
        if (PlayerNUM == 2 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

    }
    public void BombP2()
    {
        if (BombWait == false)
        {
            StartCoroutine(BMBwait());

        }
    }

    public void UpP3()
    {
        if (PlayerNUM == 3 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, PlayerKeyboardSpeed, 0);



            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
    public void DownP3()
    {
        if (PlayerNUM == 3 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, -PlayerKeyboardSpeed, 0);

            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));


            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }
    public void LeftP3()
    {
        if (PlayerNUM == 3 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(-PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }

    }
    public void RightP3()
    {
        if (PlayerNUM == 3 && !HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

    }
    public void BombP3()
    {
        if (BombWait == false)
        {
            StartCoroutine(BMBwait());

        }
    }


    public void UpP()
    {
        if (!HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, PlayerKeyboardSpeed, 0);



            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }
    }
    public void DownP()
    {
        if (!HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(0, -PlayerKeyboardSpeed, 0);

            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));


            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }
    public void LeftP()
    {
        if (!HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(-PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }

    }
    public void RightP()
    {
        if (!HareketEdiyormuyum)
        {
            rb.velocity = new Vector3(PlayerKeyboardSpeed, 0, 0);


            BulletSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

            PlayerSPRÝTE.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        }

    }
    public void BombP()
    {
        if (BombWait == false)
        {
            StartCoroutine(BMBwait());
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("BOMB") && DamageControl == false)
        {
            StartCoroutine(DamageCNTRL());
            Can--;

            SoundManager.PlaySound("Bruh");

        }

        if (collision.CompareTag("Coin"))
        {
            SoundManager.PlaySound("Coin");


            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") + 1); //coin arttý



            Instantiate(Effect1, gameObject.transform.position, Quaternion.identity);
            if (AllBombAmount < 5) // masksimum bomba sayýsý 5
            {
                AllBombAmount += 1;
                BombAmount += 1;
            }
            
            Destroy(collision.gameObject);
            
        }

    }

    private void OnTriggerStay2D(Collider2D collision) //bot bomba
    {
        if (collision.CompareTag("Player") && PlayerPrefs.GetInt("playerBOT" + PlayerNUM) == 1)
        {
            BombP();
        }

      
    }
   

    //ayný patlamayý 1 kez hasar yemek
    IEnumerator DamageCNTRL()
    {
        DamageControl = true;
        yield return new WaitForSeconds(2f);
        DamageControl = false;
       
    }
    IEnumerator BMBwait() //bomb exp
    {
        if(BombAmount > 0)
        {
            SoundManager.PlaySound("drop");
            Instantiate(bomb, gameObject.transform.position, Quaternion.identity);
            
            BombAmount--;
            yield return new WaitForSeconds(4f);
            
            
                BombAmount++;
            

            BombWait = false;
        }
        else
        {
            BombWait = false;
        }
    }

    IEnumerator BotMoveTime()
    {
        botEnumarate = true;
        yield return new WaitForSeconds(BotRuntime);
        botEnumarate = false;
    }
}
