
using UnityEngine;

public class TargetTrigger : MonoBehaviour
{
    public Transform Target;

    private bool imAlive = false;

    public float DistanceToTarget;
    //usamos delegados
    public delegate void SetResult(float result);
    public event SetResult OnHitCollider;

    // Start is called before the first frame update
    public void OnCollisionEnter(Collision col)
    {
        Debug.DrawRay(transform.position, Target.transform.position - transform.position,Color.red,10f); 
        DistanceToTarget =Vector3.Distance(transform.position, Target.transform.position);
        OnHitCollider(DistanceToTarget);
        imAlive = true;
    }

    public void Update()
    {
        if (imAlive) gameObject.SetActive(false);
          
    }
}
