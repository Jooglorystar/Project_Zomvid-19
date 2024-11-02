using UnityEngine;

public class Fence : MonoBehaviour, IDamagable
{
    public float Capacity = 30;

    public void TakeDamage(float damage)
    {
        Debug.Log("펜스가 데미지를 입었습니다.");
        Capacity = Mathf.Clamp(Capacity - damage, 0, Capacity);
        if(Capacity == 0)
        {
            Destroy(gameObject);
        }
    }
}