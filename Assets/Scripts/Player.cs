using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float moveSpeed = 8f;
    GameObject currentFloor;
    [SerializeField] int Hp;
    [SerializeField] GameObject HpBar;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Button replayButton;
    int score;
    float scoreTime;
    SpriteRenderer render;
    Animator anim;
    AudioSource deathSound;
    // Start is called before the first frame update
    void Start()
    {
        Hp = 10;
        replayButton.gameObject.SetActive(false);
        render = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(-moveSpeed*Time.deltaTime,0,0);
            render.flipX = true;
            anim.SetBool("Run",true);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(moveSpeed*Time.deltaTime,0,0);
            render.flipX = false;
            anim.SetBool("Run",true);
        }
        else
        {
            anim.SetBool("Run",false);
        }
        UpdateScore();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Floor1")
        {
            if(other.contacts[0].normal == Vector2.up)
            {
                currentFloor = other.gameObject;
                ModifyHp(1);
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(other.gameObject.tag == "Floor2")
        {
            if(other.contacts[0].normal == Vector2.up)
            {
                currentFloor = other.gameObject;
                ModifyHp(-3);
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if(other.gameObject.tag == "Ceiling")
        {
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            ModifyHp(-3);
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "DeathLine")
        {
            Die();
            deathSound.Play();
        }
    }

    void ModifyHp(int num)
    {
        Hp += num;
        if(Hp > 10)
        {
            Hp = 10;
        }
        else if(Hp <= 0)
        {
            Hp = 0;
            Die();
            deathSound.Play();
        }
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        for(int i=0; i<HpBar.transform.childCount;i++)
        {
            if(Hp>i)
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                HpBar.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    void UpdateScore()
    {
        scoreTime += Time.deltaTime;
        if(scoreTime > 2f)
        {
            score ++;
            scoreTime = 0f;
            scoreText.text = "Score\n" + score.ToString();
        }
    }

    void Die()
    {
        Time.timeScale = 0;
        replayButton.gameObject.SetActive(true);
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}
