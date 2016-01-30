using UnityEngine.Networking;

public class AHMsg
{
	public const short SimpleMessage = 100;
	public const short ConnectMessage = 101;
}

public class AHSimpleMessage : MessageBase
{
	public string msg;
}

public class AHConnectMessage : MessageBase
{
	public int playerNum;
}