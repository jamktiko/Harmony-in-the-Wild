using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour
{
    public FlipMode Mode;
    //public float TimeBetweenPages = 1;
    //public float DelayBeforeStarting = 0;
    public bool AutoStartFlip = true;
    public Book ControledBook;
    public int AnimationFramesCount = 40;
    bool _isFlipping = false;
    // Use this for initialization

    private AudioSource _audioSource;
    private int _maxSpreads;
    private int _currentSpread;
    private SceneManagerHelper.Scene _nextScene;
    private bool _isAutoFlipping = false;
    private float _pageFlipTime = 1;

    [FormerlySerializedAs("delayBeforeAutoFlipStart")]
    [Header("Config")]
    [SerializeField] private float _delayBeforeAutoFlipStart;
    [FormerlySerializedAs("delayBetweenAutoFlippedPages")] [SerializeField] private float _delayBetweenAutoFlippedPages;
    [FormerlySerializedAs("autoFlipTime")] [SerializeField] private float _autoFlipTime = 0.2f;
    [FormerlySerializedAs("regularFlipTime")] [SerializeField] private float _regularFlipTime = 1.7f;

    void Start()
    {

        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (!ControledBook)
            ControledBook = GetComponent<Book>();
        /*if (AutoStartFlip)
            StartFlipping();*/
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));

        _maxSpreads = ControledBook.SetMaxSpreads();

        _nextScene = StorybookHandler.Instance.GetNextScene();

        if (StorybookHandler.Instance.CheckForDungeonEnding())
        {
            _pageFlipTime = _autoFlipTime;
            _isAutoFlipping = true;
            StartCoroutine(AutoFlipToDungeonEnding());
        }
    }

    private void Update()
    {
        if (PlayerInputHandler.Instance.JumpInput.WasPressedThisFrame() && !_isAutoFlipping)
        {
            FlipRightPage();
        }
    }

    void PageFlipped()
    {
        _isFlipping = false;
    }

    public void FlipRightPage()
    {
        if (_isFlipping) return;
        _currentSpread++;

        if (_currentSpread > _maxSpreads)
        {
            GameEventsManager.instance.UIEvents.ShowLoadingScreen(_nextScene);
            return;
        }


        if (ControledBook.CurrentPage >= ControledBook.totalPageCount) return;
        _isFlipping = true;
        float frameTime = _pageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.endBottomRight.x + ControledBook.endBottomLeft.x) / 2;
        float xl = ((ControledBook.endBottomRight.x - ControledBook.endBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.endBottomRight.y) * 0.9f;
        float dx = (xl) * 2 / AnimationFramesCount;

        PlayFlipSound();

        StartCoroutine(FlipRtl(xc, xl, h, frameTime, dx));
    }

    IEnumerator FlipRtl(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));

        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRtlToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }

        ControledBook.ReleasePage();
    }

    IEnumerator AutoFlipToDungeonEnding()
    {
        yield return new WaitForSeconds(_delayBeforeAutoFlipStart);

        float frameTime = _autoFlipTime / AnimationFramesCount;
        float xc = (ControledBook.endBottomRight.x + ControledBook.endBottomLeft.x) / 2;
        float xl = ((ControledBook.endBottomRight.x - ControledBook.endBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.endBottomRight.y) * 0.9f;
        //y=-(h/(xl)^2)*(x-xc)^2          
        //               y         
        //               |          
        //               |          
        //               |          
        //_______________|_________________x         
        //              o|o             |
        //           o   |   o          |
        //         o     |     o        | h
        //        o      |      o       |
        //       o------xc-------o      -
        //               |<--xl-->
        //               |
        //               |
        float dx = (xl) * 2 / AnimationFramesCount;
        switch (Mode)
        {
            case FlipMode.RightToLeft:
                while (ControledBook.CurrentPage < ControledBook.totalPageCount - 2)
                {
                    _currentSpread++;
                    PlayFlipSound();
                    StartCoroutine(FlipRtl(xc, xl, h, frameTime, dx));
                    yield return new WaitForSeconds(_delayBetweenAutoFlippedPages);
                }

                _pageFlipTime = _regularFlipTime;
                _isAutoFlipping = false;
                break;
        }
    }

    private void PlayFlipSound()
    {
        AudioManager.Instance.PlaySound(AudioName.PropStorybookPage, transform);
    }
}

// NOTE CODE FROM ASSET THAT ARE CURRENTLY NOT NECESSARY
// NOTE CAN BE REMOVED ONCE THEY ARE DEFINITELY NOT NEEDED ANYMORE

/*public void StartFlipping()
{
    StartCoroutine(FlipToEnd());
}*/

/*public void FlipLeftPage()
   {
       if (isFlipping) return;
       if (ControledBook.currentPage <= 0) return;
       isFlipping = true;
       float frameTime = PageFlipTime / AnimationFramesCount;
       float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
       float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
       //float h =  ControledBook.Height * 0.5f;
       float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
       float dx = (xl) * 2 / AnimationFramesCount;
       StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
   }
   IEnumerator FlipToEnd()
   {
       yield return new WaitForSeconds(DelayBeforeStarting);
       float frameTime = PageFlipTime / AnimationFramesCount;
       float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
       float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2)*0.9f;
       //float h =  ControledBook.Height * 0.5f;
       float h = Mathf.Abs(ControledBook.EndBottomRight.y)*0.9f;
       //y=-(h/(xl)^2)*(x-xc)^2          
       //               y         
       //               |          
       //               |          
       //               |          
       //_______________|_________________x         
       //              o|o             |
       //           o   |   o          |
       //         o     |     o        | h
       //        o      |      o       |
       //       o------xc-------o      -
       //               |<--xl-->
       //               |
       //               |
       float dx = (xl)*2 / AnimationFramesCount;
       switch (Mode)
       {
           case FlipMode.RightToLeft:
               while (ControledBook.currentPage < ControledBook.TotalPageCount)
               {
                   StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
                   yield return new WaitForSeconds(TimeBetweenPages);
               }
               break;
           case FlipMode.LeftToRight:
               while (ControledBook.currentPage > 0)
               {
                   StartCoroutine(FlipLTR(xc, xl, h, frameTime, dx));
                   yield return new WaitForSeconds(TimeBetweenPages);
               }
               break;
       }
   }*/

/*IEnumerator FlipLTR(float xc, float xl, float h, float frameTime, float dx)
{
    float x = xc - xl;
    float y = (-h / (xl * xl)) * (x - xc) * (x - xc);
    ControledBook.DragLeftPageToPoint(new Vector3(x, y, 0));
    for (int i = 0; i < AnimationFramesCount; i++)
    {
        y = (-h / (xl * xl)) * (x - xc) * (x - xc);
        ControledBook.UpdateBookLTRToPoint(new Vector3(x, y, 0));
        yield return new WaitForSeconds(frameTime);
        x += dx;
    }
    ControledBook.ReleasePage();
}*/
