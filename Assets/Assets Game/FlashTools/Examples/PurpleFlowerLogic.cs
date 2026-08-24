using System.Collections;
using FTRuntime;
using FTRuntime.Yields;
using UnityEngine;

namespace FlashTools.Examples
{
	[RequireComponent(typeof(SwfClipController))]
	public class PurpleFlowerLogic : MonoBehaviour
	{
		private static string[] _idleSequences = new string[4] { "talk", "idle0", "idle1", "idle2" };

		private static string _fadeInSequence = "fadeIn";

		private static string _fadeOutSequence = "fadeOut";

		private void Start()
		{
			SwfClipController component = GetComponent<SwfClipController>();
			StartCoroutine(StartCoro(component));
		}

		private IEnumerator StartCoro(SwfClipController ctrl)
		{
			while (true)
			{
				yield return ctrl.PlayAndWaitStopOrRewind(_fadeInSequence);
				for (int i = 0; i < 3; i++)
				{
					string idle_seq = GetRandomIdleSequence(ctrl);
					yield return ctrl.PlayAndWaitStopOrRewind(idle_seq);
				}
				yield return ctrl.PlayAndWaitStopOrRewind(_fadeOutSequence);
				yield return new WaitForSeconds(2f);
			}
		}

		private string GetRandomIdleSequence(SwfClipController ctrl)
		{
			string sequence = ctrl.clip.sequence;
			string text;
			do
			{
				int num = Random.Range(0, _idleSequences.Length);
				text = _idleSequences[num];
			}
			while (!(text != sequence));
			return text;
		}
	}
}
