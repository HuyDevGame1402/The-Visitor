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

        [Header("Material Override Control (Only for Fallback/Default Material)")]
        [Tooltip("Chỉ áp dụng khi Frame KHÔNG CÓ Material sẵn. Tích chọn = Dùng Default Material. Bỏ tích = Ẩn submesh.")]
        public List<bool> materialVisibilities = new List<bool>();

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

        private bool IsDefaultMaterialVisible(int index)
        {
            if (materialVisibilities == null || index >= materialVisibilities.Count)
            {
                return true;
            }
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
                        if (frameMats != null && i < frameMats.Length && frameMats[i] != null)
                        {
                            finalMats[i] = frameMats[i];
                        }
                        else
                        {
                            if (IsDefaultMaterialVisible(i))
                            {
                                finalMats[i] = Canvas.GetDefaultCanvasMaterial();
                            }
                            else
                            {
                                finalMats[i] = null;
                            }
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
                Texture2D mainTexture = null;

                if (sprite && sprite.texture)
                {
                    mainTexture = sprite.texture;
                }

                // Nếu Clip không chứa Sprite Texture, dùng Texture từ Material gốc
                if (mainTexture == null && _meshRenderer.sharedMaterial != null)
                {
                    mainTexture = _meshRenderer.sharedMaterial.mainTexture as Texture2D;
                }

                if (mainTexture == null)
                {
                    mainTexture = Texture2D.whiteTexture;
                }

                // Gán Texture vào các Property Name phổ biến của Shader để tránh bị lỗi Shader Custom
                _curPropBlock.SetTexture(SwfUtils.MainTexShaderProp, mainTexture);
                _curPropBlock.SetTexture("_MainTex", mainTexture);
                _curPropBlock.SetTexture("_BaseMap", mainTexture);

                Texture2D alphaTexture = ((!sprite) ? null : sprite.associatedAlphaSplitTexture);

                if ((bool)alphaTexture)
                {
                    _curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, alphaTexture);
                    _curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 1f);
                }
                else
                {
                    _curPropBlock.SetTexture(SwfUtils.AlphaTexShaderProp, Texture2D.whiteTexture);
                    _curPropBlock.SetFloat(SwfUtils.ExternalAlphaShaderProp, 0f);
                }

                int matCount = _meshRenderer.sharedMaterials.Length;
                for (int i = 0; i < matCount; i++)
                {
                    if (_meshRenderer.sharedMaterials[i] != null)
                    {
                        _meshRenderer.SetPropertyBlock(_curPropBlock, i);
                    }
                    else
                    {
                        _meshRenderer.SetPropertyBlock(null, i);
                    }
                }
            }
        }

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
                    int count = clip.Sequences.Count;
                    for (int i = 0; i < count; i++)
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
                    int count2 = clip.Sequences.Count;
                    for (int j = 0; j < count2; j++)
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