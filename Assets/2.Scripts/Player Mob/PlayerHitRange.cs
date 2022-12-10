using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitRange : MonoBehaviour
{
    public GameObject Player;
    public MobGenerater mobGenerater;
    public List<Vector3> curPos;
    public GameObject curTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject collide = other.gameObject;
        int damage;
        if(collide.tag == "AttackRange")
        {
            //Mobindex && attackRange
            //고블린
            if (collide.transform.parent.gameObject.GetComponent<Mob>().index == 1 && collide.transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<AttackRange>().AttackPos)
            {
                collide.transform.parent.transform.GetChild(0).gameObject.GetComponent<AttackRange>().Attacked();
                damage = collide.transform.parent.GetComponent<Mob>().attackDamage;
                Player.GetComponent<Player>().getDamage(damage);
            }
            //투척
            if (collide.transform.parent.gameObject.GetComponent<Mob>().index == 2 && collide.transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<AttackRange>().AttackPos)
            {
                collide.transform.parent.transform.GetChild(0).gameObject.GetComponent<AttackRange>().Attacked();
                damage = collide.transform.parent.GetComponent<Mob>().attackDamage;
                Player.GetComponent<Player>().getDamage(damage);
            }
            //bullet
            if (collide.transform.parent.gameObject.GetComponent<Mob>().index == 99 && collide.transform.parent.gameObject.transform.GetChild(0).gameObject.GetComponent<AttackRange>().AttackPos)
            {
                collide.transform.parent.transform.GetChild(0).gameObject.GetComponent<AttackRange>().Attacked();
                damage = collide.transform.parent.GetComponent<Bullet>().damage;
                Player.GetComponent<Player>().getDamage(damage);
                Player.GetComponent<Player>().gameManager.GetComponent<GameManager>().attackDamage -= 1;
                Player.GetComponent<Player>().gameManager.GetComponent<GameManager>().bulletDamage += 1;
                collide.transform.parent.GetComponent<Bullet>().destroyBullet();
            }
        }

        if (collide.tag == "Tile")
        {
            curTile = collide;
        }

        curPlayerPos();
    }

    public void curPlayerPos()
    {
        //왼쪽 위 오른쪽 위
        //왼쪽 아래 오른쪽 아래
        List<Vector3> arr = new List<Vector3>();
        double[] dX = { -0.25, 0.25, 0.25, -0.25 };
        double[] dZ = { 0.25, 0.25, -0.25, -0.25 };
        Vector3 playerPos = Player.transform.position;

        for (int i = 0; i < 4; i++)
        {
            arr.Add(new Vector3(playerPos.x + (float)dX[i], 0, playerPos.z + (float)dZ[i]));
        }

        curPos = arr;
    }
}
