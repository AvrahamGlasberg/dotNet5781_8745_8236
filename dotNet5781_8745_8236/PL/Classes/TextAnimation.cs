using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PL
{ 
    static class TextAnimation
    {
        private static DoubleAnimation anim = new DoubleAnimation();
        private static double maxSize;
        private const double ORIGINAL = 30;
        static TextAnimation()
        {
            anim.Duration = TimeSpan.FromMilliseconds(500);
        }
        public static void Increase(this Button bt, double addSize)
        {
            maxSize = addSize + bt.FontSize;

            anim.From = ORIGINAL;
            anim.To = maxSize;
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
        public static void Decrease(this Button bt)
        {
            anim.From = bt.FontSize;
            anim.To = ORIGINAL;
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
    }
}
