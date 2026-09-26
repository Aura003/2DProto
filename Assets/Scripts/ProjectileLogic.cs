using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    float lifeTime = 5f;
    float speed = 5f;
    Animator projectileAnimator;
    Rigidbody2D projRbdy;
    bool hasCollided = false;
    Vector2 direction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        projectileAnimator=this.GetComponent<Animator>();
        projRbdy=this.GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, lifeTime);
    }
    public Vector2 SetDirection(Vector2 dir)
    {
        return direction = dir;
    }
    void FixedUpdate()
    {
        if (hasCollided)
             projRbdy.linearVelocity = new Vector2(0, projRbdy.linearVelocity.y);
        else
            projRbdy.linearVelocity = new Vector2(direction.x * speed, projRbdy.linearVelocity.y);      
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hasCollided = true;
        projectileAnimator.SetTrigger("explode");
        Destroy(this.gameObject,1f);
    }
}
