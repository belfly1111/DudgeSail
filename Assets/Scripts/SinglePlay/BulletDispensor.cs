using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDispensor : MonoBehaviour
{
    public GameObject bullet_prefab;
    public GameObject player;
    public float FireDelay = 3f;
    public float curTime = 0;

    // 1초 간격으로 총알 발사
    void Update()
    {
        curTime += Time.deltaTime;
        if(curTime > FireDelay)
        {
            GameObject bullet = Instantiate(bullet_prefab, this.transform.position, this.transform.rotation);
            Vector2 direction = (player.transform.position - this.transform.position).normalized;

            bullet.GetComponent<Rigidbody2D>().velocity = direction * 3f;
            curTime = 0;
            StartCoroutine(BulletLifetime(bullet));
        }
    }
    IEnumerator BulletLifetime(GameObject bullet)
    {
        yield return new WaitForSeconds(5f);
        Destroy(bullet);
    }
}
