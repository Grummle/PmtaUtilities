using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PmtaUtilities.XML
{
    public class Status
    {
        public MtaInfo Info { get; set; }
        public SpoolInfo Spool { get; set; }
        public DnsInfo Dns { get; set; }

        public SMTPTraffic InboundTraffic {get;set;}
        public SMTPTraffic OutboundTraffic { get; set;}

        public ConnectionInfo InboundConnection { get; set; }
        public ConnectionInfo OutboundConnection { get; set; }

        public QueueInfo Queue { get; set; }


        public class SMTPTraffic
        {
            public SMTPMetric LastHour { get; set; }
            public SMTPMetric TopHour { get; set; }
            public SMTPMetric LastMinute { get; set; }
            public SMTPMetric TopMinute { get; set; }    
            public SMTPMetric Total { get; set; }
        }
        public class SMTPMetric
        {
            public int Recipients { get; set; }
            public int Messages { get; set; }
            public double KB { get; set; }
        }
        public class QueueMetric
        {
            public int Recipients { get; set; }
            public int Domains { get; set; }
            public double KB { get; set; }    
        }
        public class ProductInfo
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public DateTime BuildDate { get; set; }
        }
        public class OsInfo
        {
            public string Name { get; set; }
            public string Version { get; set; }
            public string Build { get; set; }
        }
        public class HardwareInfo
        {
            public string CpuType { get; set; }
            public int CpuCount { get; set; }
            public int Ram { get; set; }
        }
        public class MtaInfo
        {
            public string HostName { get; set; }
            public ProductInfo PmtaInfo {get;set;}
            public OsInfo OsInfo { get; set; }
            public HardwareInfo HardwareInfo { get; set; }
        }
        public class SpoolInfo
        {
            public int PercentInitialized { get; set; }
            public int DirectoryCount { get; set; }
            public FileInfo FileInfo  {get;set;}
        }
        public class FileInfo
        {
            public int InUse { get; set; }
            public int Recycled { get; set; }
            public int Total { get; set; }
        }
        public class DnsInfo
        {
            public int Cached { get; set; }
            public int Pending { get; set; }
        }
        public class ConnectionInfo
        {
            public int Current { get; set; }
            public int Max {get;set;}
            public int Top { get; set; }
        }
        public class QueueInfo
        {
            public QueueMetric Smtp { get; set; }
            public QueueMetric Discard { get; set; }
            public QueueMetric File { get; set; }
            public QueueMetric Pipe { get; set; }
            public QueueMetric GmImprinter { get; set; }
            public QueueMetric Alias { get; set; }
        }
    }
}
