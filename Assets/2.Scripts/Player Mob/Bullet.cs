using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Mob
{
    public float delTime = 4f;
    public float bulletSpeed = 0.1f;
    public int damage = 1;
    public Vector3 mokpyo;
    public GameObject master;
    // Start is called before the first frame update
    void Start()
    {
        mokpyo = player.transform.position;
        StartCoroutine(DeleteTime(delTime));
    }

    // Update is called once per frame
    protected void Update()
    {
        if (master == null)
            destroyBullet();
        else
            transform.position += (mokpyo - master.transform.position) * bulletSpeed * Time.deltaTime;
    }

    public IEnumerator DeleteTime(float delTime)
    {
        while (delTime > 0.0f)
        {
            delTime -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        destroyBullet();
    }
    public void destroyBullet()
    {
        Destroy(gameObject);
    }

}
