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
	/// Service Contract Class - GetMessageListForSchoolRequest
	/// </summary>
	[WCF::MessageContract(IsWrapped = false)] 
	public partial class GetMessageListForSchoolRequest
	{
		private int dFESNumber;
	 		
		[WCF::MessageBodyMember(Name = "DFESNumber")] 
		public int DFESNumber
		{
			get { return dFESNumber; }
			set { dFESNumber = value; }
		}
	}
}

