using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL
{
    static class Cloning
    {
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
