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
	/// Service Contract Class - GetBasicPupilDetailsResponse
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetBasicPupilDetailsResponse", WrapperNamespace = "http://www.forvus.co.uk/Web09/services")] 
	public partial class GetBasicPupilDetailsResponse
	{
		private Web09.Services.DataContracts.Pupil pupilDetails;
	 		
		[WCF::MessageBodyMember(Name = "PupilDetails")] 
		public Web09.Services.DataContracts.Pupil PupilDetails
		{
			get { return pupilDetails; }
			set { pupilDetails = value; }
		}
	}
}

