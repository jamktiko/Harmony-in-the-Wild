﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Book))]
public class AutoFlip : MonoBehaviour {
    public FlipMode Mode;
    public float PageFlipTime = 1;
    //public float TimeBetweenPages = 1;
    //public float DelayBeforeStarting = 0;
    public bool AutoStartFlip=true;
    public Book ControledBook;
    public int AnimationFramesCount = 40;
    bool isFlipping = false;
    // Use this for initialization

    private AudioSource audioSource;
    private int maxSpreads;
    private int currentSpread;

    void Start () {
        if (!ControledBook)
            ControledBook = GetComponent<Book>();
        /*if (AutoStartFlip)
            StartFlipping();*/
        ControledBook.OnFlip.AddListener(new UnityEngine.Events.UnityAction(PageFlipped));

        audioSource = GetComponent<AudioSource>();

        maxSpreads = ControledBook.SetMaxSpreads();
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipRightPage();
        }
    }

    void PageFlipped()
    {
        isFlipping = false;
    }
	/*public void StartFlipping()
    {
        StartCoroutine(FlipToEnd());
    }*/
    public void FlipRightPage()
    {
        if (isFlipping) return;
        currentSpread++;

        if(currentSpread > maxSpreads-1)
        {
            SceneManager.LoadScene("Overworld");
            return;
        }

        
        if (ControledBook.currentPage >= ControledBook.TotalPageCount) return;
        isFlipping = true;
        float frameTime = PageFlipTime / AnimationFramesCount;
        float xc = (ControledBook.EndBottomRight.x + ControledBook.EndBottomLeft.x) / 2;
        float xl = ((ControledBook.EndBottomRight.x - ControledBook.EndBottomLeft.x) / 2) * 0.9f;
        //float h =  ControledBook.Height * 0.5f;
        float h = Mathf.Abs(ControledBook.EndBottomRight.y) * 0.9f;
        float dx = (xl)*2 / AnimationFramesCount;

        PlayFlipSound();

        StartCoroutine(FlipRTL(xc, xl, h, frameTime, dx));
    }
   
    IEnumerator FlipRTL(float xc, float xl, float h, float frameTime, float dx)
    {
        float x = xc + xl;
        float y = (-h / (xl * xl)) * (x - xc) * (x - xc);

        ControledBook.DragRightPageToPoint(new Vector3(x, y, 0));

        for (int i = 0; i < AnimationFramesCount; i++)
        {
            y = (-h / (xl * xl)) * (x - xc) * (x - xc);
            ControledBook.UpdateBookRTLToPoint(new Vector3(x, y, 0));
            yield return new WaitForSeconds(frameTime);
            x -= dx;
        }

        ControledBook.ReleasePage();
    }

    private void PlayFlipSound()
    {
        audioSource.Play();
    }
}

// NOTE CODE FROM ASSET THAT ARE CURRENTLY NOT NECESSARY
// NOTE CAN BE REMOVED ONCE THEY ARE DEFINITELY NOT NEEDED ANYMORE

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
