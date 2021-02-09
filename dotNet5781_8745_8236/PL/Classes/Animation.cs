using System;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace PL
{
    /// <summary>
    /// Class for animation
    /// </summary>
    static class Animation
    {
        /// <summary>
        /// Double animation, for all animations
        /// </summary>
        private static DoubleAnimation anim = new DoubleAnimation();
        /// <summary>
        /// Max text size
        /// </summary>
        private static double maxSize;
        /// <summary>
        /// Originial text size = 30 for preventing bags
        /// </summary>
        private const double ORIGINAL = 30;
        /// <summary>
        /// Extention method for Buttons, increases the text size in animation.
        /// Time of the animation is 0.5 seconds
        /// </summary>
        /// <param name="bt">The button</param>
        /// <param name="addSize">Amount of font size to add</param>
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
        /// <summary>
        /// Decreasing back text size.
        /// Time of the animation is 0.5 seconds
        /// </summary>
        /// <param name="bt">The button</param>
        public static void Decrease(this Button bt)
        {
            anim.From = bt.FontSize;
            anim.To = ORIGINAL;
            anim.Duration = TimeSpan.FromMilliseconds(500);
            anim.RepeatBehavior = new RepeatBehavior(1);
            anim.AutoReverse = false;
            bt.BeginAnimation(Button.FontSizeProperty, anim);
        }
        /// <summary>
        /// Animation for image in canvas as extention method.
        /// The animation is from right to left.
        /// </summary>
        /// <param name="image">The image</param>
        /// <param name="canvas">The canvas</param>
        /// <param name="time">Time of animation</param>
        public static void StartAnimationForever(this Image image, Canvas canvas, double time)
        {
            anim.From = -image.ActualWidth;
            anim.To = canvas.ActualWidth;
            anim.Duration = TimeSpan.FromSeconds(time);

            //anim.AutoReverse = true;
            anim.RepeatBehavior = RepeatBehavior.Forever;

            image.BeginAnimation(Canvas.RightProperty, anim);
        }
    }
}
