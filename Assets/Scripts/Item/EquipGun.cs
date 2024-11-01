using UnityEngine;

public class EquipGun : Equip
{
    [SerializeField] private ItemSO itemData;
    [SerializeField] private ParticleSystem gunFlash;
    [SerializeField] private Transform bulletPos;

    public override void OnAttackInput()
    {
        if (itemData is RangeEquipItemSO rangeWeaponData)
        {
            SetBullet(rangeWeaponData);
            gunFlash.Play();
        }
    }

    private void SetBullet(RangeEquipItemSO rangeWeaponData)
    {
        GameObject bullet = GameManager.Instance.objectPool.GetFromPool(PoolObject.bullet);
        bullet.SetActive(true);
        bullet.GetComponent<Bullet>().damage = rangeWeaponData.damage;
        bullet.transform.position = bulletPos.position;
    }

    public override void OnRunInput(bool isRunning)
    {

    }

}
