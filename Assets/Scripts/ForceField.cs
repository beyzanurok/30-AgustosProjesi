using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceField : MonoBehaviour
{
    public float shrinkWaitTime;
    public float shrinkAmount;
    public float shrinkDuration;
    public float minShrinkAmount;

    public int playerDamage;

    private float lastShrinkEndTime;
    private bool shrinking;
    private float targetDiameter;
    private float lastPlayerCheckTime;

    void Start ()
    {
        lastShrinkEndTime = Time.time;
        targetDiameter = transform.localScale.x;
    }

    void Update()
    {
        if(shrinking)
        {
            // kucultme baslıyor
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * targetDiameter, (shrinkAmount / shrinkDuration) * Time.deltaTime);

            //hedefe geldi mi diye kontrol yapılıyor
            if(transform.localScale.x == targetDiameter)
                shrinking = false;
        }
        else
        {
            //gelmediysse tekrar kucultme baslar
            if(Time.time - lastShrinkEndTime >= shrinkWaitTime && transform.localScale.x > minShrinkAmount)
                Shrink();
        }

        CheckPlayers();
    }

    void Shrink ()
    {
        shrinking = true;

        // minimum miktarın kontrolu
        if (transform.localScale.x - shrinkAmount > minShrinkAmount)
            targetDiameter -= shrinkAmount;
        else
            targetDiameter = minShrinkAmount;

        lastShrinkEndTime = Time.time + shrinkDuration;
    }

    void CheckPlayers ()
    {
        if(Time.time - lastPlayerCheckTime > 1.0f)
        {
            lastPlayerCheckTime = Time.time;

            
            foreach(PlayerController player in GameManager.instance.players)
            {
                // skip
                if(!player || player.dead)
                    continue;

                // güç alanının dışındaysa
                if (Vector3.Distance(Vector3.zero, player.transform.position) >= transform.localScale.x)
                {
                    // zarar ver
                    player.photonView.RPC("TakeDamage", player.photonPlayer, 0, playerDamage);
                }
            }
        }
    }
}