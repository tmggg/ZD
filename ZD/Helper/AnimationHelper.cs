using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace SgS.Helper
{
    public  class AnimationHelper
    {
        public static void CreateWidthChangedAnimation( UIElement element, double form, double to, TimeSpan span)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = form;
            doubleAnimation.To = to;
            doubleAnimation.Duration = span;
            Storyboard.SetTarget(doubleAnimation, element);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Width"));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }
    }
}
