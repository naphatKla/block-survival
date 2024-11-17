using UnityEngine;

public class CombatSystem : Singleton<CombatSystem>
{
    #region Declare Variables
    [Header("Bullet Type")]
    [SerializeField] public Bullet bullet;
    [SerializeField] private Bullet assaultRifleBullet;
    [SerializeField] public Bullet shotgunBullet;
    [SerializeField] public Bullet sniperBullet;
    [SerializeField] public Bullet missileBullet;
    [SerializeField] public Bullet sword;
    [SerializeField] public GameObject explode;
    [SerializeField] private AudioSource shootingSoundEffect;
    public PlayerClass playerClass;
    
 
    private Player _player;
    private Level _level;
    public float bulletSpeed;
    public float bulletOfsetScale;

    public enum PlayerClass
    {
        Default,
        AssaultRifle,
        Shotgun,
        Sniper,
        Missile,
        Sword
    }

    #endregion
    
    #region Unity Method
    void Start()
    {
        _level = GetComponent<Level>();
        bulletSpeed = _level.defaultData.bulletSpeed;
        bulletOfsetScale = _level.defaultData.bulletOffSetScale;
        _player = GetComponent<Player>();
        playerClass = PlayerClass.Default;
        Invoke("BulletSpawn", _player.PlayerAttackSpeed);
    }
    void Update()
    {
        
    }
    #endregion

    
    #region Method
    private void BulletSpawn()
    {
        switch (playerClass)
        {
            case PlayerClass.Default:
                BulletDefaultPatternSpawn();
                break;
            case PlayerClass.Shotgun:
                BulletShotGunPatternSpawn();
                break;
            case PlayerClass.AssaultRifle:
                BulletAssaultRiflePatternSpawn();
                break;
            case PlayerClass.Sniper:
                BulletSniperPatternSpawn();
                break;
            case PlayerClass.Missile:
                BulletMissilePatternSpawn();
                break;
            case PlayerClass.Sword:
                BulletSwordPatternSpawn();
                break;
        }
        shootingSoundEffect.Play();
        Invoke("BulletSpawn", _player.PlayerAttackSpeed);
    }
    
    private void BulletDefaultPatternSpawn()
    {
        Vector3 bulletOffSet = _player.playerTransform.up * bullet.bulletOffSetScale;
        GameObject bulletSpawn = Instantiate(bullet.gameObject, _player.playerTransform.position + bulletOffSet, _player.playerTransform.rotation);
        bulletSpawn.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
    }

    private void BulletShotGunPatternSpawn()
    {
        Vector3 bulletOffSet = _player.playerTransform.up * _level.shotgunData.bulletOffSetScale;
        float angle = 30;

        for (int i = 0; i < 5; i++)
        {
            GameObject bulletSpawn = Instantiate(shotgunBullet.gameObject, _player.playerTransform.position + bulletOffSet, _player.playerTransform.rotation * Quaternion.Euler(0,0,angle));
            angle -= 15;
            bulletSpawn.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
        }
    }
    private void BulletAssaultRiflePatternSpawn()
    {
        Vector3 bulletOffSet = _player.playerTransform.up * _level.assaultRifleData.bulletOffSetScale;
        GameObject bulletSpawn = Instantiate(assaultRifleBullet.gameObject, _player.playerTransform.position + bulletOffSet, _player.playerTransform.rotation);
        bulletSpawn.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
    }
    
    private void BulletSniperPatternSpawn()
    {
        Vector3 bulletOffSet = _player.playerTransform.up * _level.sniperData.bulletOffSetScale;
        Debug.Log(sniperBullet.bulletOffSetScale);
        GameObject bulletSpawn = Instantiate(sniperBullet.gameObject, _player.playerTransform.position + bulletOffSet, _player.playerTransform.rotation);
        bulletSpawn.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
    }
    
    private void BulletMissilePatternSpawn()
    {
        Vector3 bulletOffSet = _player.playerTransform.up * _level.missileData.bulletOffSetScale;
        GameObject bulletSpawn = Instantiate(missileBullet.gameObject, _player.playerTransform.position + bulletOffSet, _player.playerTransform.rotation);
        bulletSpawn.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
    }
    
    private void BulletSwordPatternSpawn()
    {
        Vector3 bulletOffSet = _player.playerTransform.up * _level.swordData.bulletOffSetScale;
        float angle = 75;

        for (int i = 0; i < 3; i++)
        {
            GameObject bulletSpawn = Instantiate(sword.gameObject, _player.playerTransform.position + bulletOffSet, _player.playerTransform.rotation * Quaternion.Euler(0,0,angle));
            bulletSpawn.GetComponent<Bullet>().bulletSpeed = bulletSpeed;
            Destroy(bulletSpawn,0.2f);
            angle -= 75;
        }
    }
    #endregion
}
