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
	/// Service Contract Class - GetLateResultsCountRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetLateResultsCountRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm/services")] 
	public partial class GetLateResultsCountRequest
	{
		private int dCSFNumber;
	 	private System.Nullable<short> keystageID;
	 		
		[WCF::MessageBodyMember(Name = "DCSFNumber")] 
		public int DCSFNumber
		{
			get { return dCSFNumber; }
			set { dCSFNumber = value; }
		}
			
		[WCF::MessageBodyMember(Name = "KeystageID")] 
		public System.Nullable<short> KeystageID
		{
			get { return keystageID; }
			set { keystageID = value; }
		}
	}
}

