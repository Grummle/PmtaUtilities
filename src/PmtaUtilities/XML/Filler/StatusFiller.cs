using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PmtaUtilities.XML.Filler
{
    public class StatusFiller
    {
        private XDocument _doc;
        private Status _status;

        public Status Extract(XDocument doc)
        {
            _doc = doc;
            _status = new Status();

            FillInfo();
            FillInbound();
            FillOutBound();
            FillSpool();
            FillResolver();
            FillConnections();
            FillQueue();


            return _status;
        }

        private void FillInfo()
        {
            _status.Info = new Status.MtaInfo();

            _status.Info.HostName = _doc.Descendants("mta")
                .Select(x => (string)x.Element("fullHostName"))
                .First();

            _status.Info.PmtaInfo = _doc.Descendants("product")
                .Select(x => new Status.ProductInfo
                                 {
                                     Name = (string)x.Element("name"),
                                     BuildDate = DateTime.Parse((string)x.Element("buildDate")),
                                     Version = (string)x.Element("version")
                                 })
                .First();

            _status.Info.OsInfo = _doc.Descendants("os")
                .Select(x => new Status.OsInfo
                                 {
                                     Build = (string)x.Element("build"),
                                     Name = (string)x.Element("name"),
                                     Version = (string)x.Element("version")
                                 })
                .First();

            _status.Info.HardwareInfo = _doc.Descendants("mta")
                .Select(x => new Status.HardwareInfo
                                 {
                                     CpuCount = x.Descendants("cpu").Select(y => Int32.Parse((string)y.Element("count"))).First(),
                                     CpuType = x.Descendants("cpu").Select(y => (string)y.Element("type")).First(),
                                     Ram = x.Descendants("ram").Select(y => Int32.Parse((string)y.Element("real"))).First()
                                 })
                .First();

        }
        private void FillInbound()
        {
            _status.InboundTraffic = new Status.SMTPTraffic();
            ExtractTraffic(_status.InboundTraffic, "in");
        }
        private void FillOutBound()
        {
            _status.OutboundTraffic = new Status.SMTPTraffic();
            ExtractTraffic(_status.OutboundTraffic, "out");
            
        }
        private void FillSpool()
        {
            _status.Spool = _doc.Descendants("spool")
                     .Select(x => new Status.SpoolInfo
                     {
                         PercentInitialized = int.Parse((string)x.Element("initPct")),
                         DirectoryCount = int.Parse((string)x.Element("dirs")),
                         FileInfo = x.Descendants("files").Select(y=>new Status.FileInfo
                                                                         {
                                                                            InUse = int.Parse((string) y.Element("inUse")),
                                                                            Recycled = int.Parse((string)y.Element("recycled")),
                                                                            Total = int.Parse((string)y.Element("total"))
                                                                         }).First()
                     })
                     .First();    
        }
        private void FillResolver()
        {
            _status.Dns = new Status.DnsInfo
                              {
                                  Cached = _doc.Descendants("resolver")
                                           .Select(x => int.Parse((string) x.Element("namesCached"))).First(),
                                  Pending = _doc.Descendants("resolver")
                                            .Select(x => int.Parse((string)x.Element("queriesPending"))).First()
                              };
        }
        private void FillConnections()
        {
            _status.InboundConnection = ExtractConnectionInfo("smtpIn");
            _status.OutboundConnection = ExtractConnectionInfo("smtpOut");
        }
        private void FillQueue()
        {
            _status.Queue = new Status.QueueInfo
                                {
                                    Smtp = ExtractQueueInfo("smtp"),
                                    File = ExtractQueueInfo("file"),
                                    Pipe = ExtractQueueInfo("file"),
                                    Discard = ExtractQueueInfo("discard"),
                                    GmImprinter = ExtractQueueInfo("gmImprinter"),
                                    Alias = ExtractQueueInfo("alias")
                                };
        }

        private Status.ConnectionInfo ExtractConnectionInfo(string direction)
        {
            return new Status.ConnectionInfo
            {
                Current = _doc.Descendants("conn")
                              .Descendants(direction)
                              .Select(x => int.Parse((string)x.Element("cur"))).First(),
                Max = _doc.Descendants("conn")
                          .Descendants(direction)
                          .Select(x => int.Parse((string)x.Element("max"))).First(),
                Top = _doc.Descendants("conn")
                          .Descendants(direction)
                          .Select(x => int.Parse((string)x.Element("top"))).First(),
            };    
        }
        private void ExtractTraffic(Status.SMTPTraffic traffic,string direction)
        {
            traffic.Total = _doc.Descendants("traffic")
              .Descendants("total")
              .Descendants(direction)
              .Select(x => new Status.SMTPMetric
              {
                  Recipients = int.Parse((string)x.Element("rcp")),
                  KB = double.Parse((string)x.Element("kb")),
                  Messages = int.Parse((string)x.Element("msg"))
              })
                               .First();

            traffic.LastHour = _doc.Descendants("traffic")
            .Descendants("lastHr")
            .Descendants(direction)
            .Select(x => new Status.SMTPMetric
            {
                Recipients = int.Parse((string)x.Element("rcp")),
                KB = double.Parse((string)x.Element("kb")),
                Messages = int.Parse((string)x.Element("msg"))
            })
                                 .First();

            traffic.LastMinute = _doc.Descendants("traffic")
            .Descendants("lastMin")
            .Descendants(direction)
            .Select(x => new Status.SMTPMetric
            {
                Recipients = int.Parse((string)x.Element("rcp")),
                KB = double.Parse((string)x.Element("kb")),
                Messages = int.Parse((string)x.Element("msg"))
            })
                                     .First();

            traffic.TopHour = _doc.Descendants("traffic")
            .Descendants("topPerHr")
            .Descendants(direction)
            .Select(x => new Status.SMTPMetric
            {
                Recipients = int.Parse((string)x.Element("rcp")),
                KB = double.Parse((string)x.Element("kb")),
                Messages = int.Parse((string)x.Element("msg"))
            })
                                     .First();

            traffic.TopMinute = _doc.Descendants("traffic")
            .Descendants("topPerMin")
            .Descendants(direction)
            .Select(x => new Status.SMTPMetric
            {
                Recipients = int.Parse((string)x.Element("rcp")),
                KB = double.Parse((string)x.Element("kb")),
                Messages = int.Parse((string)x.Element("msg"))
            })
                                     .First();
        }
        private Status.QueueMetric ExtractQueueInfo(string section)
        {
            return _doc.Descendants("queue")
                        .Descendants(section)
                        .Select(x => new Status.QueueMetric
                                         {
                                             Domains = int.Parse((string) x.Element("dom")),
                                             KB = double.Parse((string)x.Element("kb")),
                                             Recipients = int.Parse((string) x.Element("rcp"))
                                         }).First();    
        }
        

    }
}
