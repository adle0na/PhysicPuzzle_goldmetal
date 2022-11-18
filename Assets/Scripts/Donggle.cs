using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Donggle : MonoBehaviour
    {
        public GameManager manager;
        public ParticleSystem effect;
        
        public int  level;
        public bool isDrag;
        public bool isMergy;
        
        private Rigidbody2D      _rigid;
        private CircleCollider2D _circle;
        private Animator         _anim;
        
        private void Awake()
        {
            _rigid  = GetComponent<Rigidbody2D>();
            _circle = GetComponent<CircleCollider2D>();
            _anim   = GetComponent<Animator>();
        }

        void OnEnable()
        {
            _anim.SetInteger("Level", level);
        }
        
        private void Update()
        {
            if(isDrag)
            {
                Vector3 mousePos  = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                float leftBorder  = -4.2f + transform.localScale.x / 2f;
                float rightBorder = 4.2f - transform.localScale.x / 2f;

                if (mousePos.x < leftBorder)
                    mousePos.x = leftBorder;
                else if (mousePos.x > rightBorder)
                    mousePos.x = rightBorder;

                mousePos.y = 8;
                mousePos.z = 0;
                transform.position = Vector3.Lerp(transform.position, mousePos, 0.2f);
            }
        }

        public void Drag()
        {
            isDrag = true;
        }

        public void Drop()
        {
            isDrag = false;
            _rigid.simulated = true;
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Donggle")
            {
                Donggle other = collision.gameObject.GetComponent<Donggle>();

                if (level == other.level && !isMergy && !other.isMergy && level < 7)
                {
                    float meX = transform.position.x;
                    float meY = transform.position.y;
                    float otherX = other.transform.position.x;
                    float otherY = other.transform.position.y;
                    // 내가 아래에 있는 경우
                    if (meY < otherY || (meY == otherY && meX > otherX))
                    {
                        other.Hide(transform.position);
                        
                        LevelUp();
                    }
                    // 같은 높이일 경우
                }

            }
        }

        public void Hide(Vector3 targetPos)
        {
            isMergy = true;

            _rigid.simulated = false;
            _circle.enabled = false;

            StartCoroutine(HideRoutine(targetPos));
        }

        IEnumerator HideRoutine(Vector3 targetPos)
        {
            int frameCount = 0;

            while (frameCount < 20)
            {
                frameCount++;
                transform.position = Vector3.Lerp(transform.position, targetPos, 0.5f);
                yield return null;        
            }

            isMergy = false;
            gameObject.SetActive(false);
        }

        void LevelUp()
        {
            isMergy = true;

            _rigid.velocity = Vector2.zero;
            _rigid.angularVelocity = 0;

            StartCoroutine(LevelUpRoutine());
        }

        IEnumerator LevelUpRoutine()
        {
            yield return new WaitForSeconds(0.2f);
            
            _anim.SetInteger("Level", level + 1);
            EffectPlay();

            yield return new WaitForSeconds(0.3f);
            level++;

            manager.maxLevel = Mathf.Max(level, manager.maxLevel);
            isMergy = false;
        }

        void EffectPlay()
        {
            effect.transform.position = transform.position;
            effect.transform.localScale = transform.localScale;
            effect.Play();
        }
    }
}