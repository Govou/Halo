using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halobiz.Common.Auths
{
    public class ModuleName : Attribute
    {
        private HalobizModules _modules;
        private int _controllerId;
        public ModuleName(HalobizModules halobizModules, int controllerId)
        {
            _modules = halobizModules;
            _controllerId = controllerId;
        }

        public (string, int) GetModuleAndControllerId()
        {
            return (_modules.ToString(), _controllerId);
        }

    }

    public enum HalobizModules : int
    {
        Profile,
        Setups,
        Supplier,        
        Finance,
        ClientManagement,
        RolesManagement, 
        LeadAdministration,
        SecuredMobility,
        ProjectManagment,
        ApprovalManagement,
        Reporting,
        ComplaintManagement,
        InventoryManagement,
        GuardManagement,
        PayrollManagement,
        CronJobs,
    }
}
