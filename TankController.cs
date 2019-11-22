using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour {

    // tank constant variable
    readonly float dropSpeed = 6f;
    readonly float moveSpeed = 1f;
    readonly int maxPower = 200;

    // audio clip
    readonly int clip_move = 0;
    readonly int clip_fire = 2;
    readonly int clip_bomb = 4;

    // ui constant variable
    readonly float smallGaugeWidth = 150f;
    readonly float bigGaugeWidth = 570f;


    // 변수를 사용하기위해 기존 프로퍼티를 숨기는 키워드인 new를 사용
    new SpriteRenderer renderer;
    new BoxCollider2D collider;
    new AudioSource audio;
    Animator animator;
    CameraController cameraController;
    GroundController groundController;
    Image weaponImage;
    bool isAir;
    bool isTurn;
    int wallDirection; // 1 = left, 2 = right, 0 = nope

    public Tank tankData;
    GameObject powerGauge;
    GameObject moveGauge;

    // TEST
    //public GameObject minObject;
    //public GameObject maxObject;


    // Use this for initialization
    void Start () {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();
        audio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        cameraController = Camera.main.GetComponent<CameraController>();
        GameObject ground = GameObject.Find("ground");
        if (ground) groundController = ground.GetComponent<GroundController>();
        GameObject weapon = GameObject.Find("WeaponImage");
        if (weapon) weaponImage = weapon.GetComponent<Image>();

        wallDirection = 0;

        TankType myType = (TankType)Random.Range(0, 3);
        tankData = new Tank(myType);
        //switch(myType)
        //{
        //    case TankType.Cannon:

        //        break;
        //}
        Sprite sprite = Resources.Load<Sprite>("Images/" + tankData.GetTankPath() + "/Stand/01");
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animations/" + tankData.GetTankPath() + "/stand");
        renderer.sprite = sprite;
        powerGauge = GameObject.Find("PowerGauge");
        moveGauge = GameObject.Find("MoveGauge");
    }
	
	// Update is called once per frame
	void Update ()
    {
        float angle = transform.rotation.eulerAngles.z;
        if (isTurn && !isAir)
        {
            // TODO : GetKeyDown will be changed GetKey
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (angle < 285 && angle >= 270 || wallDirection == 1 || tankData.m_iRemainMove <= 0)
                {
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (angle > 75 && angle <= 90 || wallDirection == 2 || tankData.m_iRemainMove <= 0)
                {
                    audio.Stop();
                    audio.clip = tankData.m_audioClips[clip_move + 1];
                    audio.loop = false;
                    audio.Play(0);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (renderer.flipX == true)
                    renderer.flipX = false;

                if (angle < 285 && angle >= 270 || wallDirection == 1 || tankData.m_iRemainMove <= 0)
                {
                    if(audio.clip != tankData.m_audioClips[clip_move + 1])
                    {
                        audio.Stop();
                        audio.clip = tankData.m_audioClips[clip_move + 1];
                        audio.loop = false;
                        audio.Play(0);
                    }
                    return;
                }

                float remainGauge = (float)(--tankData.m_iRemainMove) / tankData.m_iMaxMove * smallGaugeWidth;
                SetGaugeFill(moveGauge.transform.Find("Fill").GetComponent<RectTransform>(), remainGauge);
                transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0), Space.Self);
                CalculateAngle();
                cameraController.FollowCamera(transform.position);

                if (audio.clip != tankData.m_audioClips[clip_move])
                {
                    audio.Stop();
                    audio.clip = tankData.m_audioClips[clip_move];
                    audio.loop = true;
                    audio.Play(0);
                }
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                if (renderer.flipX == false)
                    renderer.flipX = true;

                if (angle > 75 && angle <= 90 || wallDirection == 2 || tankData.m_iRemainMove <= 0)
                {
                    if (audio.clip != tankData.m_audioClips[clip_move + 1])
                    {
                        audio.Stop();
                        audio.clip = tankData.m_audioClips[clip_move + 1];
                        audio.loop = false;
                        audio.Play(0);
                    }
                    return;
                }

                float remainGauge = (float)(--tankData.m_iRemainMove) / tankData.m_iMaxMove * smallGaugeWidth;
                SetGaugeFill(moveGauge.transform.Find("Fill").GetComponent<RectTransform>(), remainGauge);
                transform.Translate(new Vector3(Time.deltaTime * moveSpeed, 0, 0), Space.Self);
                CalculateAngle();
                cameraController.FollowCamera(transform.position);
                
                if (audio.clip != tankData.m_audioClips[clip_move])
                {
                    audio.Stop();
                    audio.clip = tankData.m_audioClips[clip_move];
                    audio.loop = true;
                    audio.Play(0);
                }
            }
            else
            {
                if (audio.clip)
                {
                    audio.Stop();
                    audio.clip = null;
                }
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (tankData.m_iCurrentAngle >= tankData.m_iMaxAngle)
                    return;

                // angle change sound
                tankData.m_iCurrentAngle++;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (tankData.m_iCurrentAngle <= tankData.m_iMinAngle)
                    return;

                // angle change sound
                tankData.m_iCurrentAngle--;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                RectTransform fill = powerGauge.transform.Find("Fill").GetComponent<RectTransform>();
                int current = tankData.m_iShootingValue;
                if (tankData.m_isShooting == false)
                {
                    tankData.m_isShooting = true;
                    tankData.m_iShootingValue = 0;

                    RectTransform lastFill = powerGauge.transform.Find("LastFill").GetComponent<RectTransform>();
                    float gauge = fill.sizeDelta.x;
                    if (gauge > 40)
                        SetGaugeFill(lastFill, gauge);
                    else
                        SetGaugeFill(lastFill, fill.GetComponent<Image>().fillAmount * 40);
                }
                else
                {
                    if (++current > maxPower)
                        current = maxPower;

                    tankData.m_iShootingValue = current;
                    SetGaugeFill(fill, current * bigGaugeWidth / maxPower);
                }
            }
            else
            {
                if(tankData.m_isShooting)
                {
                    tankData.m_isShooting = false;
                    isTurn = false;
                    // shoot bomb
                    Tool.EmitWeaponSound(tankData.m_audioClips[clip_fire + tankData.m_iBombType]);

                    cameraController.StopCameraSmoothMove();

                    float shootAngle = (renderer.flipX ? angle + tankData.m_iCurrentAngle : 180 + angle - tankData.m_iCurrentAngle);
                    Bomb bomb = Bomb.Create(collider.bounds.center, shootAngle, tankData.m_iShootingValue, tankData.m_audioClips[clip_bomb + tankData.m_iBombType], (renderer.flipX ? 1 : -1));
                    bomb.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/UI/Weapons/" + tankData.GetTankPath() + "/" + tankData.m_iBombType);

                    bomb.radius = tankData.m_iBombRadius[tankData.m_iBombType];
                    bomb.remain = tankData.m_iBombCount[tankData.m_iBombType];
                    cameraController.FollowCamera(bomb.transform.position, true);
                    
                    RectTransform lastFill = powerGauge.transform.Find("LastFill").GetComponent<RectTransform>();
                    SetGaugeFill(lastFill, 0);

                }
            }
        }
        if(transform.position.y <= 0)
        {
            tankData.m_isDeadTank = true;
            if (isTurn)
            {
                TankController tank = GameDirector.GetNextTank();
                if (tank) tank.NextTurn(1);
                else
                {
                    // Game End
                }

                audio.Stop();
                isTurn = false;
            }
            //gameObject.SetActive(false);
            renderer.color = new Color(1, 1, 1, 0);
            return;
        }

        int groundHeight = groundController.GetGroundHeight(transform.position);
        if(groundHeight == int.MinValue)
        {
            Debug.Log("can't get ground height " + transform.position);
            return;
        }
        // 움직이다가 땅속으로 빠지는것 방지
        if(!isAir && groundHeight < -2 && !((angle > 89 && angle < 91) || (angle > 269 && angle < 271)))
        {
            transform.Translate(new Vector3(0, 0.0125f * -groundHeight), Space.World);
            groundHeight = groundController.GetGroundHeight(transform.position);
        }
        if (groundHeight > 0)
        {
            if (groundHeight > 30)
            {
                cameraController.FollowCamera(transform.position);

                // angle reset
                transform.rotation = Quaternion.Euler(new Vector3());
                audio.Stop();
            }
            else
            {
                CalculateAngle();
            }
            float maxDropHeight =  (float)groundHeight / 80 * -1;
            transform.Translate(new Vector3(0, Mathf.Max(maxDropHeight, -Time.deltaTime * dropSpeed), 0), Space.World);
            //transform.position = new Vector3(transform.position.x, (float)((int)(transform.position.y * 80)) / 80);

            isAir = true;
            return;
        }

        if(isAir)
        {
            CalculateAngle();
        }
    }

    void CalculateAngle()
    {
        //Vector3 min = new Vector3(), max = new Vector3();
        //min.x = transform.position.x - 0.1f;
        //max.x = transform.position.x + 0.1f;
        //max.y = min.y = transform.position.y;

        float angle = transform.rotation.eulerAngles.z;
        Vector3 max = new Vector3(Mathf.Cos(angle / 180 * Mathf.PI) * 0.05f, Mathf.Sin(angle / 180 * Mathf.PI) * 0.05f, 0);
        Vector3 min = new Vector3(-max.x, -max.y, 0);
        max += transform.position;
        min += transform.position;

        // calculate angle
        int leftGroundHeight = groundController.GetGroundHeight(min) - (int)(min.y * 80);
        int rightGroundHeight = groundController.GetGroundHeight(max) - (int)(max.y * 80);
        //if (leftGroundHeight < 0 || rightGroundHeight < 0)
        //{
        //    float adjust = 0f;
        //    if (leftGroundHeight < 0 && rightGroundHeight < 0)
        //        adjust = Mathf.Abs(leftGroundHeight - rightGroundHeight) * 0.0125f;
        //    if (leftGroundHeight < 0)
        //        adjust = 0.0125f * leftGroundHeight;
        //    else
        //        adjust = 0.0125f * rightGroundHeight;

        //    min.y += adjust;
        //    max.y += adjust;

        //    leftGroundHeight = groundController.GetGroundHeight(min);
        //    rightGroundHeight = groundController.GetGroundHeight(max);
        //    Debug.Log(leftGroundHeight.ToString() + " " + rightGroundHeight.ToString());
        //}
        //minObject.transform.position = min;
        //maxObject.transform.position = max;
        int distHeight = leftGroundHeight - rightGroundHeight;
        float rotation = Mathf.Atan2(distHeight, Mathf.Abs(max.x - min.x) * 80) / Mathf.PI * 180.0f;
        if (rotation > 75 && renderer.flipX)
        {
            wallDirection = 2;
            rotation = 0;
        }
        else if(rotation < -75 && !renderer.flipX)
        {
            wallDirection = 1;
            rotation = 0;
        }
        else
        {
            wallDirection = 0;
        }
        transform.rotation = Quaternion.Euler(0, 0, Mathf.RoundToInt(rotation / 3) * 3);

        isAir = false;
        // check turn
        // player control
    }

    public void HasTurn()
    {
        isTurn = true;
        RectTransform move = moveGauge.transform.Find("Fill").GetComponent<RectTransform>();
        tankData.m_iRemainMove = tankData.m_iMaxMove;
        weaponImage.sprite = Resources.Load<Sprite>("Images/UI/Weapons/" + tankData.GetTankPath() + "/" + tankData.m_iBombType);
        SetGaugeFill(move, 150);
        cameraController.SetCameraSmoothMove(transform.position, 5f, true);

        GameObject angleController = GameObject.Find("GroundAngle");
        angleController.GetComponent<GroundAngleController>().SetMyTank(gameObject);
        angleController = GameObject.Find("GroundAngleText");
        angleController.GetComponent<GroundAngleTextController>().SetMyTank(gameObject);
        angleController = GameObject.Find("MinAngle");
        angleController.GetComponent<TankAngleController>().SetMyTank(gameObject, tankData.m_iMinAngle);
        angleController = GameObject.Find("MaxAngle");
        angleController.GetComponent<TankAngleController>().SetMyTank(gameObject, tankData.m_iMaxAngle);
        angleController = GameObject.Find("ShootAngle");
        angleController.GetComponent<ShootAngleController>().SetMyTank(gameObject);
        angleController = GameObject.Find("ShootAngleText");
        angleController.GetComponent<ShootAngleTextController>().SetMyTank(gameObject);
    }

    public void NextTurn(float delay)
    {
        StartCoroutine(_NextTurn(delay));
    }

    IEnumerator _NextTurn(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        HasTurn();
    }

    public void ShowMe()
    {
        cameraController.SetCameraSmoothMove(transform.position, 5f, true);
    }

    public void SetGaugeFill(RectTransform rt, float value)
    {
        Image img = rt.GetComponent<Image>();
        if (value > 40)
        {
            if(img.type != Image.Type.Sliced)
                img.type = Image.Type.Sliced;

            rt.sizeDelta = new Vector2(value, 40);
        }
        else
        {
            if (img.type != Image.Type.Filled)
                img.type = Image.Type.Filled;

            img.fillMethod = Image.FillMethod.Horizontal;
            img.fillAmount = value / 40f;
            rt.sizeDelta = new Vector2(40, 40);
        }
    }
}
