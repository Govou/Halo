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
        public ModuleName(HalobizModules halobizModules)
        {
            _modules = halobizModules;
        }

        public string GetModuleName()
        {
            return _modules.ToString();
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
