using System.Xml;
using System.Xml.Serialization;

[XmlRoot(ElementName = "TextureAtlas")]
public class TextureAtlasXml
{
    [XmlAttribute(AttributeName = "imagePath")]
    public string imagePath;

    [XmlElement(ElementName = "SubTexture")]
    public SubTextureXml[] frame;
}

[XmlRoot(ElementName = "SubTexture")]
public class SubTextureXml
{
    [XmlAttribute(AttributeName = "name")]
    public string name;
    [XmlAttribute(AttributeName = "x")]
    public int x;
    [XmlAttribute(AttributeName = "y")]
    public int y;
    [XmlAttribute(AttributeName = "width")]
    public int width;
    [XmlAttribute(AttributeName = "height")]
    public int height;
    [XmlAttribute(AttributeName = "frameX")]
    public int frameX;
    [XmlAttribute(AttributeName = "frameY")]
    public int frameY;
    [XmlAttribute(AttributeName = "frameWidth")]
    public int frameWidth;
    [XmlAttribute(AttributeName = "frameHeight")]
    public int frameHeight;
}
