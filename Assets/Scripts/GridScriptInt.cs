using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridScriptInt
{

    public const int HEAT_MAP_MAX_VALUE = 100;
    public const int HEAT_MAP_MIN_VALUE = 0;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }
    private int width;
    private int height;
    private float cellSize;
    private int[,] gridArray;
    private Vector3 originPos;

    public GridScriptInt(int width, int height, float cellSize, Vector3 originPos)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPos = originPos;
        gridArray = new int[width, height];
    }

    //������
    public int GetWidth()
    {
        return width;
    }

    //������
    public int GetHeight()
    {
        return height;
    }

    //������� � ����(�����)
    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPos;
    }

    //������ ������
    public float GetCellSize()
    {
        return cellSize;
    }

    //���������� �������
    private void GetXY(Vector3 worldPos, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPos - originPos).x / cellSize);
        y = Mathf.FloorToInt((worldPos - originPos).y / cellSize);
    }

    //���������� �������� ������ �� �����������
    public void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = Mathf.Clamp(value, HEAT_MAP_MIN_VALUE, HEAT_MAP_MAX_VALUE);
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    //���������� �������� ������ �� ������� � ����
    public void SetValue(Vector3 worldPos, int value)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        SetValue(x, y, value);
    }

    //�������� �������� � ������ ����� ��������
    public void AddValue(int x, int y, int value)
    {
        SetValue(x, y, GetValue(x, y) + value);
    }

    //�������� �������� �� �����������
    public int GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        } 
        else
        {
            return 0;
        }
    }

    //�������� �������� �� ������� � ����
    public int GetValue(Vector3 worldPos)
    {
        int x, y;
        GetXY(worldPos, out x, out y);
        return GetValue(x, y);
    }

    //�� ������� � ���� �������� �������� � ���������� �������
    public void AddValue(Vector3 worldPos, int value, int fullValueRange, int totalRange)
    {
        int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));

        GetXY(worldPos, out int originX, out int originY);
        for (int x = 0; x < totalRange; x++)
        {
            for (int y = 0; y < totalRange - x; y++)
            {
                int radius = x + y;
                int addValueAmount = value;
                if (radius >= fullValueRange)
                {
                    addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                }

                AddValue(originX + x, originY + y, addValueAmount);

                if (x != 0)
                {
                    AddValue(originX - x, originY + y, addValueAmount);
                }
                if (y != 0)
                {
                    AddValue(originX + x, originY - y, addValueAmount);
                    if (x != 0)
                    {
                        AddValue(originX - x, originY - y, addValueAmount);
                    }
                }
            }
        }
    }
}
