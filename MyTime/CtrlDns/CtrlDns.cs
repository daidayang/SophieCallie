using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Timers;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO;

using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using Newtonsoft.Json;

using CtrlDns.models;

namespace CtrlDns
{
    public partial class CtrlDns : ServiceBase
    {
        // P/Invoke for GetDeviceCaps
        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private System.Timers.Timer timer = null;

        private string CtrlUrl = string.Empty;

        private string _XmlResponse = string.Empty;
        private bool DebugTimeTick;

        private MyDow ToDayOfWeek(DayOfWeek dow)
        {
            if (dow == DayOfWeek.Monday) return MyDow.Monday;
            if (dow == DayOfWeek.Tuesday) return MyDow.Tuesday;
            if (dow == DayOfWeek.Wednesday) return MyDow.Wednesday;
            if (dow == DayOfWeek.Thursday) return MyDow.Thursday;
            if (dow == DayOfWeek.Friday) return MyDow.Friday;
            if (dow == DayOfWeek.Saturday) return MyDow.Saturday;
            return MyDow.Sunday;
        }

        public CtrlDns()
        {
            InitializeComponent();

            DebugTimeTick = false;
            timer = new System.Timers.Timer(60000);
            timer.Elapsed += new ElapsedEventHandler(this.ServiceTimer_Tick);
        }

        public async void DebugStart()
        {
            CtrlUrl = ConfigurationManager.AppSettings["CtrlUrl"];

            // await PostRunningProcesses(CtrlUrl + "/PostTaskList");
            await CaptureScreenshotAsync(string.Empty, CtrlUrl + "/PostImage");

            //OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            //UsageControls uc = new UsageControls
            //{
            //    Controls = new List<UsageControl>                {
            //        new UsageControl {
            //            DateRanges = new List<DateRange>
            //            {
            //                new DateRange {
            //                    DOW = MyDow.Monday | MyDow.Tuesday | MyDow.Wednesday | MyDow.Thursday | MyDow.Friday,
            //                    TimeRanges = new List<TimeRange> {
            //                        new TimeRange {
            //                            StartHour = 1,
            //                            StartMin = 0,
            //                            EndHour = 17,
            //                            EndMin = 30
            //                        },
            //                        new TimeRange {
            //                            StartHour = 18,
            //                            StartMin = 30,
            //                            EndHour = 23,
            //                            EndMin = 30
            //                        }
            //                    }
            //                },
            //                new DateRange {
            //                    DOW = MyDow.Saturday | MyDow.Sunday,
            //                    TimeRanges = new List<TimeRange> {
            //                        new TimeRange {
            //                            StartHour = 1,
            //                            StartMin = 0,
            //                            EndHour = 15,
            //                            EndMin = 30
            //                        },
            //                        new TimeRange {
            //                            StartHour = 23,
            //                            StartMin = 30,
            //                            EndHour = 23,
            //                            EndMin = 59
            //                        }
            //                    }
            //                }
            //            },
            //            ControlItems = new List<ControlItem>
            //            {
            //                new ControlItem {
            //                    Identifier = "www.youtube.com",
            //                    Name = "YouTube",
            //                    Type = ControlItemType.WWW
            //                },
            //                new ControlItem {
            //                    Identifier = "www.roblox.com",
            //                    Name = "roblox www",
            //                    Type = ControlItemType.WWW
            //                },
            //                new ControlItem {
            //                    Identifier = "web.roblox.com",
            //                    Name = "roblox web",
            //                    Type = ControlItemType.WWW
            //                },
            //                new ControlItem {
            //                    Identifier = "robloxplayerbeta.exe",
            //                    Name = "roblox exec",
            //                    Type = ControlItemType.APP
            //                }
            //            }
            //        }
            //    }
            //};

            //string sTmp = JsonConvert.SerializeObject(uc);
            //UsageControls dr2 = JsonConvert.DeserializeObject<UsageControls>(sTmp);

            CtrlUrl = ConfigurationManager.AppSettings["CtrlUrl"];
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStop()
        {
            timer.AutoReset = false;
            timer.Enabled = false;
        }

        private async void ServiceTimer_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime TimeNow = DateTime.Now;
            if (DebugTimeTick && log.IsDebugEnabled)
                log.DebugFormat("Timer Tick: {0}", TimeNow);

            this.timer.Stop();

            try
            {
                int rc = Utils.HttpGet(CtrlUrl, out string HttpResponse);
                if (rc >= 0)
                    await ProcessServerResponse(HttpResponse);

                if (log.IsDebugEnabled)
                    log.DebugFormat("URL={0}, rc={1}", CtrlUrl, rc);
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Main loop exception.  Error={0}, StackTrace={1}", ex.Message, ex.StackTrace);
            }

            this.timer.Start();
        }

        private async Task ProcessServerResponse(string blockList)
        {
            UsageControls ucs = JsonConvert.DeserializeObject<UsageControls>(blockList);

            #region Process Usage Controls

            List<string> BlockedUrls = new List<string>();
            List<string> BlockedApps = new List<string>();

            foreach (UsageControl uc in ucs.Controls)
            {
                bool blocked = false;
                DateTime NOW = DateTime.Now;
                MyDow TodayDOW = ToDayOfWeek(DateTime.Today.DayOfWeek);

                foreach (DateRange dr in uc.DateRanges)
                {
                    if ((TodayDOW & dr.DOW) != TodayDOW)
                    {
                        log.DebugFormat("Today is {0}. Ignore entry {1}", TodayDOW, dr.DOW);
                        continue;
                    }

                    foreach (TimeRange tr in dr.TimeRanges)
                    {
                        DateTime BeginTime = DateTime.Today.AddHours(tr.StartHour).AddMinutes(tr.StartMin);
                        DateTime EndTime = DateTime.Today.AddHours(tr.EndHour).AddMinutes(tr.EndMin);
                        if (BeginTime <= NOW && NOW <= EndTime)
                        {
                            log.DebugFormat("Now is {0}. Enforce entry [{1}, {2}:{3} - {4}:{5}]", NOW, dr.DOW, tr.StartHour, tr.StartMin, tr.EndHour, tr.EndMin);
                            blocked = true;
                            break;
                        }
                        else
                        {
                            log.DebugFormat("Now is {0}. Ignore entry [{1}, {2}:{3} - {4}:{5}]", NOW, dr.DOW, tr.StartHour, tr.StartMin, tr.EndHour, tr.EndMin);
                        }
                    }
                }

                if (blocked)
                {
                    foreach (ControlItem ci in uc.ControlItems)
                    {

                        if (ci.Type == ControlItemType.WWW)
                        {
                            BlockedUrls.Add(ci.Identifier);
                        }
                        else
                        {
                            BlockedApps.Add(ci.Identifier);
                        }
                    }
                }
            }

            #region Modify hosts file to block URL mappings

            string[] OldHostsFileLines = System.IO.File.ReadAllLines(@"C:\Windows\System32\drivers\etc\hosts");
            List<string> lstLines = new List<string>();
            StringBuilder sb = new StringBuilder();
            foreach (string l in OldHostsFileLines)
            {
                bool keep = true;
                string[] ss = l.Split(' ');
                if (ss.Length >= 3)
                {
                    int TxtCnt = 1;
                    string Txt2 = null;
                    for (int idy = 0; idy < ss.Length; idy++)
                    {
                        if (string.IsNullOrWhiteSpace(ss[idy]))
                            continue;

                        if (TxtCnt == 2)
                            Txt2 = ss[idy];

                        if (ss[idy] == "#CtrlDns")
                        {
                            if (!BlockedUrls.Contains(Txt2))
                                keep = false;
                        }
                        TxtCnt++;
                    }

                    if (keep)
                    {
                        sb.AppendLine(l);
                        lstLines.Add(l);
                    }
                }
            }

            string FullText = sb.ToString();
            foreach (string l in BlockedUrls)
            {
                if (string.IsNullOrWhiteSpace(l))
                    continue;

                log.DebugFormat("Need to block {0}", l);

                if (FullText.IndexOf(l, StringComparison.OrdinalIgnoreCase) < 0)
                    lstLines.Add(string.Format("127.0.0.1  {0}  #CtrlDns", l));
            }

            System.IO.File.WriteAllLines(@"C:\Windows\System32\drivers\etc\hosts", lstLines);

            #endregion

            #region Kill apps that are blocked

            KillProcesses(BlockedApps, 1);

            #endregion

            #endregion

            #region Process MyTime Tasks

            foreach (MyTimeTaskItem uc in ucs.Tasks)
            {
                if (uc == null)
                    continue;

                #region Collect Windows Task lists

                if (uc.TaskName == "GetTaskList")
                    await PostRunningProcesses(CtrlUrl + "/PostTaskList");

                #endregion

                #region Take a screen shot

                if (uc.TaskName == "TakeScreenShot")
                    await CaptureScreenshotAsync(string.Empty, CtrlUrl + "/PostImage");

                #endregion
            }

            #endregion
        }

        private void KillProcesses(List<string> lstBlockedProcessNames, int delay)
        {
            if (lstBlockedProcessNames == null)
                return;

            foreach (string srtProcessName in lstBlockedProcessNames)
            {
                Process[] lstProcessByName = Process.GetProcessesByName(srtProcessName);

                log.DebugFormat("Process.GetProcessesByName('{0}') found {1} running process", srtProcessName, (lstProcessByName == null) ? 0 : lstProcessByName.Length);

                if (lstProcessByName != null && lstProcessByName.Length > 0)
                {
                    List<Process> PidToBeKilled = new List<Process>();

                    #region Get list of pid that are alive 30 min long

                    for (int idx = 0; idx < lstProcessByName.Length; idx++)
                    {
                        Process p = lstProcessByName[idx];
                        try
                        {
                            if (log.IsDebugEnabled)
                                log.DebugFormat("ProcessID:{0}, StartTime:{1} ", p.Id, p.StartTime);

                            if (DateTime.Now.Subtract(p.StartTime).TotalMinutes > delay)
                                PidToBeKilled.Add(p);
                        }
                        catch (Exception e)
                        {
                            log.ErrorFormat("Unable to get process exec time.  PID={0}.  Error={1}, StackTrace={2}", p.Id, e.Message, e.StackTrace);
                        }
                    }

                    #endregion

                    #region It is time to kill this process

                    foreach (Process p in PidToBeKilled)
                    {
                        if (log.IsDebugEnabled)
                            log.DebugFormat("Kill orphaned {1} process.  ID={0}", p.Id, p.ProcessName);

                        try
                        {
                            p.Kill();
                        }
                        catch (Exception ex1)
                        {
                            log.ErrorFormat("CleanOrphanPhantomJS() exception.  Error={0}, StackTrace={1}", ex1.Message, ex1.StackTrace);
                        }
                    }

                    #endregion
                }
            }
        }

        private async Task PostRunningProcesses(string url)
        {
            List<WindowsTaskItem> ret = new List<WindowsTaskItem>();

            Process[] lstProcessByName = Process.GetProcesses();

            if (lstProcessByName != null && lstProcessByName.Length > 0)
            {
                for (int idx = 0; idx < lstProcessByName.Length; idx++)
                {
                    Process p = lstProcessByName[idx];

                    if (p.ProcessName == "svchost")
                        continue;

                    WindowsTaskItem wp = new WindowsTaskItem();
                    wp.TaskName = p.ProcessName;
                    wp.PID = p.Id;
                    //wp.ExePath = p.MainModule.FileName;
                    wp.Status = p.Responding ? "Running" : "Not Responding";
                    ret.Add(wp);
                }
            }

            if (ret.Count > 0)
            {
                string sTmp = JsonConvert.SerializeObject(ret);
                await PostJsonAsync(sTmp, url);
            }
        }

        private async Task CaptureScreenshotAsync(string filePath, string url)
        {
            int screenID = 0;
            try
            {
                foreach (var screen in Screen.AllScreens)
                {
                    // Get the bounds of the screen
                    Rectangle bounds = screen.Bounds;

                    // Use Graphics to get the actual DPI settings
                    using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
                    {
                        IntPtr hdc = g.GetHdc();
                        int dpiX = GetDeviceCaps(hdc, 88); // LOGPIXELSX
                        int dpiY = GetDeviceCaps(hdc, 90); // LOGPIXELSY

                        //  https://pinvoke.net/default.aspx/gdi32/GetDeviceCaps.html
                        //  https://stackoverflow.com/questions/5082610/get-and-set-screen-resolution
                        /// <summary>
                        /// Vertical height of entire desktop in pixels
                        /// </summary>
                        //  DESKTOPVERTRES = 117,
                        /// <summary>
                        /// Horizontal width of entire desktop in pixels
                        /// </summary>
                        //  DESKTOPHORZRES = 118,
                        int LogicalScreenHeight = GetDeviceCaps(hdc, 8);
                        int PhysicalScreenHeight = GetDeviceCaps(hdc, 10);
                        int DesktopHeight = GetDeviceCaps(hdc, 117);
                        int DesktopWidth = GetDeviceCaps(hdc, 118);

                        g.ReleaseHdc(hdc);

                        // Calculate the actual bounds without scaling
                        int width = (int)(bounds.Width * 96 / dpiX);
                        int height = (int)(bounds.Height * 96 / dpiY);

                        // Create a bitmap with the actual dimensions
                        using (Bitmap bitmap = new Bitmap(DesktopWidth, DesktopHeight))
                        {
                            // Create a graphics object from the bitmap
                            using (Graphics graphics = Graphics.FromImage(bitmap))
                            {
                                // Copy the screen to the bitmap
                                graphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, new Size(DesktopWidth, DesktopHeight), CopyPixelOperation.SourceCopy);
                            }

                            // Post the bitmap to the remote URL
                            await PostImageAsync(screenID++, bitmap, url);

                            // Save the bitmap to a file
                            if (!string.IsNullOrWhiteSpace(filePath))
                            {
                                // Create a unique file path for each screen
                                string screenFilePath = Path.Combine(Path.GetDirectoryName(filePath), $"{Path.GetFileNameWithoutExtension(filePath)}_{screen.DeviceName.Replace("\\", "").Replace(".", "")}{Path.GetExtension(filePath)}");
                                bitmap.Save(screenFilePath, ImageFormat.Png);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("An error occurred while capturing the screenshot: {0}", ex.Message);
            }
        }

        private async Task PostImageAsync(int screenID, Bitmap bitmap, string url)
        {
            // Extract the host from the URL
            Uri uri = new Uri(url);
            string host = uri.Host;

            using (HttpClient client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Convert the bitmap to a memory stream
                    using (var memoryStream = new MemoryStream())
                    {
                        bitmap.Save(memoryStream, ImageFormat.Png);
                        memoryStream.Seek(0, SeekOrigin.Begin); // Reset the stream position

                        // Create the stream content for the multipart form
                        var streamContent = new StreamContent(memoryStream);
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                        content.Add(streamContent, "image", string.Format("screenshot_{0}_{1:MMddHHmm}.png", screenID, DateTime.Now));
                        client.DefaultRequestHeaders.Host = host;

                        // Send the POST request
                        HttpResponseMessage response = await client.PostAsync(url, content);
                        if (response.IsSuccessStatusCode)
                        {
                            log.DebugFormat("Image uploaded successfully.");
                        }
                        else
                        {
                            log.ErrorFormat("Image upload failed: {0}", response.StatusCode);
                        }
                    }
                }
            }
        }

        private async Task PostJsonAsync(string jsonData, string url)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();
                    log.DebugFormat("Response: {0}", responseBody);
                }
            }
            catch (HttpRequestException e)
            {
                log.ErrorFormat("Request error: {0}", e.Message);
            }
        }
    }
}




