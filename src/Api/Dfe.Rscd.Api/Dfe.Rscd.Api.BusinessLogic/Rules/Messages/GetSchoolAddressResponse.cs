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
	/// Service Contract Class - GetSchoolAddressResponse
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetSchoolAddressResponse", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetSchoolAddressResponse
	{
		private Web09.Services.DataContracts.SchoolAddress schoolAddress;
	 		
		[WCF::MessageBodyMember(Name = "SchoolAddress")] 
		public Web09.Services.DataContracts.SchoolAddress SchoolAddress
		{
			get { return schoolAddress; }
			set { schoolAddress = value; }
		}
	}
}

