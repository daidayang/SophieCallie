using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CRS.HouseKeeper
{
    public class ThreadNLBSupport
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static DateTime NextRateUpdateTime = DateTime.Today;
        private long _Interval = 0;
        HouseKeeper _Parent;
        public bool exitThread;
        public AutoResetEvent exitingThreadDone;

        public ThreadNLBSupport(HouseKeeper p, long interval)
        {
            _Parent = p;
            _Interval = interval;
            exitingThreadDone = new AutoResetEvent(false);
        }

        public void Start()
        {
            log.InfoFormat("ThreadResExport thread starts");

            //while (!exitThread)
            //{
            //    DateTime _Now = DateTime.Now;

            //    if (_Now > NextRateUpdateTime)
            //    {
            //        for (int ClusterID = 1; ClusterID <=1; ClusterID++)
            //        {
            //            #region process one cluster

            //            if (exitThread)
            //                break;

            //            CrsResWS.Support.Support wsSupport = new CRS.Interface.ZDirect.CrsResWS.Support.Support();
            //            bool AccessGranted = wsSupport.AppServiceRequestClusterAccess(ClusterID, _Parent.AppServiceID_ResExport, _Parent.AppServiceID_ResExport * 10 + _Parent.AppServiceInstanceID);
            //            if (log.IsDebugEnabled)
            //                log.DebugFormat("wsSupport.AppServiceRequestClusterAccess(ClusterID={0}, AppServiceID={1}, AppServiceInstanceID={2}) return {3}", ClusterID, _Parent.AppServiceID_ResExport, _Parent.AppServiceID_ResExport * 10 + _Parent.AppServiceInstanceID, AccessGranted);

            //            if (!AccessGranted)
            //                continue;

            //            #region Got permission to proceed

            //            try
            //            {
            //                CrsResWS.Pms.Pms ws = new CrsResWS.Pms.Pms();
            //                CrsResWS.Pms.ResExportInfo[] ExportList = ws.GetResExportList(ClusterID, false);

            //                if (ExportList == null)
            //                    continue;

            //                foreach (CrsResWS.Pms.ResExportInfo export in ExportList)
            //                {
            //                    if (exitThread)
            //                        break;

            //                    #region Regular Res Event

            //                    int LastProcessedResLogID = 0;
            //                    CrsResWS.Pms.ResvIDReslogID[] QueueItems = null;
            //                    if (export.ResExportType == CRS.Interface.ZDirect.CrsResWS.Pms.ResExportTypeEnum.Res_WS_TralixAdv)
            //                    {
            //                        QueueItems = ws.GetResExportEventsForTralix(ClusterID, export.ResExportID);
            //                        if (log.IsInfoEnabled)
            //                            log.InfoFormat("ws.GetResExportEventsForTralix(ClusterID={0}, ResExportID={1}) returns {2} records", ClusterID, export.ResExportID, QueueItems.Length);
            //                    }
            //                    else
            //                    {
            //                        QueueItems = ws.GetResExportEvents(export.ResExportID, export.ChainID, export.HotelID);
            //                        if (log.IsInfoEnabled)
            //                            log.InfoFormat("ws.GetResExportEvents(ResExportID={0},ChainID={1},HotelID={2}) returns {3} records", export.ResExportID, export.ChainID, export.HotelID, QueueItems.Length);
            //                    }

            //                    if (QueueItems != null && QueueItems.Length > 0)
            //                    {
            //                        try
            //                        {
            //                            foreach (CrsResWS.Pms.ResvIDReslogID queItem in QueueItems)
            //                            {
            //                                CrsResWS.Pms.PmsReservation Resv = ws.SelectResvEvent(1, queItem.HotelID, queItem.ResvID, queItem.ReslogID);
            //                                if (Resv == null)
            //                                {
            //                                    log.ErrorFormat("Unable retrieve reservation.  HotelID={0}, ResvID={1}, ResLogID={2}", queItem.HotelID, queItem.ResvID, queItem.ReslogID);
            //                                    break;
            //                                }

            //                                if ((export.Configuration & CrsResWS.Pms.ResExportConfigEnum.IncludeCC) != CrsResWS.Pms.ResExportConfigEnum.IncludeCC)
            //                                    MessageConvertor.CleanUpCC(Resv);

            //                                string XmlStr = MessageConvertor.ResvToText(export.ResExportType, Resv);

            //                                MessageTransmitter xmit = new MessageTransmitter(export, Resv, XmlStr);
            //                                int rc = xmit.Send();

            //                                if (rc == -1)
            //                                {
            //                                    #region Communication Error need to
            //                                    //  1.  Report event
            //                                    //  2.  Exit the hotel loop
            //                                    break;
            //                                    #endregion
            //                                }

            //                                LastProcessedResLogID = (LastProcessedResLogID < queItem.ReslogID) ? queItem.ReslogID : LastProcessedResLogID;

            //                                #region Log Files

            //                                if (_Parent.LocalMessageBackupPath != string.Empty)
            //                                {
            //                                    string foldername = string.Format("{0}\\ResExport\\{1}\\{2}\\{3:yyMMdd}", _Parent.LocalMessageBackupPath, export.Description, Resv.HotelCode, DateTime.Today);
            //                                    if (rc != 0)
            //                                        foldername = string.Format("{0}\\Error", foldername);
            //                                    if (!Directory.Exists(foldername))
            //                                        Directory.CreateDirectory(foldername);
            //                                    int cnt = 1;
            //                                    string filename;
            //                                    do
            //                                    {
            //                                        filename = string.Format("{0}\\{1}_{2}tx.xml", foldername, Resv.CrsResvID, cnt++);
            //                                    } while (File.Exists(filename));
            //                                    TextWriter tw = new StreamWriter(filename);
            //                                    tw.Write(XmlStr);
            //                                    tw.Close();
            //                                }

            //                                #endregion
            //                            }
            //                        }
            //                        catch (Exception e)
            //                        {
            //                            log.ErrorFormat("Error At ProcessOneCluster.  Error Message:{0}", e.Message);
            //                        }
            //                        finally
            //                        {
            //                            if (LastProcessedResLogID > 0)
            //                            {
            //                                ws.UpdateResExportLastResLogID(ClusterID, export.ResExportID, LastProcessedResLogID);
            //                                if (log.IsDebugEnabled)
            //                                    log.DebugFormat("ws.UpdateResExportLastResLogID(ClusterID={0}, export.ResExportID={1}, LastProcessedResLogID={2});", ClusterID, export.ResExportID, LastProcessedResLogID);
            //                            }
            //                        }
            //                    }

            //                    #endregion


            //                    #region Pre / Post Res Event

            //                    LastProcessedResLogID = 0;
            //                    QueueItems = null;
            //                    if (export.ResExportType == CrsResWS.Pms.ResExportTypeEnum.Res_WS_TralixAdv &&
            //                        (export.Configuration & CrsResWS.Pms.ResExportConfigEnum.IncludePrePost) == CrsResWS.Pms.ResExportConfigEnum.IncludePrePost)
            //                    {
            //                        QueueItems = ws.GetPendingPrePostEventsForTralix(ClusterID, export.ResExportID);
            //                        if (log.IsInfoEnabled)
            //                            log.InfoFormat("ws.GetPendingPrePostEventsForTralix(ClusterID={0}, ResExportID={1}) returns {2} records", ClusterID, export.ResExportID, QueueItems.Length);
            //                    }

            //                    if (QueueItems == null || QueueItems.Length <= 0)
            //                        continue;

            //                    try
            //                    {
            //                        foreach (CrsResWS.Pms.ResvIDReslogID queItem in QueueItems)
            //                        {
            //                            CrsResWS.Pms.PmsReservation Resv = ws.SelectResvEvent(1, queItem.HotelID, queItem.ResvID, queItem.ReslogID);
            //                            if (Resv == null)
            //                            {
            //                                log.ErrorFormat("Unable retrieve reservation.  HotelID={0}, ResvID={1}, ResLogID={2}", queItem.HotelID, queItem.ResvID, queItem.ReslogID);
            //                                LastProcessedResLogID = (LastProcessedResLogID < queItem.QueueID) ? queItem.QueueID : LastProcessedResLogID;
            //                            }
            //                            else
            //                            {
            //                                if ((export.Configuration & CrsResWS.Pms.ResExportConfigEnum.IncludeCC) != CrsResWS.Pms.ResExportConfigEnum.IncludeCC)
            //                                    MessageConvertor.CleanUpCC(Resv);

            //                                if (queItem.SendType == 7) Resv.ResStatus = "PRE";
            //                                if (queItem.SendType == 8) Resv.ResStatus = "POST";

            //                                string XmlStr = MessageConvertor.ResvToText(export.ResExportType, Resv);

            //                                MessageTransmitter xmit = new MessageTransmitter(export, Resv, XmlStr);
            //                                int rc = xmit.Send();

            //                                if (rc == -1)
            //                                {
            //                                    #region Communication Error need to
            //                                    //  1.  Report event
            //                                    //  2.  Exit the hotel loop
            //                                    break;
            //                                    #endregion
            //                                }

            //                                LastProcessedResLogID = (LastProcessedResLogID < queItem.QueueID) ? queItem.QueueID : LastProcessedResLogID;

            //                                #region Log Files

            //                                if (_Parent.LocalMessageBackupPath != string.Empty)
            //                                {
            //                                    string foldername = string.Format("{0}\\ResExport\\{1}\\{2}\\{3:yyMMdd}", _Parent.LocalMessageBackupPath, export.Description, Resv.HotelCode, DateTime.Today);
            //                                    if (rc != 0)
            //                                        foldername = string.Format("{0}\\Error", foldername);
            //                                    if (!Directory.Exists(foldername))
            //                                        Directory.CreateDirectory(foldername);
            //                                    int cnt = 1;
            //                                    string filename;
            //                                    do
            //                                    {
            //                                        filename = string.Format("{0}\\{1}_{2}tx.xml", foldername, Resv.CrsResvID, cnt++);
            //                                    } while (File.Exists(filename));
            //                                    TextWriter tw = new StreamWriter(filename);
            //                                    tw.Write(XmlStr);
            //                                    tw.Close();
            //                                }

            //                                #endregion
            //                            }
            //                        }
            //                    }
            //                    catch (Exception e)
            //                    {
            //                        log.ErrorFormat("Error At ProcessOneCluster.  Error Message:{0}", e.Message);
            //                    }
            //                    finally
            //                    {
            //                        if (LastProcessedResLogID > 0)
            //                        {
            //                            ws.UpdateResExportInvValue(ClusterID, export.ResExportID, LastProcessedResLogID, "intParam1");
            //                            if (log.IsDebugEnabled)
            //                                log.DebugFormat("ws.UpdateResExportInvValue(ClusterID={0}, export.ResExportID={1}, LastProcessedResLogID={2}, 'intParam1');", ClusterID, export.ResExportID, LastProcessedResLogID);
            //                        }
            //                    }

            //                    #endregion
            //                }
            //            }
            //            finally
            //            {
            //                AccessGranted = wsSupport.AppServiceReleaseClusterAccess(ClusterID, _Parent.AppServiceID_ResExport, _Parent.AppServiceID_ResExport * 10 + _Parent.AppServiceInstanceID);
            //                if (log.IsDebugEnabled)
            //                    log.DebugFormat("wsSupport.AppServiceReleaseClusterAccess(ClusterID={0}, AppServiceID={1}, AppServiceInstanceID={2}) returns {3}", ClusterID, _Parent.AppServiceID_ResExport, _Parent.AppServiceID_ResExport * 10 + _Parent.AppServiceInstanceID, AccessGranted);
            //            }

            //            #endregion

            //            #endregion
            //        }

            //        while (NextRateUpdateTime < _Now)
            //            NextRateUpdateTime = NextRateUpdateTime.AddSeconds(_Interval);
            //    }

            //    if (log.IsDebugEnabled && _Parent.DebugTimeTick)
            //        log.Debug("ThreadResExport go into sleep for 5s");

            //    System.Threading.Thread.Sleep(5000);
            //}

            //log.InfoFormat("ThreadResExport thread exit");

            ////  Signal the world that I exited.
            //exitingThreadDone.Set();
        }
    }
}
