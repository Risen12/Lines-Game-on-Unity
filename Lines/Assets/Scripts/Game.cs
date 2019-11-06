using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public GameObject object1 = new GameObject();
    public AudioSource source;
    public Text text;
    [HideInInspector]
    public AudioClip clip;


    Button[,] buttons;
    Image[] images;
    Lines lines;

    void Start()
    {
        object1.SetActive(false);
        lines = new Lines(ShowBox, PlayCut, PlayEnd, PlayStart);
        InitButtons();
        InitImages();
        ShowBox(1, 2, 3);
        lines.Start();
    }
    public void ShowBox(int x, int y, int ball)
    {
        buttons[x, y].GetComponent<Image>().sprite = images[ball].sprite;
    }
    public void PlayCut()
    {
        source = source.GetComponent<AudioSource>();
        clip = Resources.Load("cut", typeof(AudioClip)) as AudioClip;
        source.clip = clip;
        source.Play();
        text.text = lines.Record.ToString();
    }
     
    public void PlayStart()
    {
        source = source.GetComponent<AudioSource>();
        clip = Resources.Load("startgame", typeof(AudioClip)) as AudioClip;
        source.clip = clip;
        source.Play();

    } 
    public void PlayEnd()
    {
        source = source.GetComponent<AudioSource>();
        clip = Resources.Load("endgame", typeof(AudioClip)) as AudioClip;
        source.clip = clip;
        source.Play();
    }
    public void Click()
    {
        string name = EventSystem.current.currentSelectedGameObject.name;
        int nr = GetNmber(name);
        int x = nr % Lines.size;
        int y = nr / Lines.size;
        lines.Click(x,y);
        if (lines.IsGameOver)
        {
            object1.SetActive(true);

        }
    }
    private void InitButtons()
    {
        buttons = new Button[Lines.size, Lines.size];
        for (int nr = 0; nr < Lines.size * Lines.size; nr++)
            buttons[nr % Lines.size, nr / Lines.size] = GameObject.Find($"Button ({nr})").GetComponent<Button>();
    }
    private int GetNmber(string name)
    {
        Regex regex = new Regex("\\((\\d+)\\)");
        Match math = regex.Match(name);
        if (!math.Success)
            throw new System.Exception();
        Group group = math.Groups[1];
        return Convert.ToInt32(group.Value);
    }

    private void InitImages()
    {
        images = new Image[Lines.balls];
        for (int j = 0; j < Lines.balls; j++)
            images[j] = GameObject.Find($"Image ({j})").GetComponent<Image>();
    }

}
