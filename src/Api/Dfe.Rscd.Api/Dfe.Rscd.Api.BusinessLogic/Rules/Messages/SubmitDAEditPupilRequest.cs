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
	/// Service Contract Class - SubmitDAEditPupilRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "SubmitDAEditPupilRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class SubmitDAEditPupilRequest
	{
		private Web09.Services.DataContracts.PupilDetails pupil;
	 	private Web09.Services.DataContracts.UserContext userContext;
	 		
		[WCF::MessageBodyMember(Name = "Pupil")] 
		public Web09.Services.DataContracts.PupilDetails Pupil
		{
			get { return pupil; }
			set { pupil = value; }
		}
			
		[WCF::MessageBodyMember(Name = "UserContext")] 
		public Web09.Services.DataContracts.UserContext UserContext
		{
			get { return userContext; }
			set { userContext = value; }
		}
	}
}

