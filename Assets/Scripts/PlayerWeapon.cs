using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


//oyuncunun silah scripti
public class PlayerWeapon : MonoBehaviour
{
    [Header("Stats")]
    public int damage;
    public int curAmmo;
    public int maxAmmo;
    public float bulletSpeed;
    public float shootRate;

    private float lastShootTime;

    public GameObject bulletPrefab;
    public Transform bulletSpawnPos;

    private PlayerController player;

    void Awake ()
    {
        
        player = GetComponent<PlayerController>();
    }

    public void TryShoot ()
    {
        // ates etme
        if(curAmmo <= 0 || Time.time - lastShootTime < shootRate)
            return;

        curAmmo--;
        lastShootTime = Time.time;

        // arayuz
        GameUI.instance.UpdateAmmoText();

        // mermi türetme
        player.photonView.RPC("SpawnBullet", RpcTarget.All, bulletSpawnPos.position, Camera.main.transform.forward);
    }

    [PunRPC]
    void SpawnBullet (Vector3 pos, Vector3 dir)
    {
        // mermi olustur
        GameObject bulletObj = Instantiate(bulletPrefab, pos, Quaternion.identity);
        bulletObj.transform.forward = dir;

        
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        // mermi fırlar ve hızını ayarla
        bulletScript.Initialize(damage, player.id, player.photonView.IsMine);
        bulletScript.rig.velocity = dir * bulletSpeed;
    }

    //kit alındıgında
    [PunRPC]
    public void GiveAmmo (int ammoToGive)
    {
        curAmmo = Mathf.Clamp(curAmmo + ammoToGive, 0, maxAmmo);

        // guncelle
        GameUI.instance.UpdateAmmoText();
    }
}