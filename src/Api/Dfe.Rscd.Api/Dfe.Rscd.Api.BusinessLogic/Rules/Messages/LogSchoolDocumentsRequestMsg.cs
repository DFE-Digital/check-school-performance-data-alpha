//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using WCF = global::System.ServiceModel;

namespace Web09.Services.MessageContracts
{
	/// <summary>
	/// Service Contract Class - LogSchoolDocumentsRequestMsg
	/// </summary>
	[WCF::MessageContract(WrapperName = "LogSchoolDocumentsRequestMsg", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class LogSchoolDocumentsRequestMsg
	{
		private System.DateTime requestTime;
	 	private string userName;
	 	private string forename;
	 	private string surname;
	 	private string rolename;
	 	private short documentID;
	 	private string documentTitleAtTimeOfRequest;
	 	private string documentPathAtTimeOfRequest;
	 	private string clientIPAddress;
	 		
		[WCF::MessageBodyMember(Name = "RequestTime")] 
		public System.DateTime RequestTime
		{
			get { return requestTime; }
			set { requestTime = value; }
		}
			
		[WCF::MessageBodyMember(Name = "UserName")] 
		public string UserName
		{
			get { return userName; }
			set { userName = value; }
		}
			
		[WCF::MessageBodyMember(Name = "Forename")] 
		public string Forename
		{
			get { return forename; }
			set { forename = value; }
		}
			
		[WCF::MessageBodyMember(Name = "Surname")] 
		public string Surname
		{
			get { return surname; }
			set { surname = value; }
		}
			
		[WCF::MessageBodyMember(Name = "Rolename")] 
		public string Rolename
		{
			get { return rolename; }
			set { rolename = value; }
		}
			
		[WCF::MessageBodyMember(Name = "DocumentID")] 
		public short DocumentID
		{
			get { return documentID; }
			set { documentID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "DocumentTitleAtTimeOfRequest")] 
		public string DocumentTitleAtTimeOfRequest
		{
			get { return documentTitleAtTimeOfRequest; }
			set { documentTitleAtTimeOfRequest = value; }
		}
			
		[WCF::MessageBodyMember(Name = "DocumentPathAtTimeOfRequest")] 
		public string DocumentPathAtTimeOfRequest
		{
			get { return documentPathAtTimeOfRequest; }
			set { documentPathAtTimeOfRequest = value; }
		}
			
		[WCF::MessageBodyMember(Name = "ClientIPAddress")] 
		public string ClientIPAddress
		{
			get { return clientIPAddress; }
			set { clientIPAddress = value; }
		}
	}
}

