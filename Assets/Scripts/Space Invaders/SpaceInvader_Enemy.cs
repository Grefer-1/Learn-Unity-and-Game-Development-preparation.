using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceInvader_Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    public int score { get; private set; }

    private GameObject spawnedBullet;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetScore(int score) => this.score = score;

    public void Fire()
    {
        if (spawnedBullet == null)
        {
            spawnedBullet = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y - 0.5f, 0f), Quaternion.identity, transform);
            spawnedBullet.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SpaceInvader_GameManager.Instance.AddScore(score);
        gameObject.SetActive(false);
        Destroy(collision.gameObject);
    }
}
