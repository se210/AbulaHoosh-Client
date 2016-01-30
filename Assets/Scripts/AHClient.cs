using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;
using System.Collections;

public class AHClient : MonoBehaviour {

	public NetworkDiscovery networkDiscovery;
	public bool useNetworkDiscovery = true;
	public string serverAddress;
	public int serverPort;
	NetworkClient client = null;

	// Use this for initialization
	void Start () {
		if (!networkDiscovery.Initialize())
		{
			Debug.Log("NetworkDiscovery failed to initialize! Port not available.");
		}
		else
		{
			clientSetup();
			if (useNetworkDiscovery)
			{
				networkDiscovery.StartAsClient();
			}
			else
			{
				client.Connect(serverAddress, serverPort);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (client.isConnected)
		{
			Debug.Log("Sending network message to server.");
			AHSimpleMessage simpleMsg = new AHSimpleMessage();
			simpleMsg.msg = "hello world";
			client.Send(AHMsg.SimpleMessage, simpleMsg);
		}
	}

	public void connectToServer(AHServerInfo serverInfo)
	{
		if (!client.isConnected)
		{
			networkDiscovery.StopBroadcast();
			client.Connect(serverInfo.serverAddress, serverInfo.serverPort);
		}
	}

	void clientSetup()
	{
		client = new NetworkClient();
		client.RegisterHandler(MsgType.Connect, OnConnect);
		client.RegisterHandler(MsgType.Error, OnError);
	}

	// ---------------- msg handlers ------------------------
	void OnConnect(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server.");
	}

	void OnError(NetworkMessage netMsg) {
		ErrorMessage errorMsg = netMsg.ReadMessage<ErrorMessage>();
		Debug.Log("Error connecting with code " + errorMsg.errorCode);
	}

}
