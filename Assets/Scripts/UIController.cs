using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {
    public GameObject Canvas;
    SocketManager socketMgr;
    public post postObj;
    // Use this for initialization
    void Start () {
        socketMgr = SocketManager.Instance;

    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Space))
        {
            if(SocketManager.Instance.myCurrent == SocketManager.Character.Server)
            {

            }
            else if(SocketManager.Instance.myCurrent == SocketManager.Character.Client)
            {
                SocketManager.Instance.ClientSendMsg("On Click Space");
            }

        }
	}

    public void OnClickServer()
    {
        SocketManager.Instance.InitServer((o) => { print(o); });
        CloseUI();
        SocketManager.Instance.myCurrent = SocketManager.Character.Server;
    }

    public void OnClickClinet()
    {
        //SocketManager.Instance.ClientConnectServer(SocketManager.Instance.ip, SocketManager.Instance.port, (o) => { print(o); });
        //CloseUI();
        //SocketManager.Instance.myCurrent = SocketManager.Character.Client;

        postObj.Post(SocketManager.Instance.ip, "111");
    }

    void CloseUI()
    {
        Canvas.gameObject.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        //SocketManager.Instance.OnApplicationQuit();
    }
}
