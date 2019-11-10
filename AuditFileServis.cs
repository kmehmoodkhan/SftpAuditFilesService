using SFPTClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SftpAuditFilesService
{
    public partial class AuditFileServis : ServiceBase
    {
        private System.Timers.Timer timer = new System.Timers.Timer();
        public AuditFileServis()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Audit File Service";
                eventLog.WriteEntry("Audit File has started", EventLogEntryType.Information);
            }

            
            timer.Interval = (60 * 1000) * ApplicationSettings.TimerInterval;
            timer.AutoReset = true;
            timer.Elapsed += timer_Elapsed;            
            timer.Start();
            //ProcessFiles();
        }

        protected override void OnStop()
        {
            timer.Stop();
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Audit File Service";
                eventLog.WriteEntry("Audit File has stopped", EventLogEntryType.Information);
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ProcessFiles();
        }

        private void ProcessFiles()
        {
            string controlId = "";
            try
            {
                
                SftpPoll sftpPoll = new SftpPoll();
                var list = sftpPoll.GetLatestFiles();                

                var downloads = sftpPoll.DownloadLatestFiles(list);

                var files = sftpPoll.ExtractZipFiles(downloads);


                List<DBResult> dBResults = new List<DBResult>();

                foreach (var f in files)
                {
                    controlId = f.ControlId;
                    var result = sftpPoll.AddXmlData(f);

                    dBResults.Add(result);
                    controlId = "";
                }

                sftpPoll.SendEmail(dBResults.Count);

                string xml = sftpPoll.GetXmlOutput(dBResults);

                sftpPoll.UploadDocument(xml);
            }
            catch(Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Audit File Service";
                    eventLog.WriteEntry("Exception occured in file with ControlId="+ controlId+ "\n"+ex.StackTrace, EventLogEntryType.Error);
                }
            }
        }
    }
}
