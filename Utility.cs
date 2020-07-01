using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WinSCP;
using System.ComponentModel;
using System.Net.Mail;
using System.Xml.Linq;
using System.Xml;

namespace SFPTClient
{
    //public static class Utility
    //{
    //    private static string PollFilePath
    //    {
    //        get
    //        {
    //            var appPath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
    //            appPath += "\\lastpoll.txt";
    //            return appPath;
    //        }
    //    }
    //    public static string FtpUrl
    //    {
    //        get
    //        {
    //            string url = $"ftp://{ApplicationSettings.SFTPHost + ":" + ApplicationSettings.SFTPDefaultPort + "/" + ApplicationSettings.SFTPDirPath}";
    //            return url;
    //        }
    //    }
    //    public static void SavePollTime()
    //    {
    //        System.IO.File.WriteAllText(PollFilePath, DateTime.Now.ToString());
    //    }
    //    public static DateTime GetLastPollTime()
    //    {
    //        DateTime lastPollTime = DateTime.MinValue;


    //        string fileContent = string.Empty;

    //        if (File.Exists(PollFilePath))
    //        {
    //            fileContent = System.IO.File.ReadAllText(PollFilePath);
    //        }

    //        if (!string.IsNullOrEmpty(fileContent))
    //        {
    //            lastPollTime = Convert.ToDateTime(fileContent);
    //        }

    //        Utility.SavePollTime();

    //        return lastPollTime;
    //    }
    //    public static List<FileInfo> ListFiles()
    //    {
    //        List<FileInfo> filesList = null;
    //        try
    //        {
    //            SessionOptions sessionOptions = new SessionOptions
    //            {
    //                Protocol = Protocol.Sftp,
    //                HostName = ApplicationSettings.SFTPHost,
    //                UserName = ApplicationSettings.SFTPUsername,
    //                Password = ApplicationSettings.SFTPUserPassword,
    //                GiveUpSecurityAndAcceptAnySshHostKey = true
    //            };

    //            using (Session session = new Session())
    //            {
    //                session.Open(sessionOptions);

    //                var result = session.ListDirectory(ApplicationSettings.SFTPDirPath);
    //                var fileCollection = result.Files;
    //                filesList = fileCollection.Select(t => new FileInfo() { FullName = t.FullName, FileName = t.Name, FileType = Path.GetExtension(t.Name), UploadDate = t.LastWriteTime }).ToList();
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            throw (e);
    //        }
    //        return filesList;
    //    }
    //    public static string DownloadFile(string filePath, string downloadPath)
    //    {
    //        SessionOptions sessionOptions = new SessionOptions
    //        {
    //            Protocol = Protocol.Sftp,
    //            HostName = ApplicationSettings.SFTPHost,
    //            UserName = ApplicationSettings.SFTPUsername,
    //            Password = ApplicationSettings.SFTPUserPassword,
    //            GiveUpSecurityAndAcceptAnySshHostKey = true
    //        };


    //        TransferOptions transferOptions = new TransferOptions();
    //        transferOptions.TransferMode = TransferMode.Binary;


    //        TransferOperationResult transferResult = null;

    //        try
    //        {
    //            using (Session session = new Session())
    //            {
    //                session.Open(sessionOptions);

    //                transferResult = session.GetFiles(filePath, downloadPath, false, transferOptions);

    //                if (transferResult == null)
    //                {
    //                    downloadPath = string.Empty;
    //                }
    //                else if (transferResult != null && !transferResult.IsSuccess)
    //                {
    //                    downloadPath = string.Empty;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        return downloadPath;
    //    }

    //    public static bool UploadFile(string filePath)
    //    {
    //        SessionOptions sessionOptions = new SessionOptions
    //        {
    //            Protocol = Protocol.Sftp,
    //            HostName = ApplicationSettings.SFTPHost,
    //            UserName = ApplicationSettings.SFTPUsername,
    //            Password = ApplicationSettings.SFTPUserPassword,
    //            GiveUpSecurityAndAcceptAnySshHostKey = true
    //        };


    //        TransferOptions transferOptions = new TransferOptions();
    //        transferOptions.TransferMode = TransferMode.Binary;


    //        TransferOperationResult transferResult = null;

    //        try
    //        {
    //            using (Session session = new Session())
    //            {
    //                session.Open(sessionOptions);

    //                transferResult = session.PutFiles(filePath, $"/home/{ApplicationSettings.SFTPUsername}/{ApplicationSettings.SFTUploadDirPath}/", false, transferOptions);

    //                transferResult.Check();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        return transferResult.IsSuccess;
    //    }

    //    public static string RemoveSpecialCharacters(this string str)
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        foreach (char c in str)
    //        {
    //            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
    //            {
    //                sb.Append(c);
    //            }
    //        }
    //        return sb.ToString();
    //    }
    //    public static FileContent ExtractZipFile(string filePath)
    //    {
    //        FileContent fileContent = null;
    //        string pdfPath = ApplicationSettings.DownloadDirectory;

    //        if (!pdfPath.EndsWith("\\"))
    //        {
    //            pdfPath += "\\";
    //        }

    //        pdfPath += "PDF\\";

    //        if (!Directory.Exists(pdfPath))
    //        {
    //            Directory.CreateDirectory(pdfPath);
    //        }

    //        try
    //        {

    //            fileContent = new FileContent();

    //            using (ZipArchive archive = ZipFile.Open(filePath, ZipArchiveMode.Update))
    //            {
    //                string insuredName = "";
    //                string controlID = "";

    //                foreach (var entry in archive.Entries)
    //                {

    //                    if (Path.GetExtension(entry.Name).ToLower().Equals(".xml"))
    //                    {
    //                        var sr = new StreamReader(entry.Open());
    //                        fileContent.XmlData = sr.ReadToEnd();

    //                        XDocument xdoc = XDocument.Parse(fileContent.XmlData);
    //                        xdoc.Declaration = null;
    //                        fileContent.XmlData = xdoc.ToString();

    //                        XmlDocument doc = new XmlDocument();
    //                        doc.LoadXml(fileContent.XmlData);

    //                        if (doc.SelectSingleNode("Audit/InsuredName").HasChildNodes)
    //                        {
    //                            insuredName = doc.SelectSingleNode("Audit/InsuredName").InnerText;
    //                            insuredName = RemoveSpecialCharacters(insuredName);
    //                        }

    //                        if (doc.SelectSingleNode("Audit/ControlID").HasChildNodes)
    //                        {
    //                            controlID = doc.SelectSingleNode("Audit/ControlID").InnerText;
    //                            fileContent.ControlId = controlID;
    //                        }
    //                    }
    //                    else if (Path.GetExtension(entry.Name).ToLower().Equals(".pdf"))
    //                    {
    //                        byte[] bytes;
    //                        var stream = entry.Open();
    //                        var ms = new MemoryStream();
    //                        {
    //                            stream.CopyTo(ms);
    //                            bytes = ms.ToArray();
    //                        }
    //                        fileContent.PdfString = Convert.ToBase64String(bytes);
    //                    }



    //                    bool writePdf = false;
    //                    bool pathSet = false;
    //                    if (!string.IsNullOrEmpty(insuredName) && !pathSet)
    //                    {
    //                        pdfPath += controlID+"_"+insuredName + "_"+ $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
    //                        writePdf = true;
    //                        pathSet = true;
    //                    }

    //                    if (writePdf && !string.IsNullOrEmpty(fileContent.PdfString))
    //                    {
    //                        File.WriteAllBytes(pdfPath, Convert.FromBase64String(fileContent.PdfString));
    //                        writePdf = false;
    //                        pdfPath = string.Empty;
    //                        controlID = string.Empty;
    //                    }
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        return fileContent;
    //    }

    //    #region Extract ZipFile
    //    //public static FileContent ExtractZipFile(string filePath)
    //    //{
    //    //    FileContent fileContent = null;
    //    //    try
    //    //    {

    //    //        fileContent = new FileContent();

    //    //        using (ZipArchive archive = ZipFile.Open(filePath, ZipArchiveMode.Update))
    //    //        {
    //    //            foreach (var entry in archive.Entries)
    //    //            {
    //    //                string insuredName = "";
    //    //                if (Path.GetExtension(entry.Name).ToLower().Equals(".xml"))
    //    //                {
    //    //                    var sr = new StreamReader(entry.Open());
    //    //                    fileContent.XmlData = sr.ReadToEnd();

    //    //                    XDocument xdoc = XDocument.Parse(fileContent.XmlData);
    //    //                    xdoc.Declaration = null;
    //    //                    fileContent.XmlData = xdoc.ToString();

    //    //                    XmlDocument doc = new XmlDocument();
    //    //                    doc.LoadXml(fileContent.XmlData);

    //    //                    if (doc.SelectSingleNode("Audit/InsuredName").HasChildNodes)
    //    //                    {
    //    //                        insuredName = doc.SelectSingleNode("Audit/InsuredName").InnerText;
    //    //                        insuredName = RemoveSpecialCharacters(insuredName);
    //    //                    }
    //    //                }
    //    //                else if (Path.GetExtension(entry.Name).ToLower().Equals(".pdf"))
    //    //                {
    //    //                    string pdfPath = ApplicationSettings.DownloadDirectory;

    //    //                    if(!pdfPath.EndsWith("\\"))
    //    //                    {
    //    //                        pdfPath += "\\";
    //    //                    }

    //    //                    pdfPath += "PDF\\";

    //    //                    if(!Directory.Exists(pdfPath))
    //    //                    {
    //    //                        Directory.CreateDirectory(pdfPath);
    //    //                    }

    //    //                    bool writePdf = false;
    //    //                    if (!string.IsNullOrEmpty(insuredName))
    //    //                    {
    //    //                        pdfPath += insuredName + ".pdf";
    //    //                        writePdf = true;
    //    //                    }

    //    //                    byte[] bytes;
    //    //                    var stream = entry.Open();
    //    //                    using (var ms = new MemoryStream())
    //    //                    {
    //    //                        stream.CopyTo(ms);
    //    //                        bytes = ms.ToArray();

    //    //                        if (writePdf)
    //    //                        {
    //    //                            using (FileStream file = new FileStream(pdfPath, FileMode.Open, FileAccess.Read))
    //    //                                file.CopyTo(ms);
    //    //                        }
    //    //                    }


    //    //                    fileContent.PdfString = Convert.ToBase64String(bytes);
    //    //                }
    //    //            }
    //    //        }
    //    //    }
    //    //    catch (Exception ex)
    //    //    {
    //    //        ;
    //    //    }
    //    //    return fileContent;
    //    //} 
    //    #endregion


    //    private static SmtpClient GetSmtpClient()
    //    {
    //        SmtpClient emailClient = new SmtpClient();

    //        emailClient.Host = ApplicationSettings.SmtpServer;
    //        emailClient.EnableSsl = true;
    //        emailClient.DeliveryMethod = SmtpDeliveryMethod.Network;


    //        int port = ApplicationSettings.SmtpPort;

    //        if (port > 0)
    //        {
    //            emailClient.Port = port;
    //        }

    //        if (!String.IsNullOrEmpty(ApplicationSettings.SmtpUsername))
    //        {
    //            emailClient.UseDefaultCredentials = false;
    //            emailClient.Credentials = new System.Net.NetworkCredential(ApplicationSettings.SmtpUsername, ApplicationSettings.SmtpPassword, ApplicationSettings.SmtpServer);
    //        }
    //        else
    //        {
    //            emailClient.UseDefaultCredentials = true;
    //        }

    //        return emailClient;
    //    }

    //    public static void SendEmail(string subject, string emailBody)
    //    {
    //        System.Net.Mail.MailMessage mail = new MailMessage();
    //        mail.From = new MailAddress(ApplicationSettings.FromEmailAddress);

    //        mail.To.Add(ApplicationSettings.ReceiverEmail);

    //        mail.Priority = MailPriority.Normal;

    //        mail.IsBodyHtml = true;

    //        mail.Subject = subject;

    //        string emailTemplateBody = emailBody;
    //        mail.Body = emailTemplateBody;

    //        SmtpClient emailClient = GetSmtpClient();

    //        emailClient.Send(mail);
    //    }

    //    public static string WriteXmlFile(XmlDocument xmlDocument)
    //    {
    //        string fileName = $"Reconciliation_{DateTime.Now.ToString("yyyyMMddhhmmss")}.xml";
    //        string filePath = ApplicationSettings.OutputDirectory;

    //        if (!Directory.Exists(ApplicationSettings.OutputDirectory))
    //        {
    //            Directory.CreateDirectory(ApplicationSettings.OutputDirectory);
    //        }

    //        if (!filePath.EndsWith(@"\"))
    //        {
    //            filePath+=@"\";
    //        }

    //        filePath += fileName;
    //        xmlDocument.Save(filePath);
    //        return filePath;
    //    }
    //}
}


