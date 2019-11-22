using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity
{
    public float x;
    public float y;

    public Velocity(Vector2 vec)
    {
        x = vec.x;
        y = vec.y;
    }

    public void Set(Vector2 vec)
    {
        x = vec.x;
        y = vec.y;
    }

    public void Mul(float mul)
    {
        x *= mul;
        y *= mul;
    }

    public float Length()
    {
        return Mathf.Sqrt(x * x + y * y);
    }

    public Vector2 Get()
    {
        return new Vector2(x, y);
    }
}

public class Bomb : MonoBehaviour {
    
    GroundController groundController;

    public AudioClip clipBomb;
    public int remain;
    public int radius;

    // 편의를 위해 구조체가 아닌 클래스를 사용
    Velocity velocity;

	// Use this for initialization
	void Start () {
        GameObject ground = GameObject.Find("ground");
        if (ground) groundController = ground.GetComponent<GroundController>();
    }
	
	// Update is called once per frame
	void Update () {
        velocity.y -= 6f * Time.deltaTime;

        float current_x = transform.position.x;
        float current_y = transform.position.y;
        float diff_x = velocity.x * Time.deltaTime;
        float diff_y = velocity.y * Time.deltaTime;
        float change_x = current_x + diff_x;
        float change_y = current_y + diff_y;

        int result_x = 0;
        int result_y = 0;
        while (groundController.CheckCollisionBetween(current_x*80, current_y * 80, change_x * 80, change_y * 80, ref result_x, ref result_y) == true)
        {
            Tool.EmitWeaponSound(clipBomb);
            groundController.ExplodeGround(result_x, result_y, radius);
            Tool.CreateExplosionParticle(new Vector3(0.0125f * result_x, 0.0125f * result_y, -5), 40);

            if (--remain == 0)
            {
                Destroy(gameObject);

                // get next turn player
                GameDirector.GetNextTank().NextTurn(1);
                return;
            }
        }

        int dir = GetComponent<SpriteRenderer>().flipX ? 1 : -1;
        float angle = Mathf.Atan2(velocity.y * dir, velocity.x * dir) * 180 / Mathf.PI;

        transform.Translate(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, 0, Space.World);
        transform.rotation = Quaternion.Euler(0, 0, angle);
		if (transform.position.x < -10 || transform.position.x > 19.2f ||  transform.position.y < -3)
        {
            Destroy(gameObject);
            // get next turn player
            GameDirector.GetNextTank().NextTurn(1);
            return;
        }
        Camera.main.GetComponent<CameraController>().FollowCamera(transform.position);
    }

    public static Bomb Create(Vector3 position, float rotation, int power, AudioClip clip, int dir)
    {
        Bomb bomb = Instantiate(Resources.Load<GameObject>("Prefabs/bomb"), position, Quaternion.Euler(0, 0, rotation)).GetComponent<Bomb>();

        Vector2 velocityVector = new Vector2(Mathf.Cos(rotation / 180 * Mathf.PI), Mathf.Sin(rotation / 180 * Mathf.PI));
        velocityVector = velocityVector.normalized * power * 0.05f;
        bomb.velocity = new Velocity(velocityVector);
        bomb.clipBomb = clip;

        if (dir == 1)
        {
            bomb.GetComponent<SpriteRenderer>().flipX = true;
        }

        return bomb;
    }
}
