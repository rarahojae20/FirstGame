using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public PlayerMove player;

    public GameObject[] stages;

    public Image[] UIhealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject UIRestartBtn;


    void Update()
    {
        UIPoint.text= (totalPoint + stagePoint).ToString();
    }
    public void NextStage()
    {
        if(stageIndex < stages.Length){
        stages[stageIndex].SetActive(false);
        stageIndex ++;
        stages[stageIndex].SetActive(true);
        PlayerReposition();

        UIStage.text = "STAGE" + (stageIndex+1);
        }
        else
        {
            Time.timeScale = 0;
            UIRestartBtn.SetActive(true);
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();
            btnText.text = "Game Clear";
            UIRestartBtn.SetActive(true);
        }



        totalPoint += stagePoint;
        stagePoint = 0;
    }

 
    public void HealthDown()
    {
        if(health > 1)
    {


            health --;
            UIhealth[health].color = new Color(1,0,0,0.4f);
    }
            else
            {
            UIhealth[0].color = new Color(1,0,0,0.4f);

            player.OnDie();

            UIRestartBtn.SetActive(true);
            }
    }



   void OnTriggerEnter2D(Collider2D collision)
   {
    if(collision.gameObject.tag == "Player")
    {
     HealthDown();
        if(health > 1){
        PlayerReposition();
        }
            HealthDown();

   }
   }

  void PlayerReposition()
   {
    player.transform.position = new Vector3(-10,2,0);
    player.velocityZero();
   }

   /* public void Restart()
    {   
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        
    }*/


}