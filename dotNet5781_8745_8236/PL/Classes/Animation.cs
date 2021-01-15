using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PL
{ 
    static class Animation
    {
        private static DoubleAnimation anim = new DoubleAnimation();
        private static double maxSize;
        private const double ORIGINAL = 30;
        public static void Increase(this Button bt, double addSize)
        {
            maxSize = addSize + bt.FontSize;

            anim.From = ORIGINAL;
            anim.To = maxSize;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            anim.RepeatBehavior = new RepeatBehavior(1);
            anim.AutoReverse = false;
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
        public static void Decrease(this Button bt)
        {
            anim.From = bt.FontSize;
            anim.To = ORIGINAL;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            anim.RepeatBehavior = new RepeatBehavior(1);
            anim.AutoReverse = false;
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }

        public static void StartAnimationForever(this Image image, Canvas canvas, double time)
        {
            anim.From = -image.ActualWidth;
            anim.To = canvas.ActualWidth;
            anim.Duration = TimeSpan.FromSeconds(time);

            anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;

            image.BeginAnimation(Canvas.RightProperty, anim);
        }
    }
}
