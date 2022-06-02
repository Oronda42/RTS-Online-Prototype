using System.Xml.Serialization;

namespace RTS.Server
{
    [XmlRoot("ServerCustomConfig")]
    public class ServerCustomConfig
    {
        [XmlElement("Log")]
        public LogConfig Log { get; set; }

        [XmlElement("Database")]
        public DatabaseConfig Database { get; set; }
    }
}
