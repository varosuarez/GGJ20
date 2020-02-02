using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class SreamVideo : MonoBehaviour
{
    public RawImage image;
    public VideoPlayer video;
    public AudioSource source;
    private float timeleft = 16;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayVideo());
    }
    private void Update()
    {
        timeleft -= Time.deltaTime;
        if (timeleft <= 0)
        {
            SceneManager.LoadScene("Zone1");
        }
    }

    IEnumerator PlayVideo()
    {
        video.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while(!video.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
        image.texture = video.texture;
        video.Play();
        source.Play();
    }
}
