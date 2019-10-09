using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

public static class XmlTools
{

    public static void SerializeToXml(Attributes list, string Name)
    {
        Debug.Log("dad ");
        XmlSerializer serializer = new XmlSerializer(typeof(Attributes));
        FileStream stream = new FileStream("Assets/UnitAttirbutes/" + Name + ".txt", FileMode.Create);
        serializer.Serialize(stream, list);
        stream.Close();
    }

    public static void ParseXmlToObject(string path, ref Attributes attributes)
    {
        XmlSerializer xml = new XmlSerializer(typeof(Attributes));
        FileStream stream = new FileStream(path, FileMode.Open);
        attributes = xml.Deserialize(stream) as Attributes;
        stream.Close();
    }

    public static Attributes ParseXmlToObject(string path)
    {
        XmlSerializer xml = new XmlSerializer(typeof(Attributes));
        FileStream stream = new FileStream(path, FileMode.Open);
        Attributes attributes = xml.Deserialize(stream) as Attributes;
        stream.Close();
        return attributes;
    }
}
