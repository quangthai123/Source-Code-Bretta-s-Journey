using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public float baseValue;
    public List<float> modifiers;
    public List<int> rateModifiers;
    public Stat(float _baseValue)
    {
        baseValue = _baseValue;
        modifiers = new List<float>();
        rateModifiers = new List<int>();
    }
    public float GetValue()
    {
        float finalValue = baseValue;
        foreach (float modifier in modifiers)
        {
            finalValue += modifier;
        }
        float rateTotal = 0;
        foreach (int rateModifier in rateModifiers)
        {
            rateTotal += rateModifier;
        }
        finalValue += finalValue * rateTotal/100f;
        return finalValue;
    }
    public void AddRateModifier(int _rate)
    {
        rateModifiers.Add( _rate);
    }
    public void RemoveRateModifier(int _rate)
    {
        rateModifiers.Remove( _rate);
    }
    public void AddModifier(float _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(float _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
