// Copyright (c) Alexander Bocharov. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace AB.Xml;

/// <summary>
/// Represents a serializable dictionary.
/// </summary>
/// <seealso cref="IXmlSerializable" />
/// <remarks>
/// This class is used to serialize and deserialize dictionaries to and from XML.
/// </remarks>
public class SerializableDictionary : Dictionary<string, string?>, IXmlSerializable
{
    /// <inheritdoc />
    public XmlSchema? GetSchema() => null;

    /// <inheritdoc />
    public void ReadXml(XmlReader reader)
    {
        bool wasEmpty = reader.IsEmptyElement;
        reader.Read();

        if (wasEmpty)
        {
            return;
        }

        while (reader.NodeType != XmlNodeType.EndElement)
        {
            if (reader.NodeType != XmlNodeType.Element)
            {
                reader.Read();
                continue;
            }

            string key = reader.Name;
            string? value = reader.ReadElementContentAsString();
            Add(key, value);
        }

        reader.ReadEndElement();
    }

    /// <inheritdoc />
    public void WriteXml(XmlWriter writer)
    {
        foreach (var kvp in this.Where(kvp => kvp.Value is not null))
        {
            writer.WriteElementString(kvp.Key, kvp.Value);
        }
    }
}
