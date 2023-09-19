
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    private ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (ps && !ps.IsAlive())
        {
            DestroySelf();
        }
    }
    public void DestroySelf()
    {
        ObjectPool.Instance.PushObject(gameObject);
        //Destroy(gameObject);
    }
}
