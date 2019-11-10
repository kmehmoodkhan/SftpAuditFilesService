using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFPTClient
{
    public class FileInfo
    {
        public string FullName
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public string FileType
        {
            get;
            set;
        }

        public DateTime UploadDate
        {
            get;
            set;
        }
    }


    public class FileContent
    {
        public string XmlData
        {
            get;
            set;
        }

        public string PdfString
        {
            get;
            set;
        }
        public string ControlId
        {
            get;
            set;
        }
    }

    public class DBResult
    {
        public int VendorId
        {
            get;
            set;
        }

        public int ControlId
        {
            get;
            set;
        }
    }
}
