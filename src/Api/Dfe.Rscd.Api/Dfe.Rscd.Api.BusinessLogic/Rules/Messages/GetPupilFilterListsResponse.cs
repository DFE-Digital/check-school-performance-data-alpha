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
	/// Service Contract Class - GetPupilFilterListsResponse
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetPupilFilterListsResponse", WrapperNamespace = "http://www.forvus.co.uk/Web09/scm")] 
	public partial class GetPupilFilterListsResponse
	{
		private Web09.Services.DataContracts.CommonList ethnicityList;
	 	private Web09.Services.DataContracts.CommonList languageList;
	 	private Web09.Services.DataContracts.Cohorts cohortList;
	 	private Web09.Services.DataContracts.AwardingBodyCollection awardingBodyList;
	 	private Web09.Services.DataContracts.KeyStageConfigurationList configurationList;
	 	private Web09.Services.DataContracts.CommonList ageList;
	 	private Web09.Services.DataContracts.CommonList yearGroupList;
	 		
		[WCF::MessageBodyMember(Name = "EthnicityList")] 
		public Web09.Services.DataContracts.CommonList EthnicityList
		{
			get { return ethnicityList; }
			set { ethnicityList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "LanguageList")] 
		public Web09.Services.DataContracts.CommonList LanguageList
		{
			get { return languageList; }
			set { languageList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "CohortList")] 
		public Web09.Services.DataContracts.Cohorts CohortList
		{
			get { return cohortList; }
			set { cohortList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "AwardingBodyList")] 
		public Web09.Services.DataContracts.AwardingBodyCollection AwardingBodyList
		{
			get { return awardingBodyList; }
			set { awardingBodyList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "ConfigurationList")] 
		public Web09.Services.DataContracts.KeyStageConfigurationList ConfigurationList
		{
			get { return configurationList; }
			set { configurationList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "AgeList")] 
		public Web09.Services.DataContracts.CommonList AgeList
		{
			get { return ageList; }
			set { ageList = value; }
		}
			
		[WCF::MessageBodyMember(Name = "YearGroupList")] 
		public Web09.Services.DataContracts.CommonList YearGroupList
		{
			get { return yearGroupList; }
			set { yearGroupList = value; }
		}
	}
}

