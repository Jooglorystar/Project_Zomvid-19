using UnityEngine;

public class Campfire : MonoBehaviour
{
    public float warmTemperature = 25f;

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<PlayerCondition>(out PlayerCondition condition))
        {
            condition.nearFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerCondition>(out PlayerCondition condition))
        {
            condition.nearFire = false;
        }
    }
}