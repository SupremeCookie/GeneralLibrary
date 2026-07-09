using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ISteamCallbacks
{
	bool SubscribeCallback_OnOverlayMode(string key, System.Action<OverlayMode> callback);
	void UnSubscribeCallback_OnOverlayMode(string key);
}