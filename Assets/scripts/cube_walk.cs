using UnityEngine;
using System.Collections;


public class cube_walk : MonoBehaviour
{
    public GameObject _l;
    public GameObject _r;
    public GameObject _h;
    public GameObject parent;
    public GameObject bullet;
    public Transform fire_point;
    SteamVR_TrackedObject _lo;
    SteamVR_TrackedObject _ro;
    SteamVR_TrackedObject _ho;

    SteamVR_Controller.Device device_l;
    SteamVR_Controller.Device device_r;
    PhotonView pv = null;

    Vector3 out_po;
    Quaternion out_rota;
    // Use this for initialization
    void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        _lo = _l.GetComponent<SteamVR_TrackedObject>();
        _ro = _r.GetComponent<SteamVR_TrackedObject>();
        _ho = _h.GetComponent<SteamVR_TrackedObject>();

        device_l= SteamVR_Controller.Input((int)_lo.index);
        device_r= SteamVR_Controller.Input((int)_ro.index);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            out_po = (Vector3)stream.ReceiveNext();
            out_rota = (Quaternion)stream.ReceiveNext();
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (pv.isMine)
        {
            transform.position = parent.transform.position;
            transform.rotation = parent.transform.rotation;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, out_po, Time.deltaTime * 3.0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, out_rota, Time.deltaTime * 3.0f);
        }


        if (pv.isMine && (device_l.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) || device_r.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)))
        {
            Debug.Log("fire");
            fire();
            pv.RPC("fire", PhotonTargets.Others, null);
        }

    }

    [PunRPC]
    void fire()
    {
        Instantiate(bullet, fire_point.position, transform.rotation);
    }
}
