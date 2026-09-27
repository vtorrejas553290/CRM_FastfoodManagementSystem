using CRM.domain.Models;

namespace CRM.domain.Controllers;

public interface IBranchController
{
    /// <summary>List branches in the given tenant, filtered and sorted.</summary>
    List<BranchRow> GetBranches(BranchFilter filter);

    /// <summary>Load a single branch for editing. Returns null if not found.</summary>
    BranchRow? GetBranch(int branchId, int companyId);

    /// <summary>
    /// Create a new branch in the given tenant.
    /// Returns the new BranchId, or null on failure with <paramref name="error"/> set.
    /// </summary>
    int? CreateBranch(int companyId,
                      string branchCode,
                      string branchName,
                      string? address,
                      string? contactNumber,
                      out string? error);

    /// <summary>
    /// Update an existing branch. Returns true on success, false with
    /// <paramref name="error"/> set otherwise.
    /// </summary>
    bool UpdateBranch(int branchId,
                      int companyId,
                      string branchCode,
                      string branchName,
                      string? address,
                      string? contactNumber,
                      out string? error);

    /// <summary>
    /// Archive (IsActive=false) or unarchive (IsActive=true) a branch.
    /// </summary>
    bool SetBranchActive(int branchId,
                         int companyId,
                         bool isActive,
                         out string? error);
}