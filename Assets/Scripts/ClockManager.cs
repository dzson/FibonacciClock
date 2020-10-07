using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    public int NumPoints;
    public Sprite Sprite;
    public Color defaultColor;
    public Color secColor;
    public Color minColor;
    public Color hourColor;
    public TextMeshProUGUI digitalClock;

    private float turnFraction = 0.618033f;
    private int highlight = 34;
    private int offset = 13;
    private int secOffset = 52;
    private int minOffset = 52;
    private int hourOffset = 52;

    private SpriteRenderer[] sprites;

    private void Start()
    {
        sprites = new SpriteRenderer[NumPoints];

        for (int i = 0; i < NumPoints; i++)
        {
            float distance = i / (NumPoints - 1f);
            float angle = 2 * Mathf.PI * turnFraction * i;

            float x = distance * Mathf.Cos(angle) * 5;
            float y = distance * Mathf.Sin(angle) * 5;

            sprites[i] = PlotPoint(x, y, defaultColor);
        }
    }

    private void Update()
    {
        DateTime time = DateTime.Now;
        UpdateOffsets(time);
        UpdateDigitalClock(time);
        UpdateColors();
    }

    private SpriteRenderer PlotPoint(float x, float y, Color color)
    {
        var point = new GameObject();
        var spriteRenderer = point.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite;
        spriteRenderer.color = color;

        point.transform.SetParent(this.transform);
        point.transform.position = new Vector3(x, y);

        return spriteRenderer;
    }

    private void UpdateOffsets(DateTime time)
    {
        var secMultiplier = Map(time.Second, 0, 60, 0, highlight);
        var minMultiplier = Map(time.Minute, 0, 60, 0, highlight);
        var hourMultiplier = Map(time.Hour, 0, 60, 0, highlight);

        secOffset = 52 + offset * secMultiplier;
        minOffset = 52 + offset * minMultiplier;
        hourOffset = 52 + offset * hourMultiplier;
    }

    private void UpdateColors()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            var color = defaultColor;
            if ((i + secOffset) % highlight == 0)
                color = secColor;
            else if ((i + minOffset) % highlight == 0)
                color = minColor;
            else if ((i + hourOffset) % highlight == 0)
                color = hourColor;

            sprites[i].color = color;
        }
    }

    private void UpdateDigitalClock(DateTime time)
    {
        digitalClock.text = ColorString(time.Hour.ToString(), hourColor)
                          + ColorString(":", defaultColor)
                          + ColorString(time.Minute.ToString(), minColor)
                          + ColorString(":", defaultColor)
                          + ColorString(time.Second.ToString(), secColor);
    }

    private string ColorString(string str, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{str}</color>";
    }

    private int Map(float n, float start1, float stop1, float start2, float stop2)
    {
        return Convert.ToInt32(((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2);
    }
}
