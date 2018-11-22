using UnityEngine;

namespace Kadoku.Scripting.Actions
{
    public struct Socket
    {
        public Vector2 location;
        public bool input;

        public Socket(Vector2 location, bool input)
        {
            this.location = location;
            this.input = input;
        }

        public Socket(float x, float y, bool input) : this(new Vector2(x, y), input) { }
    }
}