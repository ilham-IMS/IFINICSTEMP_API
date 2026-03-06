using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;

namespace Domain.Abstract.Service
{
  public interface IMasterApprovalService : IBaseService<MasterApproval>
  {
      Task<int> ChangeIsActive(MasterApproval model);
      Task<int> ProcessSync(IDbTransaction transaction, InterfaceMasterApproval model);
  }
}