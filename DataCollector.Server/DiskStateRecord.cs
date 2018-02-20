using NPoco;
using System;
using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;


namespace DataCollector.Server
{
    public class DiskStateRecord
    {
        public int Id { get; set; }
        [Column("localDateTime")]
        public DateTime DateTime { get; set; }
        public string MachineName { get; set; }
        public int Session { get; set; }
        public string DriveName { get; set; }
        public string DriveType { get; set; }
        public string VolumeLabel { get; set; }
        public string DriveFormat { get; set; }
        [Column("DriveTotalSize")]
        public double TotalSize { get; set; }
        [Column("DriveTotalFreeSize")]
        public double FreeSize { get; set; }

    }
}
