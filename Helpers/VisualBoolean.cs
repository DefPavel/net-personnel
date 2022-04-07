﻿namespace AlphaPersonel.Services;

internal class VisualBoolean
{
    // Основная переменная
    public bool IsTrue { get; private set; }
    public bool IsFalse => !IsTrue;

    // Переобразуем структуру
    private static readonly VisualBoolean True = new() { IsTrue = true };
    private static readonly VisualBoolean False = new() { IsTrue = false };

    // Ставим комноненту статус Visible или Collapsed
    public Visibility IsTrueVisibility => IsTrue ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsFalseVisibility => IsFalse ? Visibility.Visible : Visibility.Collapsed;


    public static implicit operator VisualBoolean(bool isTrue)
    {
        return isTrue ? True : False;
    }
    public static implicit operator bool(VisualBoolean visualBoolean)
    {
        return visualBoolean.IsTrue;
    }
}

