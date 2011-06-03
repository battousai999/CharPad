using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad
{
    public class BindableEnum<T> where T : struct
    {
        private string name = null;
        private T value;

        public BindableEnum(T value)
        {
            this.value = value;
        }

        public BindableEnum(string name, T value)
        {
            if (!typeof(T).IsEnum)
                throw new InvalidOperationException("Cannot create this class on a type that is not an enum.");

            this.name = name;
            this.value = value;
        }

        public string Name
        {
            get
            {
                if (name == null)
                    return Enum.Format(typeof(T), value, "G");
                else
                    return name;
            }
        }

        public T Value { get { return value; } }

        public static List<BindableEnum<T>> BuildValues()
        {
            return BuildValues(null);
        }

        public static List<BindableEnum<T>> BuildValues(Dictionary<T, string> names)
        {
            return BuildValues(names, null);
        }

        public static List<BindableEnum<T>> BuildValues(Dictionary<T, string> names, List<T> excludedValues)
        {
            List<BindableEnum<T>> list = new List<BindableEnum<T>>();

            foreach (T iValue in Enum.GetValues(typeof(T)))
            {
                if ((excludedValues != null) && excludedValues.Contains(iValue))
                    continue;

                if ((names != null) && names.ContainsKey(iValue))
                    list.Add(new BindableEnum<T>(names[iValue], iValue));
                else
                    list.Add(new BindableEnum<T>(iValue));
            }

            return list;
        }
    }
}
