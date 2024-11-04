using UnityEngine;
using static UnityEditor.Progress;

public class EquipGun : Equip
{
    [SerializeField] private ParticleSystem gunFlash;
    [SerializeField] private Transform bulletPos;


    public override void Start()
    {
        base.Start();
        if (bulletPos == null)
        {
            bulletPos = GameObject.Find("BulletPos").transform;
        }
    }

    public override void OnAttackInput()
    {
        if (itemData.itemStack.itemSO is RangeEquipItemSO rangeWeaponData)
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
