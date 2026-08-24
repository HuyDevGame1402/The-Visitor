using System;
using FTRuntime.Internal;
using UnityEngine;

namespace FTRuntime
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(SwfClip))]
	public class SwfClipController : MonoBehaviour
	{
		public enum PlayModes
		{
			Forward = 0,
			Backward = 1
		}

		public enum LoopModes
		{
			Once = 0,
			Loop = 1
		}

		private SwfClip _clip;

		private bool _isPlaying;

		private float _tickTimer;

		[SerializeField]
		private bool _autoPlay = true;

		[SerializeField]
		private bool _useUnscaledDt;

		[SerializeField]
		[SwfFloatRange(0f, float.MaxValue)]
		private float _rateScale = 1f;

		[SerializeField]
		private string _groupName = string.Empty;

		[SerializeField]
		private PlayModes _playMode;

		[SerializeField]
		private LoopModes _loopMode = LoopModes.Loop;

		public bool autoPlay
		{
			get
			{
				return _autoPlay;
			}
			set
			{
				_autoPlay = value;
			}
		}

		public bool useUnscaledDt
		{
			get
			{
				return _useUnscaledDt;
			}
			set
			{
				_useUnscaledDt = value;
			}
		}

		public float rateScale
		{
			get
			{
				return _rateScale;
			}
			set
			{
				_rateScale = Mathf.Clamp(value, 0f, float.MaxValue);
			}
		}

		public string groupName
		{
			get
			{
				return _groupName;
			}
			set
			{
				_groupName = value;
			}
		}

		public PlayModes playMode
		{
			get
			{
				return _playMode;
			}
			set
			{
				_playMode = value;
			}
		}

		public LoopModes loopMode
		{
			get
			{
				return _loopMode;
			}
			set
			{
				_loopMode = value;
			}
		}

		public SwfClip clip
		{
			get
			{
				return _clip;
			}
		}

		public bool isPlaying
		{
			get
			{
				return _isPlaying;
			}
		}

		public bool isStopped
		{
			get
			{
				return !_isPlaying;
			}
		}

		public event Action<SwfClipController> OnStopPlayingEvent;

		public event Action<SwfClipController> OnPlayStoppedEvent;

		public event Action<SwfClipController> OnRewindPlayingEvent;

		public void GotoAndStop(int frame)
		{
			if ((bool)clip)
			{
				clip.currentFrame = frame;
			}
			Stop(false);
		}

		public void GotoAndStop(string sequence, int frame)
		{
			if ((bool)clip)
			{
				clip.sequence = sequence;
			}
			GotoAndStop(frame);
		}

		public void GotoAndPlay(int frame)
		{
			if ((bool)clip)
			{
				clip.currentFrame = frame;
			}
			Play(false);
		}

		public void GotoAndPlay(string sequence, int frame)
		{
			if ((bool)clip)
			{
				clip.sequence = sequence;
			}
			GotoAndPlay(frame);
		}

		public void Stop(bool rewind)
		{
			bool flag = isPlaying;
			if (flag)
			{
				_isPlaying = false;
				_tickTimer = 0f;
			}
			if (rewind)
			{
				Rewind();
			}
			if (flag && this.OnStopPlayingEvent != null)
			{
				this.OnStopPlayingEvent(this);
			}
		}

		public void Stop(string sequence)
		{
			if ((bool)clip)
			{
				clip.sequence = sequence;
			}
			Stop(true);
		}

		public void Play(bool rewind)
		{
			bool flag = isStopped;
			if (flag)
			{
				_isPlaying = true;
				_tickTimer = 0f;
			}
			if (rewind)
			{
				Rewind();
			}
			if (flag && this.OnPlayStoppedEvent != null)
			{
				this.OnPlayStoppedEvent(this);
			}
		}

		public void Play(string sequence)
		{
			if ((bool)clip)
			{
				clip.sequence = sequence;
			}
			Play(true);
		}

		public void Rewind()
		{
			switch (playMode)
			{
			case PlayModes.Forward:
				if ((bool)clip)
				{
					clip.ToBeginFrame();
				}
				break;
			case PlayModes.Backward:
				if ((bool)clip)
				{
					clip.ToEndFrame();
				}
				break;
			default:
				throw new UnityException(string.Format("SwfClipController. Incorrect play mode: {0}", playMode));
			}
			if (isPlaying && this.OnRewindPlayingEvent != null)
			{
				this.OnRewindPlayingEvent(this);
			}
		}

		internal void Internal_Update(float scaled_dt, float unscaled_dt)
		{
			while (isPlaying && (bool)clip)
			{
				float num = ((!useUnscaledDt) ? scaled_dt : unscaled_dt);
				float num2 = clip.frameRate * rateScale;
				if (num > 0f && num2 > 0f)
				{
					_tickTimer += num2 * num;
					if (_tickTimer >= 1f)
					{
						float num3 = (_tickTimer - 1f) / num2;
						_tickTimer = 0f;
						TimerTick();
						scaled_dt = num3 * (scaled_dt / num);
						unscaled_dt = num3 * (unscaled_dt / num);
						continue;
					}
					break;
				}
				break;
			}
		}

		private void TimerTick()
		{
			if (!NextClipFrame())
			{
				switch (loopMode)
				{
				case LoopModes.Once:
					Stop(false);
					break;
				case LoopModes.Loop:
					Rewind();
					break;
				default:
					throw new UnityException(string.Format("SwfClipController. Incorrect loop mode: {0}", loopMode));
				}
			}
		}

		private bool NextClipFrame()
		{
			switch (playMode)
			{
			case PlayModes.Forward:
				return (bool)clip && clip.ToNextFrame();
			case PlayModes.Backward:
				return (bool)clip && clip.ToPrevFrame();
			default:
				throw new UnityException(string.Format("SwfClipController. Incorrect play mode: {0}", playMode));
			}
		}

		private void Awake()
		{
			_clip = GetComponent<SwfClip>();
		}

		private void OnEnable()
		{
			SwfManager instance = SwfManager.GetInstance(true);
			if ((bool)instance)
			{
				instance.AddController(this);
			}
			if (autoPlay && Application.isPlaying)
			{
				Play(false);
			}
		}

		private void OnDisable()
		{
			Stop(false);
			SwfManager instance = SwfManager.GetInstance(false);
			if ((bool)instance)
			{
				instance.RemoveController(this);
			}
		}
	}
}
