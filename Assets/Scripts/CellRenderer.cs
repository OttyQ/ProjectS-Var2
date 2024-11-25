using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellRenderer : MonoBehaviour
{
    private Image _cellImage;
    private int _maxDepth;

    public void Initialize(int maxDepth, int currentDepth)
    {
        _maxDepth = maxDepth;
        _cellImage = GetComponent<Image>();
        UpdateColor(currentDepth);
    }

    public void UpdateColor(int currentDepth)
    {
        if (_cellImage != null)
        {
            float brightness = Mathf.InverseLerp(0, _maxDepth, currentDepth);
            _cellImage.color = new Color(brightness, brightness, brightness);
        }
    }
}

