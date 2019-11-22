using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    GameObject ground;
    bool isAutoFollow;
    Vector3 mousePosition;


    bool useSmoothMove;
    Vector3 movePosition;
    bool isForceMove;
    float moveDelta;

	// Use this for initialization
	void Start () {
        isAutoFollow = true;
        ground = GameObject.Find("ground");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(useSmoothMove)
        {
            SmoothMoveCamera(transform.position, movePosition, moveDelta, isForceMove);

            if ((movePosition - transform.position).sqrMagnitude < 0.1f)
                useSmoothMove = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (ground.GetComponent<Collider>().Raycast(ray, out hit, 6) == false)
                return;

            mousePosition = hit.point;
        }
		if(Input.GetMouseButton(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (ground.GetComponent<Collider>().Raycast(ray, out hit, 6) == false)
                return;

            Vector3 delta = (hit.point - mousePosition) * -1; // 드래그 반대방향으로 화면 이동
            FollowCamera(delta + transform.position, true);
            
            // 움직이고 그 지점에서 raycast 한 위치를 deltaPosition 값으로 받아야한다.
            if(ground.GetComponent<Collider>().Raycast(ray, out hit, 6) == false) // 위에서 false가 아니면 밑에서도 false가 아니다. 하지만 안정성 문제로 체크해주자.
                mousePosition = hit.point;

            isAutoFollow = false;
        }
	}

    public void FollowCamera(Vector3 position, bool Force = false)
    {
        if (!isAutoFollow && !Force)
            return;

        if (Force) isAutoFollow = true;
        
        const float horizontal = 5.3f;
        const float vertical = 2f;
        const float vertical_ui = 1f;

        if (position.x < horizontal)
            position.x = horizontal;
        if (position.x > 19.2f - horizontal)
            position.x = 19.2f - horizontal;

        if (position.y < vertical - vertical_ui)
            position.y = vertical - vertical_ui;
        if (position.y > 10.5f - vertical)
            position.y = 10.5f - vertical;

        position.z = -10;

        Camera.main.transform.position = position;
    }

    public void SetCameraSmoothMove(Vector3 target, float delta, bool Force = false)
    {
        const float horizontal = 5.3f;
        const float vertical = 2f;
        const float vertical_ui = 1f;

        if (target.x < horizontal)
            target.x = horizontal;
        if (target.x > 19.2f - horizontal)
            target.x = 19.2f - horizontal;

        if (target.y < vertical - vertical_ui)
            target.y = vertical - vertical_ui;
        if (target.y > 10.5f - vertical)
            target.y = 10.5f - vertical;
        
        movePosition = target;
        moveDelta = delta;
        isForceMove = Force;
        isAutoFollow = true;
        useSmoothMove = true;
    }

    public void StopCameraSmoothMove()
    {
        useSmoothMove = false;
    }

    void SmoothMoveCamera(Vector3 position, Vector3 target, float delta, bool Force = false)
    {
        if (!isAutoFollow && !isForceMove)
            return;
        if (!isAutoFollow && isForceMove)
            isForceMove = false;

        Camera.main.transform.position = new Vector3(Mathf.Lerp(position.x, target.x, Time.deltaTime * delta),
                                           Mathf.Lerp(position.y, target.y, Time.deltaTime * delta),
                                           -10);
    }
}
