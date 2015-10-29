namespace Monicais.Property
{
    using Monicais.ThrowHelper;
    using System;
    using System.Collections.Generic;

    public sealed class Priority
    {
        public const int Count = 0x10;
        public static readonly Priority EIGHTH = new Priority(8);
        public static readonly Priority ELEVENTH = new Priority(11);
        public static readonly Priority FIFTEENTH = new Priority(15);
        public static readonly Priority FIFTH = new Priority(5);
        public static readonly Priority FIRST = new Priority(1);
        public static readonly Priority FOURTEENTH = new Priority(14);
        public static readonly Priority FOURTH = new Priority(4);
        public static readonly Priority IMMEDIATE = new Priority(0);
        public static readonly Priority NINTH = new Priority(9);
        private static readonly Priority[] PRIORITIES = new Priority[] { IMMEDIATE, FIRST, SECOND, THIRD, FOURTH, FIFTH, SIXTH, SEVENTH, EIGHTH, NINTH, TENTH, ELEVENTH, TWELFTH, THIRTEENTH, FOURTEENTH, FIFTEENTH };
        private int priority;
        public static readonly Priority SECOND = new Priority(2);
        public static readonly Priority SEVENTH = new Priority(7);
        public static readonly Priority SIXTH = new Priority(6);
        public static readonly Priority TENTH = new Priority(10);
        public static readonly Priority THIRD = new Priority(3);
        public static readonly Priority THIRTEENTH = new Priority(13);
        public static readonly Priority TWELFTH = new Priority(12);

        internal Priority(int priority)
        {
            this.priority = priority;
        }

        public override bool Equals(object obj)
        {
            Priority priority = obj as Priority;
            if (priority == null)
            {
                return false;
            }
            return (this.priority == priority.priority);
        }

        public override int GetHashCode()
        {
            return this.priority;
        }

        public static List<Node>[] NewEffectLists<Node>()
        {
            int num2;
            List<Node>[] listArray = new List<Node>[0x10];
            for (int i = 0; i < 0x10; i = num2)
            {
                listArray[i] = new List<Node>();
                num2 = i + 1;
            }
            return listArray;
        }

        public static bool operator ==(Priority p1, Priority p2)
        {
            return object.Equals(p1, p2);
        }

        public static bool operator >(Priority p1, Priority p2)
        {
            return (p1.priority > p2.priority);
        }

        public static implicit operator int(Priority p)
        {
            return p.priority;
        }

        public static implicit operator Priority(int p)
        {
            if ((p < 0) || (p >= 0x10))
            {
                throwIndexOutOfRangeException();
            }
            return PRIORITIES[p];
        }

        public static bool operator !=(Priority p1, Priority p2)
        {
            return !object.Equals(p1, p2);
        }

        public static bool operator <(Priority p1, Priority p2)
        {
            return (p1.priority < p2.priority);
        }

        private static void throwIndexOutOfRangeException()
        {
            IndexOutOfRange.Throw("Priority should between 0 ~ 16");
        }

        public override string ToString()
        {
            return string.Format("[EffectID: Priority={0}]", this.priority);
        }
    }
}

