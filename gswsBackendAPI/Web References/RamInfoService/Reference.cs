﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace gswsBackendAPI.RamInfoService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServicesRWMSSoap11Binding", Namespace="http://service.rwms.org")]
    public partial class ServicesRWMS : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback RWMSTopupOperationCompleted;
        
        private System.Threading.SendOrPostCallback RWMSConfirmPaymentOperationCompleted;
        
        private System.Threading.SendOrPostCallback RWMSDisplayWalletOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ServicesRWMS() {
            this.Url = global::gswsBackendAPI.Properties.Settings.Default.gswsBackendAPI_RamInfoService_ServicesRWMS;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event RWMSTopupCompletedEventHandler RWMSTopupCompleted;
        
        /// <remarks/>
        public event RWMSConfirmPaymentCompletedEventHandler RWMSConfirmPaymentCompleted;
        
        /// <remarks/>
        public event RWMSDisplayWalletCompletedEventHandler RWMSDisplayWalletCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RWMSTopup", RequestNamespace="http://service.rwms.org", ResponseNamespace="http://service.rwms.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", IsNullable=true)]
        public TopupResBean RWMSTopup([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] TopupReqBean topupReqBean) {
            object[] results = this.Invoke("RWMSTopup", new object[] {
                        topupReqBean});
            return ((TopupResBean)(results[0]));
        }
        
        /// <remarks/>
        public void RWMSTopupAsync(TopupReqBean topupReqBean) {
            this.RWMSTopupAsync(topupReqBean, null);
        }
        
        /// <remarks/>
        public void RWMSTopupAsync(TopupReqBean topupReqBean, object userState) {
            if ((this.RWMSTopupOperationCompleted == null)) {
                this.RWMSTopupOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRWMSTopupOperationCompleted);
            }
            this.InvokeAsync("RWMSTopup", new object[] {
                        topupReqBean}, this.RWMSTopupOperationCompleted, userState);
        }
        
        private void OnRWMSTopupOperationCompleted(object arg) {
            if ((this.RWMSTopupCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RWMSTopupCompleted(this, new RWMSTopupCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RWMSConfirmPayment", RequestNamespace="http://service.rwms.org", ResponseNamespace="http://service.rwms.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", IsNullable=true)]
        public TransResBean RWMSConfirmPayment([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] TransReqBean transReqBean) {
            object[] results = this.Invoke("RWMSConfirmPayment", new object[] {
                        transReqBean});
            return ((TransResBean)(results[0]));
        }
        
        /// <remarks/>
        public void RWMSConfirmPaymentAsync(TransReqBean transReqBean) {
            this.RWMSConfirmPaymentAsync(transReqBean, null);
        }
        
        /// <remarks/>
        public void RWMSConfirmPaymentAsync(TransReqBean transReqBean, object userState) {
            if ((this.RWMSConfirmPaymentOperationCompleted == null)) {
                this.RWMSConfirmPaymentOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRWMSConfirmPaymentOperationCompleted);
            }
            this.InvokeAsync("RWMSConfirmPayment", new object[] {
                        transReqBean}, this.RWMSConfirmPaymentOperationCompleted, userState);
        }
        
        private void OnRWMSConfirmPaymentOperationCompleted(object arg) {
            if ((this.RWMSConfirmPaymentCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RWMSConfirmPaymentCompleted(this, new RWMSConfirmPaymentCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("urn:RWMSDisplayWallet", RequestNamespace="http://service.rwms.org", ResponseNamespace="http://service.rwms.org", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", IsNullable=true)]
        public WalletResBean RWMSDisplayWallet([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] WalletReqBean walletReqBean) {
            object[] results = this.Invoke("RWMSDisplayWallet", new object[] {
                        walletReqBean});
            return ((WalletResBean)(results[0]));
        }
        
        /// <remarks/>
        public void RWMSDisplayWalletAsync(WalletReqBean walletReqBean) {
            this.RWMSDisplayWalletAsync(walletReqBean, null);
        }
        
        /// <remarks/>
        public void RWMSDisplayWalletAsync(WalletReqBean walletReqBean, object userState) {
            if ((this.RWMSDisplayWalletOperationCompleted == null)) {
                this.RWMSDisplayWalletOperationCompleted = new System.Threading.SendOrPostCallback(this.OnRWMSDisplayWalletOperationCompleted);
            }
            this.InvokeAsync("RWMSDisplayWallet", new object[] {
                        walletReqBean}, this.RWMSDisplayWalletOperationCompleted, userState);
        }
        
        private void OnRWMSDisplayWalletOperationCompleted(object arg) {
            if ((this.RWMSDisplayWalletCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.RWMSDisplayWalletCompleted(this, new RWMSDisplayWalletCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://beans.rwms.org/xsd")]
    public partial class TopupReqBean {
        
        private string amountField;
        
        private string detailsField;
        
        private string marchantIdField;
        
        private string referanceNoField;
        
        private string strDistCodeField;
        
        private string strGWSCodeField;
        
        private string strPassWordField;
        
        private string strPayModeField;
        
        private string strUserIdField;
        
        private string transDateField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string details {
            get {
                return this.detailsField;
            }
            set {
                this.detailsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string marchantId {
            get {
                return this.marchantIdField;
            }
            set {
                this.marchantIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string referanceNo {
            get {
                return this.referanceNoField;
            }
            set {
                this.referanceNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strDistCode {
            get {
                return this.strDistCodeField;
            }
            set {
                this.strDistCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strGWSCode {
            get {
                return this.strGWSCodeField;
            }
            set {
                this.strGWSCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strPassWord {
            get {
                return this.strPassWordField;
            }
            set {
                this.strPassWordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strPayMode {
            get {
                return this.strPayModeField;
            }
            set {
                this.strPayModeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strUserId {
            get {
                return this.strUserIdField;
            }
            set {
                this.strUserIdField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string transDate {
            get {
                return this.transDateField;
            }
            set {
                this.transDateField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://beans.rwms.org/xsd")]
    public partial class WalletResBean {
        
        private string errorCodeField;
        
        private string msgField;
        
        private string topupAmountField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string errorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string msg {
            get {
                return this.msgField;
            }
            set {
                this.msgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string topupAmount {
            get {
                return this.topupAmountField;
            }
            set {
                this.topupAmountField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://beans.rwms.org/xsd")]
    public partial class WalletReqBean {
        
        private string distCodeField;
        
        private string strGWSCodeField;
        
        private string strPassWordField;
        
        private string strUserIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string distCode {
            get {
                return this.distCodeField;
            }
            set {
                this.distCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strGWSCode {
            get {
                return this.strGWSCodeField;
            }
            set {
                this.strGWSCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strPassWord {
            get {
                return this.strPassWordField;
            }
            set {
                this.strPassWordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strUserId {
            get {
                return this.strUserIdField;
            }
            set {
                this.strUserIdField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://beans.rwms.org/xsd")]
    public partial class TransResBean {
        
        private string errorCodeField;
        
        private string msgField;
        
        private string statusField;
        
        private string strTransNoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string errorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string msg {
            get {
                return this.msgField;
            }
            set {
                this.msgField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strTransNo {
            get {
                return this.strTransNoField;
            }
            set {
                this.strTransNoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://beans.rwms.org/xsd")]
    public partial class TransReqBean {
        
        private string strAgTypeField;
        
        private string strConsNameField;
        
        private string strConsNoField;
        
        private string strDateTimeField;
        
        private string strDeptCodeField;
        
        private string strDeptRcptDtField;
        
        private string strDeptRcptNoField;
        
        private string strDistCodeField;
        
        private string strFStatusField;
        
        private string strGWSCodeField;
        
        private string strPassWordField;
        
        private string strPayModeField;
        
        private string strServCodeField;
        
        private string strStaffCodeField;
        
        private string strTotAmtField;
        
        private string strTranDateField;
        
        private string strTransStField;
        
        private string strUserChrgsField;
        
        private string strUserIdField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strAgType {
            get {
                return this.strAgTypeField;
            }
            set {
                this.strAgTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strConsName {
            get {
                return this.strConsNameField;
            }
            set {
                this.strConsNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strConsNo {
            get {
                return this.strConsNoField;
            }
            set {
                this.strConsNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strDateTime {
            get {
                return this.strDateTimeField;
            }
            set {
                this.strDateTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strDeptCode {
            get {
                return this.strDeptCodeField;
            }
            set {
                this.strDeptCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strDeptRcptDt {
            get {
                return this.strDeptRcptDtField;
            }
            set {
                this.strDeptRcptDtField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strDeptRcptNo {
            get {
                return this.strDeptRcptNoField;
            }
            set {
                this.strDeptRcptNoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strDistCode {
            get {
                return this.strDistCodeField;
            }
            set {
                this.strDistCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strFStatus {
            get {
                return this.strFStatusField;
            }
            set {
                this.strFStatusField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strGWSCode {
            get {
                return this.strGWSCodeField;
            }
            set {
                this.strGWSCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strPassWord {
            get {
                return this.strPassWordField;
            }
            set {
                this.strPassWordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strPayMode {
            get {
                return this.strPayModeField;
            }
            set {
                this.strPayModeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strServCode {
            get {
                return this.strServCodeField;
            }
            set {
                this.strServCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strStaffCode {
            get {
                return this.strStaffCodeField;
            }
            set {
                this.strStaffCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strTotAmt {
            get {
                return this.strTotAmtField;
            }
            set {
                this.strTotAmtField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strTranDate {
            get {
                return this.strTranDateField;
            }
            set {
                this.strTranDateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strTransSt {
            get {
                return this.strTransStField;
            }
            set {
                this.strTransStField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strUserChrgs {
            get {
                return this.strUserChrgsField;
            }
            set {
                this.strUserChrgsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string strUserId {
            get {
                return this.strUserIdField;
            }
            set {
                this.strUserIdField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3761.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://beans.rwms.org/xsd")]
    public partial class TopupResBean {
        
        private string errorCodeField;
        
        private string msgField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string errorCode {
            get {
                return this.errorCodeField;
            }
            set {
                this.errorCodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string msg {
            get {
                return this.msgField;
            }
            set {
                this.msgField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void RWMSTopupCompletedEventHandler(object sender, RWMSTopupCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RWMSTopupCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RWMSTopupCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TopupResBean Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TopupResBean)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void RWMSConfirmPaymentCompletedEventHandler(object sender, RWMSConfirmPaymentCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RWMSConfirmPaymentCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RWMSConfirmPaymentCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public TransResBean Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((TransResBean)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    public delegate void RWMSDisplayWalletCompletedEventHandler(object sender, RWMSDisplayWalletCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3761.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class RWMSDisplayWalletCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal RWMSDisplayWalletCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public WalletResBean Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((WalletResBean)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591