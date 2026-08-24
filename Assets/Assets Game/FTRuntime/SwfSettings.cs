using UnityEngine;

namespace FTRuntime
{
	public class SwfSettings : ScriptableObject
	{
		public SwfSettingsData Settings;

		private void Reset()
		{
			Settings = SwfSettingsData.identity;
		}
	}
}
