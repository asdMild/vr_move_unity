using UnityEngine;
using System.Collections;

public class ui : MonoBehaviour
{
    public GUIText lab;
    public GameObject obj;

    public GameObject l_;
    public GameObject r_;
    public GameObject h_;
    public GameObject self_;
    // Use this for initialization
    void Start()
    {
       // UnityEngine.VR.VRSettings.showDeviceView = false;
        lianjie();
    }


    void OnConnectedToMaster()
    {
        Debug.Log("start");
        PhotonNetwork.JoinOrCreateRoom("xuxu",null,null);
    }

    void OnJoinedRoom()
    {
        Debug.Log("inroom");
        create_hands();

        Debug.Log(PhotonNetwork.GetPing());

    }

    // Update is called once per frame
    void Update()
    {
        var _roomlist = PhotonNetwork.GetRoomList();
       // Debug.Log(_roomlist.Length);
        // lab.text = PhotonNetwork.connectionStateDetailed.ToString();
    }

    public void lianjie()
    {
        PhotonNetwork.ConnectUsingSettings("1");
    }

    public void create()
    {
        PhotonNetwork.CreateRoom("xuxu0");
        Debug.Log("creat room");
    }

    public void jiaru()
    {
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("join room");
    }

    public void create_hands()
    {
       /* var l = PhotonNetwork.Instantiate(obj.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
        var r = PhotonNetwork.Instantiate(obj.name, new Vector3(0, 0, 0), Quaternion.identity, 0);

        l.GetComponent<cube_walk>().parent = l_;
        r.GetComponent<cube_walk>().parent = r_;
        */
        var h= PhotonNetwork.Instantiate(obj.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
        h.GetComponent<cube_walk>().parent = h_;
        h.GetComponent<cube_walk>()._r = r_;
        h.GetComponent<cube_walk>()._l = l_;
        h.GetComponent<cube_walk>()._h = h_;
    }
}
