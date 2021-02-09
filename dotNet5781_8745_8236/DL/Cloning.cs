using System;

namespace DL
{
    /// <summary>
    /// Static class for generic cloning
    /// </summary>
    static class Cloning
    {
        /// <summary>
        /// Clone an T object
        /// </summary>
        /// <typeparam name="T">Simple class (public simple properties only)</typeparam>
        /// <param name="original">Original object</param>
        /// <returns>The cloned object</returns>
        public static T Clone<T>(this T original)
        {
            T newObject = (T)Activator.CreateInstance(original.GetType());
            foreach (var originalProp in original.GetType().GetProperties())
            {
                originalProp.SetValue(newObject, originalProp.GetValue(original));
            }
            return newObject;
        }
    }
}
