using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public enum MinimapMode
{
    Mini, Fullscreen
}
public class MinimapController : MonoBehaviour
{
    public static MinimapController Instance;

    [FormerlySerializedAs("worldSize")] [SerializeField]
    private Vector2 _worldSize;

    [FormerlySerializedAs("fullScreenDimensions")] [SerializeField]
    private Vector2 _fullScreenDimensions = new Vector2(1000, 1000);

    [FormerlySerializedAs("zoomSpeed")] [SerializeField]
    private float _zoomSpeed = 0.1f;

    [FormerlySerializedAs("maxZoom")] [SerializeField]
    private float _maxZoom = 10f;

    [FormerlySerializedAs("minZoom")] [SerializeField]
    private float _minZoom = 1f;

    [FormerlySerializedAs("scrollViewRectTransform")] [SerializeField]
    private RectTransform _scrollViewRectTransform;

    [FormerlySerializedAs("contentRectTransform")] [SerializeField]
    private RectTransform _contentRectTransform;

    [FormerlySerializedAs("minimapIconPrefab")] [SerializeField]
    private MinimapIcon _minimapIconPrefab;

    [FormerlySerializedAs("Offset")] [SerializeField]
    private Vector2 _offset;

    private Matrix4x4 _transformationMatrix;

    private MinimapMode _currentMiniMapMode = MinimapMode.Mini;
    private MinimapIcon _followIcon;
    private Vector2 _scrollViewDefaultSize;
    private Vector2 _scrollViewDefaultPosition;
    private Dictionary<MinimapWorldObject, MinimapIcon> _miniMapWorldObjectsLookup = new Dictionary<MinimapWorldObject, MinimapIcon>();
    private void Awake()
    {
        Instance = this;
        _scrollViewDefaultSize = _scrollViewRectTransform.sizeDelta;
        _scrollViewDefaultPosition = _scrollViewRectTransform.anchoredPosition;
    }

    private void Start()
    {
        CalculateTransformationMatrix();
        ZoomMap(_minZoom);
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.OpenMapInput.WasPressedThisFrame())
        {
            SetMinimapMode(_currentMiniMapMode == MinimapMode.Mini ? MinimapMode.Fullscreen : MinimapMode.Mini);
        }
        UpdateMiniMapIcons();
        CenterMapOnIcon();
    }

    public void RegisterMinimapWorldObject(MinimapWorldObject miniMapWorldObject, bool followObject = false)
    {
        var minimapIcon = Instantiate(_minimapIconPrefab);
        minimapIcon.transform.SetParent(_contentRectTransform);
        minimapIcon.transform.SetParent(_contentRectTransform);
        minimapIcon.Image.sprite = miniMapWorldObject.minimapIcon;
        _miniMapWorldObjectsLookup[miniMapWorldObject] = minimapIcon;

        if (followObject)
            _followIcon = minimapIcon;
    }

    public void RemoveMinimapWorldObject(MinimapWorldObject minimapWorldObject)
    {
        if (_miniMapWorldObjectsLookup.TryGetValue(minimapWorldObject, out MinimapIcon icon))
        {
            _miniMapWorldObjectsLookup.Remove(minimapWorldObject);
            if (icon != null)
            {
                Destroy(icon.gameObject);
            }
        }
    }


    private Vector2 _halfVector2 = new Vector2(0.5f, 0.5f);
    public void SetMinimapMode(MinimapMode mode)
    {
        const float defaultScaleWhenFullScreen = 1.3f; // 1.3f looks good here but it could be anything

        if (mode == _currentMiniMapMode)
            return;

        switch (mode)
        {
            case MinimapMode.Mini:
                _scrollViewRectTransform.sizeDelta = _scrollViewDefaultSize;
                _scrollViewRectTransform.anchorMin = Vector2.one;
                _scrollViewRectTransform.anchorMax = Vector2.one;
                _scrollViewRectTransform.pivot = Vector2.one;
                _scrollViewRectTransform.anchoredPosition = _scrollViewDefaultPosition;
                _currentMiniMapMode = MinimapMode.Mini;
                break;
            case MinimapMode.Fullscreen:
                _scrollViewRectTransform.sizeDelta = _fullScreenDimensions;
                _scrollViewRectTransform.anchorMin = _halfVector2;
                _scrollViewRectTransform.anchorMax = _halfVector2;
                _scrollViewRectTransform.pivot = _halfVector2;
                _scrollViewRectTransform.anchoredPosition = Vector2.zero;
                _currentMiniMapMode = MinimapMode.Fullscreen;
                _contentRectTransform.transform.localScale = Vector3.one * defaultScaleWhenFullScreen;
                break;
        }
    }

    private void ZoomMap(float zoom)
    {
        if (zoom == 0)
            return;

        float currentMapScale = _contentRectTransform.localScale.x;
        // we need to scale the zoom speed by the current map scale to keep the zooming linear
        float zoomAmount = (zoom > 0 ? _zoomSpeed : -_zoomSpeed) * currentMapScale;
        float newScale = currentMapScale + zoomAmount;
        float clampedScale = Mathf.Clamp(newScale, _minZoom, _maxZoom);
        _contentRectTransform.localScale = Vector3.one * clampedScale;
    }

    private void CenterMapOnIcon()
    {
        if (_followIcon != null)
        {
            float mapScale = _contentRectTransform.transform.localScale.x;
            // we simply move the map in the opposite direction the player moved, scaled by the mapscale
            _contentRectTransform.anchoredPosition = (-_followIcon.RectTransform.anchoredPosition * mapScale);
        }
    }

    private void UpdateMiniMapIcons()
    {
        // scale icons by the inverse of the mapscale to keep them a consitent size
        float iconScale = 1 / _contentRectTransform.transform.localScale.x;
        foreach (var kvp in _miniMapWorldObjectsLookup)
        {
            var miniMapWorldObject = kvp.Key;
            var miniMapIcon = kvp.Value;
            var mapPosition = WorldPositionToMapPosition(miniMapWorldObject.transform.position);

            miniMapIcon.RectTransform.anchoredPosition = mapPosition;
            var rotation = miniMapWorldObject.transform.rotation.eulerAngles;
            miniMapIcon.IconRectTransform.localRotation = Quaternion.AngleAxis(-rotation.y, Vector3.forward);
            miniMapIcon.IconRectTransform.localScale = Vector3.one * iconScale;
        }
    }

    private Vector2 WorldPositionToMapPosition(Vector3 worldPos)
    {
        var pos = new Vector2(worldPos.x, worldPos.z) + _offset;
        return _transformationMatrix.MultiplyPoint3x4(pos);
    }


    private void CalculateTransformationMatrix()
    {
        var minimapSize = _contentRectTransform.rect.size;
        var worldSize = new Vector2(this._worldSize.x, this._worldSize.y);

        var translation = -minimapSize / 2;
        var scaleRatio = minimapSize / worldSize;

        _transformationMatrix = Matrix4x4.TRS(translation, Quaternion.identity, scaleRatio);

        //  {scaleRatio.x,   0,           0,   translation.x},
        //  {  0,        scaleRatio.y,    0,   translation.y},
        //  {  0,            0,           1,            0},
        //  {  0,            0,           0,            1}
    }
}