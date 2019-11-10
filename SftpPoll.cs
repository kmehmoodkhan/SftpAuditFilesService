using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SFPTClient
{
    public class SftpPoll : ISftpPoll
    {
        public string[] GetLatestFiles()
        {
            try
            {
                var files = Utility.ListFiles();
                var lastPollTime = Utility.GetLastPollTime();
                
                List<string> newFiles = new List<string>();

                foreach (FileInfo file in files)
                {
                    if( DateTime.Compare(file.UploadDate, lastPollTime) >0 && file.FileType == ".zip")
                    {
                        newFiles.Add(file.FullName);
                    }
                }

                if (newFiles.Count > 0)
                {
                    return newFiles.ToArray();
                }
                return null;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string[] DownloadLatestFiles(string[] files)
        {
            List<string> filesDownloaded = new List<string>();

            if (!Directory.Exists(ApplicationSettings.DownloadDirectory))
            {
                Directory.CreateDirectory(ApplicationSettings.DownloadDirectory);
            }

            if (files!=null && files.Count() > 0)
            {
                try
                {
                    foreach( var f in files)
                    {
                        var tempPath = ApplicationSettings.DownloadDirectory;
                        if (!tempPath.EndsWith(@"\"))
                        {
                            tempPath += "\\";
                        }
                        string downloadPath = tempPath + Path.GetFileName(f);
                        downloadPath = Utility.DownloadFile(f, downloadPath);

                        if (!string.IsNullOrEmpty(downloadPath))
                        {
                            filesDownloaded.Add(downloadPath);
                        }
                    }
                    /*
                    Parallel.ForEach(files, f =>
                    {
                        var tempPath = ApplicationSettings.DownloadDirectory;
                        if (!tempPath.EndsWith(@"\"))
                        {
                            tempPath += "\\";
                        }
                        string downloadPath = tempPath + Path.GetFileName(f);
                        downloadPath = Utility.DownloadFile(f, downloadPath);

                        if (!string.IsNullOrEmpty(downloadPath))
                        {
                            filesDownloaded.Add(downloadPath);
                        }
                    });*/
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return filesDownloaded.ToArray();
        }

        public FileContent[] ExtractZipFiles(string[] files)
        {
            List<FileContent> extracts = new List<FileContent>();
            try
            {
                foreach( var f in files)
                {
                    var fileContent = Utility.ExtractZipFile(f);

                    extracts.Add(fileContent);
                }
                //Parallel.ForEach(files, f =>
                // {
                //     var fileContent= Utility.ExtractZipFile(f);

                //     extracts.Add(fileContent);
                // });
            }
            catch(Exception ex)
            {
                throw (ex);
            }
            return extracts.ToArray();
        }

        public string GetXmlOutput(List<DBResult> dBResults)
        {
            string xmlDocument = string.Empty;
            string docPath = string.Empty;

            if (dBResults.Count > 0)
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                XmlElement root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, "Reconciliation", string.Empty);
                doc.AppendChild(element1);

                foreach (var result in dBResults)
                {
                    XmlElement element2 = doc.CreateElement(string.Empty, "Audit", string.Empty);
                    element1.AppendChild(element2);

                    XmlElement element3 = doc.CreateElement(string.Empty, "ControlId", string.Empty);
                    XmlText text1 = doc.CreateTextNode(result.ControlId.ToString());
                    element3.AppendChild(text1);
                    element2.AppendChild(element3);

                    XmlElement element4 = doc.CreateElement(string.Empty, "VendorId", string.Empty);
                    XmlText text2 = doc.CreateTextNode(result.VendorId.ToString());
                    element4.AppendChild(text2);
                    element2.AppendChild(element4);
                }

                xmlDocument = doc.OuterXml;

                docPath= Utility.WriteXmlFile(doc);
            }


            return docPath;
        }

        public void SendEmail(int recordsProcessed)
        {
            if (recordsProcessed > 0)
            {
                string subject = "Nexus Audit Order";
                string emailBody = @"<b>"+recordsProcessed.ToString()+"</b>"+ @" files are processed. <br\><br\> Thank you.";

                Utility.SendEmail(subject, emailBody);
            }
        }

        public DBResult AddXmlData(FileContent xmlData)
        {
            DataAccess dataAccess = new DataAccess();
            var result = dataAccess.ExecuteNonQuery(xmlData);
            return result;
        }

        public bool UploadDocument(string docPath)
        {
            return Utility.UploadFile(docPath);
        }
    }
}
