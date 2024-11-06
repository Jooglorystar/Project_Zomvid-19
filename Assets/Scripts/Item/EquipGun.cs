using UnityEngine;
using static UnityEditor.Progress;

public class EquipGun : Equip
{
    [SerializeField] private ParticleSystem gunFlash;
    [SerializeField] private Transform bulletPos;

    [SerializeField] private AudioClip FireClip;
    private AudioSource audioSource;

    private float attackRate;
    private bool isAttacking = false;

    public override void Start()
    {
        base.Start();
        if (bulletPos == null)
        {
            bulletPos = GameObject.Find("BulletPos").transform;
        }
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnAttackInput()
    {
        if (!isAttacking)
        {
            if (itemData.itemStack.itemSO is RangeEquipItemSO rangeWeaponData)
            {
                attackRate = rangeWeaponData.attackRate;
                isAttacking = true;
                Invoke("CanAttack", attackRate);

                SetBullet(rangeWeaponData);

                gunFlash.Play();
                audioSource.PlayOneShot(FireClip);
            }
        }
    }

    public void CanAttack()
    {
        isAttacking = false ;
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
