using ConfigurationService.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConfigurationService.Interfaces
{
    public interface IPractitionerDepartment
    {
        Task<ResultModel<object>> UpdateStatus(TokenModel oTokenModel, string PractitionerDepartmentUid);
        Task<ResultModel<DepartmentPractitionerMappingFromMasterModel>> InsertPractitionerProviderLocationDepartmentMapping(TokenModel oTokenModel, List<PractitionerDepartmentModel> oPractitionerDepartmentModel, string PractitionerServiceUpdateDeptPractEndPoint);
        Task<ResultModel<object>> Delete(TokenModel oTokenModel, string practitionerProviderDepartmentUid, string PractitionerServiceUpdateDeptPractEndPoint);
    }
}
