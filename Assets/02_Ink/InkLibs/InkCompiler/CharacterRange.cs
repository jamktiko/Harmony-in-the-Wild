using System.Collections.Generic;

namespace Ink
{
    /// <summary>
    /// A class representing a character range. Allows for lazy-loading a corresponding <see cref="CharacterSet">character set</see>.
    /// </summary>
    public sealed class CharacterRange
    {
        public static CharacterRange Define(char start, char end, IEnumerable<char> excludes = null)
        {
            return new CharacterRange(start, end, excludes);
        }

        /// <summary>
        /// Returns a <see cref="CharacterSet">character set</see> instance corresponding to the character range
        /// represented by the current instance.
        /// </summary>
        /// <remarks>
        /// The internal character set is created once and cached in memory.
        /// </remarks>
        /// <returns>The char set.</returns>
        public CharacterSet ToCharacterSet()
        {
            if (_correspondingCharSet.Count == 0)
            {
                for (char c = _start; c <= _end; c++)
                {
                    if (!_excludes.Contains(c))
                    {
                        _correspondingCharSet.Add(c);
                    }
                }
            }
            return _correspondingCharSet;
        }

        public char start { get { return _start; } }
        public char end { get { return _end; } }

        private CharacterRange(char start, char end, IEnumerable<char> excludes)
        {
            _start = start;
            _end = end;
            _excludes = excludes == null ? new HashSet<char>() : new HashSet<char>(excludes);
        }

        private char _start;
        private char _end;
        private ICollection<char> _excludes;
        private CharacterSet _correspondingCharSet = new CharacterSet();
    }
}
