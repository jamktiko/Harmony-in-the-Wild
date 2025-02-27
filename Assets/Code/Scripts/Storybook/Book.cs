//The implementation is based on this article:http://rbarraza.com/html5-canvas-pageflip/
//As the rbarraza.com website is not live anymore you can get an archived version from web archive 
//or check an archived version that I uploaded on my website: https://dandarawy.com/html5-canvas-pageflip/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum FlipMode
{
    RightToLeft,
    LeftToRight
}

[Serializable]
public class StorybookSection
{
    [FormerlySerializedAs("nameOfSection")] public string NameOfSection;
    [FormerlySerializedAs("storybookImages")] public Sprite[] StorybookImages;
}

//[ExecuteInEditMode]
public class Book : MonoBehaviour
{
    [FormerlySerializedAs("canvas")] public Canvas Canvas;
    [FormerlySerializedAs("BookPanel")] [SerializeField]
    private RectTransform _bookPanel;
    [FormerlySerializedAs("background")] public Sprite Background;
    [FormerlySerializedAs("bookPages")] public Sprite[] BookPages;
    [FormerlySerializedAs("interactable")] public bool Interactable = true;
    [FormerlySerializedAs("enableShadowEffect")] public bool EnableShadowEffect = true;
    //represent the index of the sprite shown in the right page
    [FormerlySerializedAs("currentPage")] public int CurrentPage = 0;
    public int totalPageCount
    {
        get { return _storybookSections[_currentStorybookIndex].StorybookImages.Length; }
    }
    public Vector3 endBottomLeft
    {
        get { return _ebl; }
    }
    public Vector3 endBottomRight
    {
        get { return _ebr; }
    }
    public float height
    {
        get
        {
            return _bookPanel.rect.height;
        }
    }
    public Image ClippingPlane;
    public Image NextPageClip;
    public Image Shadow;
    [FormerlySerializedAs("ShadowLTR")] public Image ShadowLtr;
    public Image Left;
    public Image LeftNext;
    public Image Right;
    public Image RightNext;
    public UnityEvent OnFlip;

    private float _radius1, _radius2;
    //Spine Bottom
    private Vector3 _sb;
    //Spine Top
    private Vector3 _st;
    //corner of the page
    private Vector3 _c;
    //Edge Bottom Right
    private Vector3 _ebr;
    //Edge Bottom Left
    private Vector3 _ebl;
    //follow point 
    private Vector3 _f;

    private bool _pageDragging = false;
    //current flip mode
    private FlipMode _mode;
    private Coroutine _currentCoroutine;

    [FormerlySerializedAs("storybookSections")]
    [Header("Config")]
    [SerializeField] private List<StorybookSection> _storybookSections;
    [FormerlySerializedAs("currentStorybookIndex")] [SerializeField] private int _currentStorybookIndex;
    [FormerlySerializedAs("nextScene")] [SerializeField] private string _nextScene;

    [FormerlySerializedAs("instructions")]
    [Header("Instructions")]
    [SerializeField] private GameObject _instructions;

    private void Start()
    {
        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.1f);
        }

        // set the storybook materials to match the corresponding section in the game
        if (Application.isPlaying)
        {
            _currentStorybookIndex = StorybookHandler.Instance.GetCurrentStorybookSection();

            BookPages = _storybookSections[_currentStorybookIndex].StorybookImages;
        }

        if (!Canvas) Canvas = GetComponentInParent<Canvas>();
        if (!Canvas) Debug.LogError("Book should be a child to canvas");

        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        UpdateSprites();
        CalcCurlCriticalPoints();

        float pageWidth = _bookPanel.rect.width / 2.0f;
        float pageHeight = _bookPanel.rect.height;
        NextPageClip.rectTransform.sizeDelta = new Vector2(pageWidth, pageHeight + pageHeight * 2);


        ClippingPlane.rectTransform.sizeDelta = new Vector2(pageWidth * 2 + pageHeight, pageHeight + pageHeight * 2);

        //hypotenous (diagonal) page length
        float hyp = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
        float shadowPageHeight = pageWidth / 2 + hyp;

        Shadow.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        Shadow.rectTransform.pivot = new Vector2(1, (pageWidth / 2) / shadowPageHeight);

        ShadowLtr.rectTransform.sizeDelta = new Vector2(pageWidth, shadowPageHeight);
        ShadowLtr.rectTransform.pivot = new Vector2(0, (pageWidth / 2) / shadowPageHeight);
    }

    public int SetMaxSpreads()
    {
        int spreads = BookPages.Length / 2;

        return spreads;
    }

    private void CalcCurlCriticalPoints()
    {
        _sb = new Vector3(0, -_bookPanel.rect.height / 2);
        _ebr = new Vector3(_bookPanel.rect.width / 2, -_bookPanel.rect.height / 2);
        _ebl = new Vector3(-_bookPanel.rect.width / 2, -_bookPanel.rect.height / 2);
        _st = new Vector3(0, _bookPanel.rect.height / 2);
        _radius1 = Vector2.Distance(_sb, _ebr);
        float pageWidth = _bookPanel.rect.width / 2.0f;
        float pageHeight = _bookPanel.rect.height;
        _radius2 = Mathf.Sqrt(pageWidth * pageWidth + pageHeight * pageHeight);
    }

    public Vector3 TransformPoint(Vector3 mouseScreenPos)
    {
        if (Canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Vector3 mouseWorldPos = Canvas.worldCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Canvas.planeDistance));
            Vector2 localPos = _bookPanel.InverseTransformPoint(mouseWorldPos);

            return localPos;
        }
        else if (Canvas.renderMode == RenderMode.WorldSpace)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 globalEbr = transform.TransformPoint(_ebr);
            Vector3 globalEbl = transform.TransformPoint(_ebl);
            Vector3 globalSt = transform.TransformPoint(_st);
            Plane p = new Plane(globalEbr, globalEbl, globalSt);
            float distance;
            p.Raycast(ray, out distance);
            Vector2 localPos = _bookPanel.InverseTransformPoint(ray.GetPoint(distance));
            return localPos;
        }
        else
        {
            //Screen Space Overlay
            Vector2 localPos = _bookPanel.InverseTransformPoint(mouseScreenPos);
            return localPos;
        }
    }

    public void UpdateBookRtlToPoint(Vector3 followLocation)
    {
        // make sure the instructions are hidden when starting the first flip
        _instructions.SetActive(false);

        _mode = FlipMode.RightToLeft;
        _f = followLocation;
        Shadow.transform.SetParent(ClippingPlane.transform, true);
        Shadow.transform.localPosition = Vector3.zero;
        Shadow.transform.localEulerAngles = Vector3.zero;
        Right.transform.SetParent(ClippingPlane.transform, true);

        Left.transform.SetParent(_bookPanel.transform, true);
        Left.transform.localEulerAngles = Vector3.zero;
        RightNext.transform.SetParent(_bookPanel.transform, true);
        _c = Calc_C_Position(followLocation);
        Vector3 t1;
        float clipAngle = CalcClipAngle(_c, _ebr, out t1);
        if (clipAngle > -90) clipAngle += 180;

        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);
        ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        ClippingPlane.transform.position = _bookPanel.TransformPoint(t1);

        //page position and angle
        Right.transform.position = _bookPanel.TransformPoint(_c);
        float cT1Dy = t1.y - _c.y;
        float cT1Dx = t1.x - _c.x;
        float cT1Angle = Mathf.Atan2(cT1Dy, cT1Dx) * Mathf.Rad2Deg;
        Right.transform.localEulerAngles = new Vector3(0, 0, cT1Angle - (clipAngle + 90));

        NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle + 90);
        NextPageClip.transform.position = _bookPanel.TransformPoint(t1);
        RightNext.transform.SetParent(NextPageClip.transform, true);
        Left.transform.SetParent(ClippingPlane.transform, true);
        Left.transform.SetAsFirstSibling();

        Shadow.rectTransform.SetParent(Right.rectTransform, true);
    }
    private float CalcClipAngle(Vector3 c, Vector3 bookCorner, out Vector3 t1)
    {
        Vector3 t0 = (c + bookCorner) / 2;
        float t0CornerDy = bookCorner.y - t0.y;
        float t0CornerDx = bookCorner.x - t0.x;
        float t0CornerAngle = Mathf.Atan2(t0CornerDy, t0CornerDx);
        float t0T1Angle = 90 - t0CornerAngle;

        float t1X = t0.x - t0CornerDy * Mathf.Tan(t0CornerAngle);
        t1X = NormalizeT1X(t1X, bookCorner, _sb);
        t1 = new Vector3(t1X, _sb.y, 0);

        //clipping plane angle=T0_T1_Angle
        float t0T1Dy = t1.y - t0.y;
        float t0T1Dx = t1.x - t0.x;
        t0T1Angle = Mathf.Atan2(t0T1Dy, t0T1Dx) * Mathf.Rad2Deg;
        return t0T1Angle;
    }
    private float NormalizeT1X(float t1, Vector3 corner, Vector3 sb)
    {
        if (t1 > sb.x && sb.x > corner.x)
            return sb.x;
        if (t1 < sb.x && sb.x < corner.x)
            return sb.x;
        return t1;
    }
    private Vector3 Calc_C_Position(Vector3 followLocation)
    {
        Vector3 c;
        _f = followLocation;
        float fSbDy = _f.y - _sb.y;
        float fSbDx = _f.x - _sb.x;
        float fSbAngle = Mathf.Atan2(fSbDy, fSbDx);
        Vector3 r1 = new Vector3(_radius1 * Mathf.Cos(fSbAngle), _radius1 * Mathf.Sin(fSbAngle), 0) + _sb;

        float fSbDistance = Vector2.Distance(_f, _sb);
        if (fSbDistance < _radius1)
            c = _f;
        else
            c = r1;
        float fStDy = c.y - _st.y;
        float fStDx = c.x - _st.x;
        float fStAngle = Mathf.Atan2(fStDy, fStDx);
        Vector3 r2 = new Vector3(_radius2 * Mathf.Cos(fStAngle),
           _radius2 * Mathf.Sin(fStAngle), 0) + _st;
        float cStDistance = Vector2.Distance(c, _st);
        if (cStDistance > _radius2)
            c = r2;
        return c;
    }
    public void DragRightPageToPoint(Vector3 point)
    {
        if (CurrentPage >= BookPages.Length) return;

        _pageDragging = true;
        _mode = FlipMode.RightToLeft;
        _f = point;


        NextPageClip.rectTransform.pivot = new Vector2(0, 0.12f);
        ClippingPlane.rectTransform.pivot = new Vector2(1, 0.35f);

        Left.gameObject.SetActive(true);
        Left.rectTransform.pivot = new Vector2(0, 0);
        Left.transform.position = RightNext.transform.position;
        Left.transform.eulerAngles = new Vector3(0, 0, 0);
        Left.sprite = (CurrentPage < BookPages.Length) ? BookPages[CurrentPage] : Background;
        Left.transform.SetAsFirstSibling();

        Right.gameObject.SetActive(true);
        Right.transform.position = RightNext.transform.position;
        Right.transform.eulerAngles = new Vector3(0, 0, 0);
        Right.sprite = (CurrentPage < BookPages.Length - 1) ? BookPages[CurrentPage + 1] : Background;

        RightNext.sprite = (CurrentPage < BookPages.Length - 2) ? BookPages[CurrentPage + 2] : Background;

        LeftNext.transform.SetAsFirstSibling();
        if (EnableShadowEffect) Shadow.gameObject.SetActive(true);
        UpdateBookRtlToPoint(_f);
    }

    public void OnMouseRelease()
    {
        if (Interactable)
            ReleasePage();
    }
    public void ReleasePage()
    {
        if (_pageDragging)
        {
            _pageDragging = false;
            float distanceToLeft = Vector2.Distance(_c, _ebl);
            float distanceToRight = Vector2.Distance(_c, _ebr);
            if (distanceToRight < distanceToLeft && _mode == FlipMode.RightToLeft)
                TweenBack();
            else if (distanceToRight > distanceToLeft && _mode == FlipMode.LeftToRight)
                TweenBack();
            else
                TweenForward();
        }

        StartCoroutine(StartPageDelay());
    }

    private void UpdateSprites()
    {
        LeftNext.sprite = (CurrentPage > 0 && CurrentPage <= BookPages.Length) ? BookPages[CurrentPage - 1] : Background;
        RightNext.sprite = (CurrentPage >= 0 && CurrentPage < BookPages.Length) ? BookPages[CurrentPage] : Background;
    }
    public void TweenForward()
    {
        if (_mode == FlipMode.RightToLeft)
            _currentCoroutine = StartCoroutine(TweenTo(_ebl, 0.15f, () => { Flip(); }));
        else
            _currentCoroutine = StartCoroutine(TweenTo(_ebr, 0.15f, () => { Flip(); }));
    }

    private void Flip()
    {
        if (_mode == FlipMode.RightToLeft)
            CurrentPage += 2;
        else
            CurrentPage -= 2;
        LeftNext.transform.SetParent(_bookPanel.transform, true);
        Left.transform.SetParent(_bookPanel.transform, true);
        LeftNext.transform.SetParent(_bookPanel.transform, true);
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        Right.transform.SetParent(_bookPanel.transform, true);
        RightNext.transform.SetParent(_bookPanel.transform, true);
        UpdateSprites();
        Shadow.gameObject.SetActive(false);
        ShadowLtr.gameObject.SetActive(false);
        if (OnFlip != null)
            OnFlip.Invoke();
    }
    public void TweenBack()
    {
        if (_mode == FlipMode.RightToLeft)
        {
            _currentCoroutine = StartCoroutine(TweenTo(_ebr, 0.15f,
                () =>
                {
                    UpdateSprites();
                    RightNext.transform.SetParent(_bookPanel.transform);
                    Right.transform.SetParent(_bookPanel.transform);

                    Left.gameObject.SetActive(false);
                    Right.gameObject.SetActive(false);
                    _pageDragging = false;
                }
                ));
        }
        else
        {
            _currentCoroutine = StartCoroutine(TweenTo(_ebl, 0.15f,
                () =>
                {
                    UpdateSprites();

                    LeftNext.transform.SetParent(_bookPanel.transform);
                    Left.transform.SetParent(_bookPanel.transform);

                    Left.gameObject.SetActive(false);
                    Right.gameObject.SetActive(false);
                    _pageDragging = false;
                }
                ));
        }
    }
    public IEnumerator TweenTo(Vector3 to, float duration, System.Action onFinish)
    {
        int steps = (int)(duration / 0.025f);
        Vector3 displacement = (to - _f) / steps;
        for (int i = 0; i < steps - 1; i++)
        {
            if (_mode == FlipMode.RightToLeft)
                UpdateBookRtlToPoint(_f + displacement);
            //else
            //    UpdateBookLTRToPoint(f + displacement);

            yield return new WaitForSeconds(0.025f);
        }
        if (onFinish != null)
            onFinish();
    }

    private IEnumerator StartPageDelay()
    {
        yield return new WaitForSeconds(0.125f);

        LeftNext.GetComponent<Image>().color = new Color(255, 255, 255, 255);
    }
}


// NOTE! UNNECESSARY CODE FROM THE ASSETS
// NOTE! CAN BE REMOVED LATER IF THEY ARE NOT NEEDED AT ANY STAGE OF THE DEVELOPMENT

/*public void OnMouseDragRightPage()
{
    if (interactable)
    DragRightPageToPoint(transformPoint(Input.mousePosition));

}
public void DragLeftPageToPoint(Vector3 point)
{
    if (currentPage <= 0) return;
    pageDragging = true;
    mode = FlipMode.LeftToRight;
    f = point;

    NextPageClip.rectTransform.pivot = new Vector2(1, 0.12f);
    ClippingPlane.rectTransform.pivot = new Vector2(0, 0.35f);

    Right.gameObject.SetActive(true);
    Right.transform.position = LeftNext.transform.position;
    Right.sprite = bookPages[currentPage - 1];
    Right.transform.eulerAngles = new Vector3(0, 0, 0);
    Right.transform.SetAsFirstSibling();

    Left.gameObject.SetActive(true);
    Left.rectTransform.pivot = new Vector2(1, 0);
    Left.transform.position = LeftNext.transform.position;
    Left.transform.eulerAngles = new Vector3(0, 0, 0);
    Left.sprite = (currentPage >= 2) ? bookPages[currentPage - 2] : background;

    LeftNext.sprite = (currentPage >= 3) ? bookPages[currentPage - 3] : background;

    RightNext.transform.SetAsFirstSibling();
    if (enableShadowEffect) ShadowLTR.gameObject.SetActive(true);
    UpdateBookLTRToPoint(f);
}
public void OnMouseDragLeftPage()
{
    if (interactable)
    DragLeftPageToPoint(transformPoint(Input.mousePosition));

}*/

/*void Update()
{
    if (pageDragging && interactable)
    {
        UpdateBook();
    }
}
public void UpdateBook()
{
    f = Vector3.Lerp(f, transformPoint(Input.mousePosition), Time.deltaTime * 10);

    UpdateBookRTLToPoint(f);
}
/*public void UpdateBookLTRToPoint(Vector3 followLocation)
{
    mode = FlipMode.LeftToRight;
    f = followLocation;
    ShadowLTR.transform.SetParent(ClippingPlane.transform, true);
    ShadowLTR.transform.localPosition = new Vector3(0, 0, 0);
    ShadowLTR.transform.localEulerAngles = new Vector3(0, 0, 0);
    Left.transform.SetParent(ClippingPlane.transform, true);

    Right.transform.SetParent(BookPanel.transform, true);
    Right.transform.localEulerAngles = Vector3.zero;
    LeftNext.transform.SetParent(BookPanel.transform, true);

    c = Calc_C_Position(followLocation);
    Vector3 t1;
    float clipAngle = CalcClipAngle(c, ebl, out t1);
    //0 < T0_T1_Angle < 180
    clipAngle = (clipAngle + 180) % 180;

    ClippingPlane.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
    ClippingPlane.transform.position = BookPanel.TransformPoint(t1);

    //page position and angle
    Left.transform.position = BookPanel.TransformPoint(c);
    float C_T1_dy = t1.y - c.y;
    float C_T1_dx = t1.x - c.x;
    float C_T1_Angle = Mathf.Atan2(C_T1_dy, C_T1_dx) * Mathf.Rad2Deg;
    Left.transform.localEulerAngles = new Vector3(0, 0, C_T1_Angle - 90 - clipAngle);

    NextPageClip.transform.localEulerAngles = new Vector3(0, 0, clipAngle - 90);
    NextPageClip.transform.position = BookPanel.TransformPoint(t1);
    LeftNext.transform.SetParent(NextPageClip.transform, true);
    Right.transform.SetParent(ClippingPlane.transform, true);
    Right.transform.SetAsFirstSibling();

    ShadowLTR.rectTransform.SetParent(Left.rectTransform, true);
}*/