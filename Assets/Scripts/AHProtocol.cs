using UnityEngine.Networking;

public class AHMsg
{
	public const short SimpleMessage = 100;
}

public class AHSimpleMessage : MessageBase
{
	public string msg;
}