using System;
using System.Drawing;
using FocusTracker.Native;

// ReSharper disable once CheckNamespace
namespace FocusDog.Native
{
    /// <summary>
    /// Wraps the native Windows MSG structure.
    /// </summary>
    public struct Message
    {
        private IntPtr windowHandle;
        private uint msg;
        private IntPtr wparam;
        private IntPtr lparam;
        private int time;
        private Point point;

        /// <summary>
        /// Gets the window handle
        /// </summary>
        public IntPtr WindowHandle => windowHandle;

        /// <summary>
        /// Gets the window message
        /// </summary>
        public uint Msg => msg;

        public MessageDefinitions MessageKind => (MessageDefinitions)msg;

        /// <summary>
        /// Gets the WParam
        /// </summary>
        public IntPtr WParam => wparam;

        /// <summary>
        /// Gets the LParam
        /// </summary>
        public IntPtr LParam => lparam;

        /// <summary>
        /// Gets the time
        /// </summary>
        public int Time => time;

        /// <summary>
        /// Gets the point
        /// </summary>
        public Point Point => point;

        /// <summary>
        /// Determines if two messages are equal.
        /// </summary>
        /// <param name="first">First message</param>
        /// <param name="second">Second message</param>
        /// <returns>True if first and second message are equal; false otherwise.</returns>
        public static bool operator ==(Message first, Message second)
        {
            return first.WindowHandle == second.WindowHandle && first.Msg == second.Msg && first.WParam == second.WParam && first.LParam == second.LParam && first.Time == second.Time && Equals(first.Point, second.Point);
        }

        /// <summary>
        /// Determines if two messages are not equal.
        /// </summary>
        /// <param name="first">First message</param>
        /// <param name="second">Second message</param>
        /// <returns>True if first and second message are not equal; false otherwise.</returns>
        public static bool operator !=(Message first, Message second)
        {
            return !(first == second);
        }

        /// <summary>
        /// Determines if this message is equal to another.
        /// </summary>
        /// <param name="obj">Another message</param>
        /// <returns>True if this message is equal argument; false otherwise.</returns>
        public override bool Equals(object obj)
        {
            return obj is Message message && this == message;
        }

        /// <summary>
        /// Gets a hash code for the message.
        /// </summary>
        /// <returns>Hash code for this message.</returns>
        public override int GetHashCode()
        {
            var hash = WindowHandle.GetHashCode();
            hash = hash * 31 + Msg.GetHashCode();
            hash = hash * 31 + WParam.GetHashCode();
            hash = hash * 31 + LParam.GetHashCode();
            hash = hash * 31 + Time.GetHashCode();
            hash = hash * 31 + Point.GetHashCode();
            return hash;
        }
    }
}