using UnityEngine;
using System.Collections;

public class run_behaivor : MonoBehaviour
{
    public GameObject _l;
    public GameObject _r;
    public GameObject _h;
    public GameObject _box;

    public float flySpeed=10;
    public float walkSpeed = 5;

    PhotonView pv = null;
    SteamVR_TrackedObject _lo;
    SteamVR_TrackedObject _ro;
    SteamVR_TrackedObject _ho;

    Vector3 _l_start_localposition = Vector3.zero;
    Vector3 _l_delat_position = Vector3.zero;
    Vector3 _r_start_localposition = Vector3.zero;
    Vector3 _r_delat_position = Vector3.zero;
    Vector3 _h_start_localposition = Vector3.zero;
    Vector3 _h_delat_position = Vector3.zero;
    Vector3 _box_start_position = Vector3.zero;
    Vector3 _box_delat_position = Vector3.zero;

    float _lengthOfSpeedDelat = 0;//1.5-1.4 
    float _HighOfHandles = 0;
    float speed = 0;
    bool SFly = false;
    bool inAir = false;
    Vector3 direction=Vector3.zero;
    void Awake()
    {
        _lo = _l.GetComponent<SteamVR_TrackedObject>();
        _ro = _r.GetComponent<SteamVR_TrackedObject>();
        _ho = _h.GetComponent<SteamVR_TrackedObject>();
    }
    void Start()
    {
        _lengthOfSpeedDelat = (_l.transform.localPosition - _r.transform.localPosition).magnitude;
        _HighOfHandles = _l.transform.localPosition.y + _r.transform.localPosition.y;
       // Debug.Log(_HighOfHandles);
    }

    void FixedUpdate()
    {
        direction = Mdirection(_l,_r,_h).normalized;
        SwitchOfFly();
        if (SFly)
        {
           fly();
        }
        else
        {
           walk();
        }
        status();
        NatureMovement();
    }
    void Update()
    {
    }

    void status()
    {
        if (speed >= flySpeed || ((!inAir) && speed >= 0))
        {
            speed -= 1f * Time.deltaTime;
        }
        else if (inAir)
        {
            speed = flySpeed;
        }
        else
        {
            speed = 0;
        }
    }
    void SwitchOfFly()
    {
        if (speed >= flySpeed && SFly == false)
        {
            SFly = true;
            _box.GetComponent<Rigidbody>().AddForce((direction + new Vector3(0, 1500f, 0)) * 1f);
        }

        if (speed <= walkSpeed && SFly == true)
        {
            SFly = false;
            if (!inAir) speed = 0;
           // _box.GetComponent<Rigidbody>().AddForce((direction + new Vector3(0, -10, 0)) * 1f);
        }
    }
    void NatureMovement()
    {
        RaycastHit info;
        if ((!Physics.Raycast(_h.transform.position, -_h.transform.up, out info, 2f)) && _box.transform.position.y >= 0.2f)
        //if(info.collider.tag!="dimian")
        {
            inAir = true;
            _box.GetComponent<Rigidbody>().velocity += new Vector3(0,-0.1f);
            _box.transform.Translate(new Vector3(0, 0) + direction * 0.05f);
        }
        else
        {
            inAir = false;
            _box.GetComponent<Rigidbody>().velocity =Vector3.zero;
        }


           Debug.Log(info.collider.name);
    }
    void fly()
    {
        //Debug.Log(_box.GetComponent<Rigidbody>().velocity);
        var d = _l.transform.localPosition.y + _r.transform.localPosition.y - _HighOfHandles;
        d= Mathf.Clamp(d,-0.1f,1);
        if (d <= 0)
        {
           // _box.transform.Translate(new Vector3(0, -d/2f) + direction * Mathf.Abs(d) * 0.2f, Space.World);
            speed -= d;
            _box.GetComponent<Rigidbody>().AddForce((direction + new Vector3(0, -d*700f, 0)) );
        }
        else
        {
           // _box.GetComponent<Rigidbody>().AddForce((direction + new Vector3(0, -2f, 0)) );
        }
        _HighOfHandles = _l.transform.localPosition.y + _r.transform.localPosition.y;
    }

    void walk()
    {

        //  if (Mathf.Abs(_lengthOfSpeedDelat - (_l.transform.localPosition - _r.transform.localPosition).magnitude) >= 0.5)
        //   {
        // Debug.Log("ssss");\
        _box.transform.Translate(direction * 2 * Mathf.Abs((_l.transform.localPosition - _r.transform.localPosition).magnitude - _lengthOfSpeedDelat));
        speed += Mathf.Abs((_l.transform.localPosition - _r.transform.localPosition).magnitude - _lengthOfSpeedDelat);
        _lengthOfSpeedDelat = (_l.transform.localPosition - _r.transform.localPosition).magnitude;
        //   }

        /*
                var device_l = SteamVR_Controller.Input((int)_lo.index);
                var device_r = SteamVR_Controller.Input((int)_ro.index);
                var device_h = SteamVR_Controller.Input((int)_ho.index);

                if (device_l.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && device_r.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    _lengthOfSpeedDelat = (_l.transform.localPosition - _r.transform.localPosition).magnitude;
                }

                if (device_l.GetTouch(SteamVR_Controller.ButtonMask.Trigger) && device_r.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                {

                    _box.transform.Translate(Mdirection(_l, _r) * 2 * Mathf.Abs((_l.transform.localPosition - _r.transform.localPosition).magnitude - _lengthOfSpeedDelat));
                    _lengthOfSpeedDelat = (_l.transform.localPosition - _r.transform.localPosition).magnitude;
                }

        /*
                if (device_l.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    _l_start_localposition = _l.transform.localPosition;
                    _box_start_position = _box.transform.position;
                    _box.GetComponent<Rigidbody>().useGravity = false;
                }

                if (device_l.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
                {
                   _l_delat_position= _l.transform.localPosition - _l_start_localposition;
                   _box.transform.position = _box_start_position - _l_delat_position*2;
                }

                if (device_l.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
                {

                    _box.GetComponent<Rigidbody>().useGravity = true;
                    _box.GetComponent<Rigidbody>().AddForce( -_l.GetComponent<Rigidbody>().velocity*10000);
                }
         */
    }


    Vector3 Mdirection(GameObject l, GameObject r,GameObject h)
    {
        Vector3 s = (l.transform.forward + r.transform.forward)*0.5f+h.transform.forward;
        return new Vector3(s.x, 0, s.z);
    }

}
