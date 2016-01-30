using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AHServerInfo
{
	public string serverAddress;
	public int serverPort;
}

public class AHNetworkDiscovery : NetworkDiscovery {

	public override void OnReceivedBroadcast (string fromAddress, string data)
	{
		AHServerInfo serverInfo = new AHServerInfo();
		char[] trimChars = {':', 'f'};
		serverInfo.serverAddress = fromAddress;
		int.TryParse(data, out serverInfo.serverPort);
		Debug.Log("fromAddress: " + serverInfo.serverAddress + ", data: " + data);
		SendMessage("connectToServer", serverInfo);
	}
}
