using System;
using System.Collections.Generic;
using FTRuntime.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace FTRuntime
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(SortingGroup))]
	public class SwfClip : MonoBehaviour
	{
		private MeshFilter _meshFilter;

		private MeshRenderer _meshRenderer;

		private SortingGroup _sortingGroup;

		private bool _dirtyMesh = true;

		private SwfClipAsset.Sequence _curSequence;

		private MaterialPropertyBlock _curPropBlock;

		[Header("Sorting")]
		[SerializeField]
		[SwfSortingLayer]
		private string _sortingLayer = string.Empty;

		[SerializeField]
		private int _sortingOrder;

		[Header("Animation")]
		[SerializeField]
		private Color _tint = Color.white;

		[SerializeField]
		private SwfClipAsset _clip;

		[SerializeField]
		[HideInInspector]
		private string _sequence = string.Empty;

		[SerializeField]
		[HideInInspector]
		private int _currentFrame;

		public string sortingLayer
		{
			get
			{
				return _sortingLayer;
			}
			set
			{
				_sortingLayer = value;
				ChangeSortingProperties();
			}
		}

		public int sortingOrder
		{
			get
			{
				return _sortingOrder;
			}
			set
			{
				_sortingOrder = value;
				ChangeSortingProperties();
			}
		}

		public Color tint
		{
			get
			{
				return _tint;
			}
			set
			{
				_tint = value;
				ChangeTint();
			}
		}

		public SwfClipAsset clip
		{
			get
			{
				return _clip;
			}
			set
			{
				_clip = value;
				_sequence = string.Empty;
				_currentFrame = 0;
				ChangeClip();
				EmitChangeEvents(true, true, true);
			}
		}

		public string sequence
		{
			get
			{
				return _sequence;
			}
			set
			{
				_sequence = value;
				_currentFrame = 0;
				ChangeSequence();
				EmitChangeEvents(false, true, true);
			}
		}

		public int currentFrame
		{
			get
			{
				return _currentFrame;
			}
			set
			{
				_currentFrame = value;
				ChangeCurrentFrame();
				EmitChangeEvents(false, false, true);
			}
		}

		public int frameCount
		{
			get
			{
				return (_curSequence != null && _curSequence.Frames != null) ? _curSequence.Frames.Count : 0;
			}
		}

		public float frameRate
		{
			get
			{
				return (!clip) ? 0f : clip.FrameRate;
			}
		}

		public int currentLabelCount
		{
			get
			{
				SwfClipAsset.Frame currentBakedFrame = GetCurrentBakedFrame();
				string[] array = ((currentBakedFrame == null) ? null : currentBakedFrame.Labels);
				return (array != null) ? array.Length : 0;
			}
		}

		public Bounds currentLocalBounds
		{
			get
			{
				SwfClipAsset.Frame currentBakedFrame = GetCurrentBakedFrame();
				return (currentBakedFrame == null) ? default(Bounds) : currentBakedFrame.CachedMesh.bounds;
			}
		}

		public Bounds currentWorldBounds
		{
			get
			{
				Internal_UpdateMesh();
				return (!_meshRenderer) ? default(Bounds) : _meshRenderer.bounds;
			}
		}

		public event Action<SwfClip> OnChangeClipEvent;

		public event Action<SwfClip> OnChangeSequenceEvent;

		public event Action<SwfClip> OnChangeCurrentFrameEvent;

		public void ToBeginFrame()
		{
			currentFrame = 0;
		}

		public void ToEndFrame()
		{
			currentFrame = ((frameCount > 0) ? (frameCount - 1) : 0);
		}

		public bool ToPrevFrame()
		{
			if (currentFrame > 0)
			{
				currentFrame--;
				return true;
			}
			return false;
		}

		public bool ToNextFrame()
		{
			if (currentFrame < frameCount - 1)
			{
				currentFrame++;
				return true;
			}
			return false;
		}

		public string GetCurrentFrameLabel(int index)
		{
			SwfClipAsset.Frame currentBakedFrame = GetCurrentBakedFrame();
			string[] array = ((currentBakedFrame == null) ? null : currentBakedFrame.Labels);
			return (array == null || index < 0 || index >= array.Length) ? string.Empty : array[index];
		}

        /// <summary>
        /// Bản 1 material
        /// </summary>
        //internal void Internal_UpdateMesh()
        //{
        //    if ((bool)_meshFilter && (bool)_meshRenderer && _dirtyMesh)
        //    {
        //        SwfClipAsset.Frame currentBakedFrame = GetCurrentBakedFrame();
        //        if (currentBakedFrame != null)
        //        {
        //            _meshFilter.sharedMesh = currentBakedFrame.CachedMesh;

        //            // KIỂM TRA: Nếu Frame gốc không có Material thì tự tạo Material mặc định
        //            if (currentBakedFrame.Materials != null && currentBakedFrame.Materials.Length > 0 && currentBakedFrame.Materials[0] != null)
        //            {
        //                _meshRenderer.sharedMaterials = currentBakedFrame.Materials;
        //            }
        //            else
        //            {
        //                // Nếu bị missing, dùng Material Sprites-Default để không bị xám/tím/trong suốt
        //                if (_meshRenderer.sharedMaterial == null)
        //                {
        //                    _meshRenderer.sharedMaterial = Canvas.GetDefaultCanvasMaterial();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            _meshFilter.sharedMesh = null;
        //            _meshRenderer.sharedMaterials = new Material[0];
        //        }
        //        _dirtyMesh = false;
        //    }
        //}


        /// <summary>
        /// Bản 2 material
        /// </summary>

        [Header("Material Override Control")]
        [Tooltip("Tích chọn = HIỂN THỊ Material. Bỏ tích = ẨN/XÓA Material đó.")]
        public List<bool> materialVisibilities = new List<bool>();
        // Hàm kiểm tra xem Element tại index có được ĐÃ TÍCH (Hiển thị) hay không
        private bool IsElementVisible(int index)
        {
            // Nếu list chưa được cài đặt hoặc index vượt quá độ dài list, mặc định cho hiển thị bình thường
            if (materialVisibilities == null || index >= materialVisibilities.Count)
            {
                return true;
            }

            // Trả về giá trị true/false trực tiếp từ ô bạn đã tích/bỏ tích trên Inspector
            return materialVisibilities[index];
        }

        internal void Internal_UpdateMesh()
        {
            if ((bool)_meshFilter && (bool)_meshRenderer && _dirtyMesh)
            {
                SwfClipAsset.Frame currentBakedFrame = GetCurrentBakedFrame();
                if (currentBakedFrame != null && currentBakedFrame.CachedMesh != null)
                {
                    _meshFilter.sharedMesh = currentBakedFrame.CachedMesh;

                    int subMeshCount = currentBakedFrame.CachedMesh.subMeshCount;
                    Material[] finalMats = new Material[subMeshCount];
                    Material[] frameMats = currentBakedFrame.Materials;

                    for (int i = 0; i < subMeshCount; i++)
                    {
                        if (!IsElementVisible(i))
                        {
                            finalMats[i] = null;
                            continue;
                        }

                        if (frameMats != null && i < frameMats.Length && frameMats[i] != null)
                        {
                            finalMats[i] = frameMats[i];
                        }
                        else
                        {
                            finalMats[i] = Canvas.GetDefaultCanvasMaterial();
                        }
                    }

                    _meshRenderer.sharedMaterials = finalMats;
                }
                else
                {
                    _meshFilter.sharedMesh = null;
                    _meshRenderer.sharedMaterials = new Material[0];
                }

                _dirtyMesh = false;

                // --- THÊM DÒNG NÀY ---
                // Đảm bảo Texture luôn được gán lại vào PropertyBlock ngay khi Mesh & Material mới được thiết lập
                UpdatePropBlock();
            }
        }

        private void UpdatePropBlock()
        {
            if ((bool)_meshRenderer)
            {
                if (_curPropBlock == null)
                {
                    _curPropBlock = new MaterialPropertyBlock();
                }

                _meshRenderer.GetPropertyBlock(_curPropBlock);
                _curPropBlock.SetColor(SwfUtils.TintShaderProp, tint);

                Sprite sprite = ((!clip) ? null : clip.Sprite);
                Texture2D texture2D = ((!sprite || !sprite.texture) ? Texture2D.whiteTexture : sprite.texture);
                Texture2D texture2D2 = ((!sprite) ? null : sprite.associatedAlphaSplitTexture);

                _curPropBlock.SetTexture(SwfUtils.MainTexShaderProp, (!texture2D) ? Texture2D.whiteTexture : texture2D);

                if ((bool)texture2D2)
                {
                    _curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, texture2D2);
                    _curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 1f);
                }
                else
                {
                    _curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, Texture2D.whiteTexture);
                    _curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 0f);
                }

                // Áp dụng PropertyBlock cho các Element được TÍCH (true)
                int matCount = _meshRenderer.sharedMaterials.Length;
                for (int i = 0; i < matCount; i++)
                {
                    if (IsElementVisible(i))
                    {
                        _meshRenderer.SetPropertyBlock(_curPropBlock, i);
                    }
                    else
                    {
                        // Xóa PropertyBlock nếu ô bị BỎ TÍCH (false)
                        _meshRenderer.SetPropertyBlock(null, i);
                    }
                }
            }
        }

		// ------- kết thúc test nhiều material ------------

        public void Internal_UpdateAllProperties()
		{
			ClearCache(false);
			ChangeTint();
			ChangeClip();
			ChangeSequence();
			ChangeCurrentFrame();
			ChangeSortingProperties();
		}

		private void ClearCache(bool allow_to_create_components)
		{
			_meshFilter = SwfUtils.GetComponent<MeshFilter>(base.gameObject, allow_to_create_components);
			_meshRenderer = SwfUtils.GetComponent<MeshRenderer>(base.gameObject, allow_to_create_components);
			_sortingGroup = SwfUtils.GetComponent<SortingGroup>(base.gameObject, allow_to_create_components);
			_dirtyMesh = true;
			_curSequence = null;
			_curPropBlock = null;
		}

		private void ChangeTint()
		{
			UpdatePropBlock();
		}

		private void ChangeClip()
		{
			if ((bool)_meshRenderer)
			{
				_meshRenderer.enabled = clip;
			}
			ChangeSequence();
			UpdatePropBlock();
		}

		private void ChangeSequence()
		{
			_curSequence = null;
			if ((bool)clip && clip.Sequences != null)
			{
				if (!string.IsNullOrEmpty(this.sequence))
				{
					int i = 0;
					for (int count = clip.Sequences.Count; i < count; i++)
					{
						SwfClipAsset.Sequence sequence = clip.Sequences[i];
						if (sequence != null && sequence.Name == this.sequence)
						{
							_curSequence = sequence;
							break;
						}
					}
					if (_curSequence == null)
					{
						Debug.LogWarningFormat(this, "<b>[FlashTools]</b> Sequence '{0}' not found", this.sequence);
					}
				}
				if (_curSequence == null)
				{
					int j = 0;
					for (int count2 = clip.Sequences.Count; j < count2; j++)
					{
						SwfClipAsset.Sequence sequence2 = clip.Sequences[j];
						if (sequence2 != null)
						{
							_sequence = sequence2.Name;
							_curSequence = sequence2;
							break;
						}
					}
				}
			}
			ChangeCurrentFrame();

            // --- THÊM DÒNG NÀY ---
            UpdatePropBlock();
        }

		private void ChangeCurrentFrame()
		{
			_dirtyMesh = true;
			_currentFrame = ((frameCount > 0) ? Mathf.Clamp(currentFrame, 0, frameCount - 1) : 0);
		}

		private void ChangeSortingProperties()
		{
			if ((bool)_meshRenderer)
			{
				_meshRenderer.sortingOrder = sortingOrder;
				_meshRenderer.sortingLayerName = sortingLayer;
			}
			if ((bool)_sortingGroup)
			{
				_sortingGroup.sortingOrder = sortingOrder;
				_sortingGroup.sortingLayerName = sortingLayer;
			}
		}
        /// <summary>
        /// Bản 1 material
        /// </summary>
        //private void UpdatePropBlock()
        //{
        //    if ((bool)_meshRenderer)
        //    {
        //        // Tự động gán Material mặc định (Sprites-Default) nếu MeshRenderer chưa có Material
        //        if (_meshRenderer.sharedMaterial == null)
        //        {
        //            _meshRenderer.sharedMaterial = Canvas.GetDefaultCanvasMaterial();
        //        }

        //        if (_curPropBlock == null)
        //        {
        //            _curPropBlock = new MaterialPropertyBlock();
        //        }
        //        _meshRenderer.GetPropertyBlock(_curPropBlock);
        //        _curPropBlock.SetColor(SwfUtils.TintShaderProp, tint);
        //        Sprite sprite = ((!clip) ? null : clip.Sprite);
        //        Texture2D texture2D = ((!sprite || !sprite.texture) ? Texture2D.whiteTexture : sprite.texture);
        //        Texture2D texture2D2 = ((!sprite) ? null : sprite.associatedAlphaSplitTexture);
        //        _curPropBlock.SetTexture(SwfUtils.MainTexShaderProp, (!texture2D) ? Texture2D.whiteTexture : texture2D);
        //        if ((bool)texture2D2)
        //        {
        //            _curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, texture2D2);
        //            _curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 1f);
        //        }
        //        else
        //        {
        //            _curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, Texture2D.whiteTexture);
        //            _curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 0f);
        //        }
        //        _meshRenderer.SetPropertyBlock(_curPropBlock);
        //    }
        //}

        private void EmitChangeEvents(bool clip, bool sequence, bool current_frame)
		{
			if (clip && this.OnChangeClipEvent != null)
			{
				this.OnChangeClipEvent(this);
			}
			if (sequence && this.OnChangeSequenceEvent != null)
			{
				this.OnChangeSequenceEvent(this);
			}
			if (current_frame && this.OnChangeCurrentFrameEvent != null)
			{
				this.OnChangeCurrentFrameEvent(this);
			}
		}

		private SwfClipAsset.Frame GetCurrentBakedFrame()
		{
			List<SwfClipAsset.Frame> list = ((_curSequence == null) ? null : _curSequence.Frames);
			return (list == null || currentFrame < 0 || currentFrame >= list.Count) ? null : list[currentFrame];
		}

		private void Start()
		{
			ClearCache(true);
			Internal_UpdateAllProperties();
			EmitChangeEvents(true, true, true);
		}

		private void OnEnable()
		{
			SwfManager instance = SwfManager.GetInstance(true);
			if ((bool)instance)
			{
				instance.AddClip(this);
			}
		}

		private void OnDisable()
		{
			SwfManager instance = SwfManager.GetInstance(false);
			if ((bool)instance)
			{
				instance.RemoveClip(this);
			}
		}

		private void Reset()
		{
			Internal_UpdateAllProperties();
		}

		private void OnValidate()
		{
			Internal_UpdateAllProperties();
		}
	}
}
