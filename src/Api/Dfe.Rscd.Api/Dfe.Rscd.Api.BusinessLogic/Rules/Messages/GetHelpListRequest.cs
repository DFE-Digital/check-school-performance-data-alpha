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
	/// Service Contract Class - GetHelpListRequest
	/// </summary>
	[WCF::MessageContract(WrapperName = "GetHelpListRequest", WrapperNamespace = "http://www.forvus.co.uk/Web09/services")] 
	public partial class GetHelpListRequest
	{
		private System.Nullable<short> cohortID;
	 	private System.Nullable<short> schoolGroupID;
	 	private System.Nullable<short> pageID;
	 	private System.Nullable<bool> isJune;
	 	private System.Nullable<bool> isSept;
	 	private System.Nullable<bool> isActive;
	 	private string userLevelID;
	 	private bool includeDetailInfo;
	 	private string pageName;
	 		
		[WCF::MessageBodyMember(Name = "CohortID")] 
		public System.Nullable<short> CohortID
		{
			get { return cohortID; }
			set { cohortID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "SchoolGroupID")] 
		public System.Nullable<short> SchoolGroupID
		{
			get { return schoolGroupID; }
			set { schoolGroupID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "PageID")] 
		public System.Nullable<short> PageID
		{
			get { return pageID; }
			set { pageID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "IsJune")] 
		public System.Nullable<bool> IsJune
		{
			get { return isJune; }
			set { isJune = value; }
		}
			
		[WCF::MessageBodyMember(Name = "IsSept")] 
		public System.Nullable<bool> IsSept
		{
			get { return isSept; }
			set { isSept = value; }
		}
			
		[WCF::MessageBodyMember(Name = "IsActive")] 
		public System.Nullable<bool> IsActive
		{
			get { return isActive; }
			set { isActive = value; }
		}
			
		[WCF::MessageBodyMember(Name = "UserLevelID")] 
		public string UserLevelID
		{
			get { return userLevelID; }
			set { userLevelID = value; }
		}
			
		[WCF::MessageBodyMember(Name = "IncludeDetailInfo")] 
		public bool IncludeDetailInfo
		{
			get { return includeDetailInfo; }
			set { includeDetailInfo = value; }
		}
			
		[WCF::MessageBodyMember(Name = "PageName")] 
		public string PageName
		{
			get { return pageName; }
			set { pageName = value; }
		}
	}
}

