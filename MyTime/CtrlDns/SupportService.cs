﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HouseKeeper
{

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://force10.com/CoreReservationService/200802", ConfigurationName = "SupportServiceContract")]
    public interface SupportServiceContract
    {

        [System.ServiceModel.OperationContractAttribute(Action = "http://force10.com/CoreReservationService/200802/SupportServiceContract/RefreshCa" +
            "che", ReplyAction = "http://force10.com/CoreReservationService/200802/SupportServiceContract/RefreshCa" +
            "cheResponse")]
        void RefreshCache();

        [System.ServiceModel.OperationContractAttribute(Action = "http://force10.com/CoreReservationService/200802/SupportServiceContract/RefreshCa" +
            "cheDelta", ReplyAction = "http://force10.com/CoreReservationService/200802/SupportServiceContract/RefreshCa" +
            "cheDeltaResponse")]
        void RefreshCacheDelta();
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public interface SupportServiceContractChannel : SupportServiceContract, System.ServiceModel.IClientChannel
    {
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
    public partial class SupportServiceContractClient : System.ServiceModel.ClientBase<SupportServiceContract>, SupportServiceContract
    {

        public SupportServiceContractClient()
        {
        }

        public SupportServiceContractClient(string endpointConfigurationName) :
            base(endpointConfigurationName)
        {
        }

        public SupportServiceContractClient(string endpointConfigurationName, string remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public SupportServiceContractClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(endpointConfigurationName, remoteAddress)
        {
        }

        public SupportServiceContractClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(binding, remoteAddress)
        {
        }

        public void RefreshCache()
        {
            base.Channel.RefreshCache();
        }

        public void RefreshCacheDelta()
        {
            base.Channel.RefreshCacheDelta();
        }
    }
}