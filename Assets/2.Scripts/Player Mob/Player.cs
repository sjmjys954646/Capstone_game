using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Player_Mob_Frame
{
    public GameObject mapManager;
    public GameObject attackRange;
    public GameObject hitRange;
    public GameObject playerManager;
    new Rigidbody rigidbody;
    //�̵�����
    Vector3 movement;
    float h;
    float v;
    public bool DashPos = true;
    public float originalSpeed;
    public float dashInvincibilityTime;
    public float dashLength;
    public float dashCool;
    [SerializeField]
    bool coroutineCheck = false;

    List<KeyValuePair<int, Vector3>> stack = new List<KeyValuePair<int, Vector3>>();

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        originalSpeed = speed;
    }

    private void Start()
    {
        StartCoroutine(countTime());
    }

    private void Update()
    {
        //�뽬����
        if (DashPos && Input.GetMouseButtonDown(1))
        {
            Dash();
            StartCoroutine(DashCoolTime(dashCool));
        }

        //�˹�� �ӵ��� ����� ����ƴҶ�
        if (speed != originalSpeed)
        {
            if (!coroutineCheck)
            {
                StartCoroutine(insideCheck());
                coroutineCheck = true;
            }
        }

        //�ӵ��� �̻���������
        if (rigidbody.velocity.x != 0 || rigidbody.velocity.z != 0)
            rigidbody.velocity = Vector3.zero;

        //y��ǥ�� �̻���������
        if (!goDown && gameObject.transform.position.y != 0)
            gameObject.transform.position = new Vector3(transform.position.x,0, transform.position.z);

        //���ۿ� ��������
        if(goDown)
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.05f, transform.position.z);
        if(goDown && gameObject.transform.position.y <= -20)
        {
            getDamage(1);
            Respawn();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�̵� ����
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Run(h, v);
    }

    void Run(float h, float v)
    {
        movement.Set(h, 0, v);
        movement = movement.normalized*speed*Time.deltaTime;

        rigidbody.MovePosition(transform.position + movement);
    }

    public void getDamage(int damage)
    {
        //Player ����ó��
        if (!invincible)
            health -= damage;

        //ü�� 0 ���Ͻ� ���ó��
        if (health <= 0)
        {
            Killed();
        }
        invincible = true;
        StartCoroutine(invincibility(invincibilityTime));
    }

    void Dash()
    {
        float distance;
        Vector3 worldPosition = new Vector3(0,0,0);
        Vector3 pos;
        Plane plane = new Plane(Vector3.up, 0);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
        }

        invincible = true;
        StartCoroutine(invincibility(dashInvincibilityTime));
        pos = worldPosition - gameObject.transform.position;
        pos.y = 0;
        Vector3 movement = pos * dashLength;

        transform.Translate(movement, Space.Self);
    }

    public IEnumerator DashCoolTime(float cool)
    {
        DashPos = false;

        while (cool > 0.0f)
        {
            cool -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        DashPos = true;
    }

    IEnumerator countTime()
    {
        if(hitRange.GetComponent<PlayerHitRange>().curTile != null)
            stack.Add(new KeyValuePair<int, Vector3>(hitRange.GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().index,
                hitRange.GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().transform.position));
        if(stack.Count == 10)
        {
            stack.RemoveAt(0);
        }

        yield return new WaitForSeconds(1.0f);
        StartCoroutine(countTime());
    }

    public IEnumerator insideCheck()
    {
        float keepPosTime = 1f;
        while (keepPosTime > 0.0f)
        {
            keepPosTime -= Time.deltaTime;
            //Debug.Log(hitRange.GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().index);
            if (hitRange.GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().index != 2) // �� ��ǥ�� ���� �ƴϸ�
            {
                DashPos = true;
                speed = originalSpeed;
                coroutineCheck = false;
            }
            yield return new WaitForFixedUpdate();
        }
        coroutineCheck = false;
    }

    void Respawn()
    {
        bool respawnPos = false;
        for (int i = stack.Count - 1; i >= 0; i--)
        {
            if (stack[i].Key == 0)
            {
                transform.position = stack[i].Value;
                goDown = false;
                hitRange.GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().isPlayerIn = false;
                respawnPos = true;
                break;
            }
        }

        if(!respawnPos)
        {
            playerManager.GetComponent<PlayerManager>().SetPlayerPosition();
            hitRange.GetComponent<PlayerHitRange>().curTile.GetComponent<TileFrame>().isPlayerIn = false;
            goDown = false;
        }
    }

    void Killed()
    {
        //���ӿ����� ����
        Destroy(gameObject);
    }
}
