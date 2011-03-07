using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharPad.Framework
{
    public class Dice
    {
        #region

        [Flags]
        public enum DiceOptions { None = 0, RerollAndAddOnHighest = 1, Reroll1s = 2, Reroll1and2s = 4 }

        #endregion

        private static Dictionary<string, Dice> dieCache = new Dictionary<string, Dice>();
        private static Random _rand = new Random();
        private static object randLock = new object();

        private int number;
        private int _base;

        private Dice(int number, int _base)
        {
            this.number = number;
            this._base = _base;
        }

        private static int GetRandomInt(int minValue, int maxValue)
        {
            lock (randLock)
            {
                return _rand.Next(minValue, maxValue);
            }
        }

        private static double GetRandomDouble()
        {
            lock (randLock)
            {
                return _rand.NextDouble();
            }
        }

        public static Dice Get(int number, int _base)
        {
            string key = BuildKey(number, _base);

            if (dieCache.ContainsKey(key))
                return dieCache[key];
            else
            {
                Dice die = new Dice(number, _base);
                dieCache.Add(key, die);
                return die;
            }
        }

        public static Dice Get(int _base)
        {
            return Get(1, _base);
        }

        public static int Roll(int number, int _base, DiceOptions options)
        {
            return Get(number, _base).Roll(options);
        }

        public static int Roll(int _base, DiceOptions options)
        {
            return Roll(1, _base, options);
        }

        public static int Roll(int number, int _base)
        {
            return Roll(number, _base, DiceOptions.None);
        }

        public static int Roll(int _base)
        {
            return Roll(_base, DiceOptions.None);
        }

        private static string BuildKey(int number, int _base)
        {
            return (number.ToString() + "d" + _base.ToString());
        }

        public static bool RollPercent(int chance)
        {
            if (chance <= 0)
                return false;

            if (chance >= 100)
                return true;

            return (Dice.Get(1, 100).Roll() <= chance);
        }

        public static int GetRandomNumber(int lowNumber, int highNumber)
        {
            return GetRandomInt(lowNumber, highNumber + 1);
        }

        public static List<int> GetRandomNumbers(int lowNumber, int highNumber, int count, bool isUnique)
        {
            if (isUnique)
            {
                if (highNumber - lowNumber + 1 < count)
                    throw new InvalidOperationException(String.Format("Cannot return {0} unique numbers in the range between {1} and {2}.", count, lowNumber, highNumber));

                if (highNumber - lowNumber + 1 == count)
                    return new List<int>(Enumerable.Range(lowNumber, count));
            }

            List<int> results = new List<int>();

            while (results.Count < count)
            {
                int number = GetRandomNumber(lowNumber, highNumber);

                if (!isUnique || !results.Contains(number))
                    results.Add(number);
            }

            return results;
        }

        public static double GetRandomDouble(double exclusiveHighNumber)
        {
            return GetRandomDouble() * exclusiveHighNumber;
        }

        public int Number
        {
            get { return number; }
        }

        public int Base
        {
            get { return _base; }
        }

        public int Roll()
        {
            return Roll(DiceOptions.None);
        }

        public int Roll(DiceOptions options)
        {
            int value = 0;

            for (int i = 0; i < number; i++)
            {
                int minRoll = 1;

                if ((options & DiceOptions.Reroll1and2s) == DiceOptions.Reroll1and2s)
                    minRoll = 3;
                else if ((options & DiceOptions.Reroll1s) == DiceOptions.Reroll1s)
                    minRoll = 2;

                if ((options & DiceOptions.RerollAndAddOnHighest) == DiceOptions.RerollAndAddOnHighest)
                    value += InternalRollWithRerollAndAddHighest(_base, minRoll);
                else
                    value += GetRandomInt(minRoll, _base + 1);
            }

            return value;
        }

        private int InternalRollWithRerollAndAddHighest(int _base, int minRoll)
        {
            int value = GetRandomInt(minRoll, _base + 1);

            if (value == _base)
                value += InternalRollWithRerollAndAddHighest(minRoll, _base);

            return value;
        }

        public int RollMultiple(int numberOfRolls)
        {
            return RollMultiple(numberOfRolls, DiceOptions.None);
        }

        public int RollMultiple(int numberOfRolls, DiceOptions options)
        {
            int value = 0;

            for (int i = 0; i < numberOfRolls; i++)
            {
                value += Roll();
            }

            return value;
        }

        public int MinRoll
        {
            get { return number; }
        }

        public int MaxRoll
        {
            get { return (number * _base); }
        }

        public int AverageRoll
        {
            get { return ((MinRoll + MaxRoll) / 2); }
        }

        public string DisplayString
        {
            get { return GetDisplayString(Number, Base); }
        }

        public static string GetDisplayString(int number, int diceBase)
        {
            return String.Format("{0}d{1}", number, diceBase);
        }

        public Dice GetDice(int multiplier)
        {
            return Dice.Get(this.Number * multiplier, this.Base);
        }
    }
}
