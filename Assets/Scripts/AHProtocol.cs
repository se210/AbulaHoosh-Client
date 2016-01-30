using UnityEngine.Networking;
using System;

public class AHMsg
{
	public const short SimpleMessage = 100;
	public const short ConnectMessage = 101;
	public const short VoiceFileMessage = 102;
	public const short VoiceFileInfoMessage = 103;
	public const short VoiceFileCompleteMessage = 104;
	public const short ShakeMessage = 105;
}

public class AHSimpleMessage : MessageBase
{
	public int playerNum;
	public string msg;
}

public class AHConnectMessage : MessageBase
{
	public int playerNum;
}

public class AHVoiceFileInfoMessage : MessageBase
{
	public int playerNum;
	public int fileSize;
}

public class AHVoiceFileMessage : MessageBase
{
	public int playerNum;
	public int index;
	public Byte[] bytes;
}

public class AHVoiceFileCompleteMessage : MessageBase
{
	public int playerNum;
}

public class AHShakeMessage : MessageBase
{
	public int playerNum;
	public float shakeRate;
	public int numShake;
}