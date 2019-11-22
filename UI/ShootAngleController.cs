using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAngleController : MonoBehaviour
{

    GameObject tank;
    SpriteRenderer tankRenderer;
    TankController tankController;
    bool isSetTank;

    // Use this for initialization
    void Start()
    {
        isSetTank = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isSetTank == false || tank == null)
            return;


        int dir = tankRenderer.flipX ? 1 : -1;
        transform.localScale = new Vector3(dir, 1, 1);
        int shootAngle = tankController.tankData.m_iCurrentAngle;
        transform.rotation = Quaternion.Euler(0, 0, tank.transform.rotation.eulerAngles.z + (shootAngle * dir));
    }

    public void SetMyTank(GameObject tank)
    {
        this.tank = tank;
        isSetTank = true;

        tankRenderer = tank.GetComponent<SpriteRenderer>();
        tankController = tank.GetComponent<TankController>();
    }
}
