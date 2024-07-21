using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpaceInvader_Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private GameObject bullet;

    private GameObject spawnedBullet;

    private float elapsed;
    private float duration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.RightArrow))
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) && spawnedBullet == null)
        {
            spawnedBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y + 1f, 0f), Quaternion.identity, transform);
            spawnedBullet.SetActive(true);
        }

        elapsed += Time.deltaTime;
        if (elapsed / duration >= 1f)
        {
            RaycastHit2D[] hits_0 = Physics2D.RaycastAll(new Vector2(transform.position.x - 2.5f, transform.position.y + 1f), new Vector2(0f, 10f));
            RaycastHit2D[] hits_1 = Physics2D.RaycastAll(new Vector2(transform.position.x, transform.position.y + 1f), new Vector2(0f, 10f));
            RaycastHit2D[] hits_2 = Physics2D.RaycastAll(new Vector2(transform.position.x + 2.5f, transform.position.y + 1f), new Vector2(0f, 10f));
            if (hits_0 != null && hits_0.Length > 0 && hits_0[0].transform != null)
                hits_0.Where(hit => hit.transform.position.y == hits_0.Min(hit => hit.transform.position.y)).FirstOrDefault().transform.GetComponent<SpaceInvader_Enemy>().Fire();
            if (hits_1 != null && hits_1.Length > 0 && hits_1[0].transform != null)
                hits_1.Where(hit => hit.transform.position.y == hits_1.Min(hit => hit.transform.position.y)).FirstOrDefault().transform.GetComponent<SpaceInvader_Enemy>().Fire();
            if (hits_2 != null && hits_2.Length > 0 && hits_2[0].transform != null)
                hits_2.Where(hit => hit.transform.position.y == hits_2.Min(hit => hit.transform.position.y)).FirstOrDefault().transform.GetComponent<SpaceInvader_Enemy>().Fire();

            elapsed = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Bullet"))
            SpaceInvader_GameManager.Instance.GameOver();
    }
}
