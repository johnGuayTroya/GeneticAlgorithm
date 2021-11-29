using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine;
public class AutomaticCombat : MonoBehaviour
{
    public PlayerManager agent;
    public GameObject[] actionBtn;

    void Start()
    {
        agent = FindObjectOfType<PlayerManager>();
        actionBtn = new GameObject[4];
        // Esperamos hasta obtener los botones
       

        //StartCoroutine(FindActionBtn());
    }

    public void StartCombat()
    {
        //StartCoroutine(FindActionBtn());
    }

    // Buscamos los botones para que automaticamente los pulse
    //IEnumerator FindActionBtn()
    //{

    //    yield return new WaitForSeconds(0.5f);

    //   //tiene que recibir el boolean

    //    // Mientras el agente tenga vida...
    //    while (agent.Info.HP > 0)
    //    {
    //        yield return new WaitForSeconds(0.5f);
    //        SearchBtn();
    //        int rand = Random.Range(0, 4);

    //        var att = actionBtn[rand].GetComponent<ActionButtonAttack>();

    //        // Si la energia que tiene el player es superior a la que consume el ataque, se ejecuta
    //        if (att.TheAttack.AttackMade.Energy < agent.Info.Energy)
    //        {
    //            att.OnClick();
    //            print("he elgido " + actionBtn[rand].name);
    //        }
    //        else
    //        {
    //            // Si no, recargamos energia
    //            actionBtn[3].GetComponent<ActionButtonAttack>().OnClick();
    //        }
    //    }



    //}

    void SearchBtn()
    {
        string gameobj = "Soft-Dummy-Hard-Rest";
        string[] cache = gameobj.Split('-');
        
        for (int i = 0; i < actionBtn.Length; i++)
        {
            actionBtn[i] = GameObject.Find(cache[i]);
            actionBtn[i].GetComponent<Button>().image.enabled = false;
            actionBtn[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        
    }
}