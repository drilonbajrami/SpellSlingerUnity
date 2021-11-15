using System;

// Retreived from Stefan Sedich's Blog : https://weblogs.asp.net/stefansedich/enum-with-string-values-in-c

namespace SpellSlinger
{
    public class StringValueAttribute : Attribute
    {
        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        /// <summary>
        /// Constructor used to initalize a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value) => this.StringValue = value;
    }
}
