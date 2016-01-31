using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;

public class AHClient : MonoBehaviour {

	public static AHClient singleton = null;

	public NetworkDiscovery networkDiscovery;
	public bool useNetworkDiscovery = true;
	public string serverAddress;
	public int serverPort;
	public Image statusPanel;
	public MicrophoneBehavior microphoneBehavior;
	NetworkClient client = null;
	int playerNum = -1;

	void Awake() {
		if (singleton == null)
		{
			singleton = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		if (!networkDiscovery.Initialize())
		{
			Debug.Log("NetworkDiscovery failed to initialize! Port not available.");
		}
		else
		{
			clientSetup();
			initiateConnection();
		}
	}
	
	// Update is called once per frame
	void Update () {
		updateStatusPanel();
	}

	void initiateConnection()
	{
		if (useNetworkDiscovery)
		{
			networkDiscovery.StartAsClient();
		}
		else
		{
			client.Connect(serverAddress, serverPort);
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

	public void sendVoiceToServer()
	{
		if (client.isConnected)
		{
			Byte[] bytes = File.ReadAllBytes(microphoneBehavior.filePath);

			// notify the server with file information
			AHVoiceFileInfoMessage fileInfoMsg = new AHVoiceFileInfoMessage();
			fileInfoMsg.playerNum = playerNum;
			fileInfoMsg.fileSize = bytes.Length;

			client.Send(AHMsg.VoiceFileInfoMessage, fileInfoMsg);

			// break up the file in 1024 byte chunks and send
			for (int i=0; i < bytes.Length; i += 1024)
			{
				AHVoiceFileMessage voiceMsg = new AHVoiceFileMessage();
				voiceMsg.playerNum = playerNum;
				voiceMsg.index = i;
				int size = i + 1024 > bytes.Length ? bytes.Length - i : 1024;
				voiceMsg.bytes = new Byte[size];
				Array.Copy(bytes, i, voiceMsg.bytes, 0, size);

				client.Send(AHMsg.VoiceFileMessage, voiceMsg);
				client.connection.FlushChannels();
			}

			// notify the server that file transfer has finished
			AHVoiceFileCompleteMessage fileCompleteMsg = new AHVoiceFileCompleteMessage();
			fileCompleteMsg.playerNum = playerNum;

			client.Send(AHMsg.VoiceFileCompleteMessage, fileCompleteMsg);
		}
	}

	public void sendShakeRateToServer(float shakeRate, int numShake)
	{
		if(!client.isConnected || playerNum < 0)
			return;
		AHShakeMessage shakeMsg = new AHShakeMessage();
		shakeMsg.playerNum = playerNum;
		shakeMsg.shakeRate = shakeRate;
		shakeMsg.numShake = numShake;

		client.Send(AHMsg.ShakeMessage, shakeMsg);
	}

	void clientSetup()
	{
		client = new NetworkClient();
		client.RegisterHandler(MsgType.Connect, OnConnect);
		client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
		client.RegisterHandler(MsgType.Error, OnError);
		client.RegisterHandler(AHMsg.ConnectMessage, OnConnectMsg);
	}

	void updateStatusPanel()
	{
		switch (playerNum)
		{
		case 0:
			statusPanel.color = Color.red;
			break;
		case 1:
			statusPanel.color = Color.blue;
			break;
		default:
			statusPanel.color = Color.black;
			break;
		}
	}

	// ---------------- msg handlers ------------------------
	void OnConnect(NetworkMessage netMsg)
	{
		Debug.Log("Connected to server.");
	}

	void OnDisconnect(NetworkMessage netMsg)
	{
		Debug.Log("Disconnected from server.");
		playerNum = -1;
		initiateConnection();
	}

	void OnError(NetworkMessage netMsg) {
		ErrorMessage errorMsg = netMsg.ReadMessage<ErrorMessage>();
		Debug.Log("Error connecting with code " + errorMsg.errorCode);
	}

	void OnConnectMsg(NetworkMessage netMsg)
	{
		AHConnectMessage msg = netMsg.ReadMessage<AHConnectMessage>();
		playerNum = msg.playerNum;
		updateStatusPanel();
	}

}
