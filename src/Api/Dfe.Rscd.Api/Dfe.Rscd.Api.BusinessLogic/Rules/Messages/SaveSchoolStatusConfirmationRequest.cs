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
	/// Service Contract Class - SaveSchoolStatusConfirmationRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "SaveSchoolStatusConfirmationRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class SaveSchoolStatusConfirmationRequest
	{
		private Web09.Services.DataContracts.UserContext userContext;
	 	private string action;
	 	private string newEmail;
	 	private string newDCSFNumber;
	 	private string oldUserName;
	 	private string consumerURL;
	 		
		[WCF::MessageBodyMember(Name = "UserContext")] 
		public Web09.Services.DataContracts.UserContext UserContext
		{
			get { return userContext; }
			set { userContext = value; }
		}
			
		[WCF::MessageBodyMember(Name = "Action")] 
		public string Action
		{
			get { return action; }
			set { action = value; }
		}
			
		[WCF::MessageBodyMember(Name = "NewEmail")] 
		public string NewEmail
		{
			get { return newEmail; }
			set { newEmail = value; }
		}
			
		[WCF::MessageBodyMember(Name = "NewDCSFNumber")] 
		public string NewDCSFNumber
		{
			get { return newDCSFNumber; }
			set { newDCSFNumber = value; }
		}
			
		[WCF::MessageBodyMember(Name = "OldUserName")] 
		public string OldUserName
		{
			get { return oldUserName; }
			set { oldUserName = value; }
		}
			
		[WCF::MessageBodyMember(Name = "ConsumerURL")] 
		public string ConsumerURL
		{
			get { return consumerURL; }
			set { consumerURL = value; }
		}
	}
}

