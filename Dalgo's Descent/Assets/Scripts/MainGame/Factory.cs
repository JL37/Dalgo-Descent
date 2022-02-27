using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory
{
    public static void CreateCoins(Vector3 pos,int num = 1, float radius = 0.75f)
    {
        ObjectPoolManager pool = GameObject.Find("coinPool").GetComponent<ObjectPoolManager>();

        if (num == 1)
        {
            GameObject coin = pool.GetFromPool();
            Vector3 basePos = pos;
            basePos.y += 0.4f;
            coin.transform.position = basePos;
            return;
        }

        Vector3 dir = new Vector3(1, 0, 0);
        float degree = 360 / num;

        for (int i = 0; i < num; ++i)
        {
            GameObject coin = pool.GetFromPool();

            Vector3 basePos = pos;
            basePos.y += 0.4f;

            //Debug.Log("index i is " + i + " and basepos.y is " + basePos.y);

            coin.transform.position = basePos + (dir * radius);
            coin.transform.rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);

            //Rotate vector
            dir = Quaternion.Euler(0, degree, 0) * dir;
        }
    }
}
