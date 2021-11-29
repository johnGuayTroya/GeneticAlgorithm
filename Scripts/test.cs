using System.Collections;
using UnityEngine;

public class test : MonoBehaviour
{
    public PlayerManager agent;
    public GameObject[] actionBtn;

    void Start()
    {
        actionBtn = new GameObject[4];
        StartCoroutine(FindActionBtn());
    }

    IEnumerator FindActionBtn()
    {
        //esperamos un segundo para capturar nuestro objetos
        yield return new WaitForSeconds(1);
       
        actionBtn[0] = GameObject.Find("Soft");
        actionBtn[1] = GameObject.Find("Dummy");
        actionBtn[2] = GameObject.Find("Hard");
        actionBtn[3] = GameObject.Find("Rest");
        
        //cuidado con la vida del enemigo sucede un bug
        while (agent.Info.HP >0)
        {
            int rand = Random.Range(0, 4);
            actionBtn[rand].GetComponent<ActionButtonAttack>().OnClick();
            print("he elgido " + actionBtn[rand].name);
            yield return new WaitForSeconds(0.5f);
        }
     
    }

    
}
