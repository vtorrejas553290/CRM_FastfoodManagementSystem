using CRM.domain.Controllers;
using CRM.domain.Entities;
using CRM.domain.Models;
using CRM.infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.api.Controllers;

public class BranchController : IBranchController
{
    // ================================================================
    // LIST
    // ================================================================
    public List<BranchRow> GetBranches(BranchFilter filter)
    {
        using var db = AppServices.CreateTenantContext(filter.CompanyId);

        var search = (filter.Search ?? "").Trim().ToLower();
        var showArchived = filter.ShowArchived;

        var query = db.Branches.AsNoTracking();

        if (showArchived)
            query = query.Where(x => !x.IsActive);
        else
            query = query.Where(x => x.IsActive);

        // Fetch branches first (with the filter), then fill manager / staff info
        // in a second query so we don't need a complex SQL join.
        var branches = query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                x.BranchCode.ToLower().Contains(search) ||
                x.BranchName.ToLower().Contains(search))
            .OrderBy(x => x.BranchCode)
            .Select(x => new
            {
                x.BranchId,
                x.BranchCode,
                x.BranchName,
                x.Address,
                x.ContactNumber,
                x.IsActive,
                x.CreatedAt
            })
            .ToList();

        var branchIds = branches.Select(b => b.BranchId).ToList();

        // One query for all assigned users in these branches.
        var assignedUsers = db.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .Where(u => u.BranchId != null && branchIds.Contains(u.BranchId.Value))
            .Select(u => new
            {
                u.UserId,
                u.BranchId,
                u.FullName,
                RoleCode = u.Role != null ? u.Role.RoleCode : ""
            })
            .ToList();

        // Group by branch for the grid.
        var managerByBranch = assignedUsers
            .Where(u => u.RoleCode == "MANAGER" && u.BranchId.HasValue)
            .GroupBy(u => u.BranchId!.Value)
            .ToDictionary(g => g.Key, g => g.First().FullName);

        var staffCountByBranch = assignedUsers
            .Where(u => u.RoleCode == "STAFF" && u.BranchId.HasValue)
            .GroupBy(u => u.BranchId!.Value)
            .ToDictionary(g => g.Key, g => g.Count());

        return branches
            .Select(x => new BranchRow
            {
                BranchId = x.BranchId,
                BranchCode = x.BranchCode,
                BranchName = x.BranchName,
                Address = x.Address ?? "",
                ContactNumber = x.ContactNumber ?? "",
                Status = x.IsActive ? "Active" : "Archived",
                CreatedAt = x.CreatedAt,
                ManagerName = managerByBranch.TryGetValue(x.BranchId, out var mgrName)
                              ? mgrName
                              : "—",
                StaffCount = staffCountByBranch.TryGetValue(x.BranchId, out var cnt)
                              ? cnt
                              : 0
            })
            .ToList();
    }

    // ================================================================
    // GET ONE
    // ================================================================
    public BranchRow? GetBranch(int branchId, int companyId)
    {
        using var db = AppServices.CreateTenantContext(companyId);

        return db.Branches
            .AsNoTracking()
            .Where(x => x.BranchId == branchId)
            .Select(x => new BranchRow
            {
                BranchId = x.BranchId,
                BranchCode = x.BranchCode,
                BranchName = x.BranchName,
                Address = x.Address ?? "",
                ContactNumber = x.ContactNumber ?? "",
                Status = x.IsActive ? "Active" : "Archived",
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefault();
    }

    // ================================================================
    // CREATE
    // ================================================================
    public int? CreateBranch(int companyId,
                             string branchCode,
                             string branchName,
                             string? address,
                             string? contactNumber,
                             out string? error)
    {
        error = null;

        branchCode = (branchCode ?? "").Trim();
        branchName = (branchName ?? "").Trim();
        address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();
        contactNumber = string.IsNullOrWhiteSpace(contactNumber) ? null : contactNumber.Trim();

        if (string.IsNullOrWhiteSpace(branchCode)) { error = "Branch code is required."; return null; }
        if (string.IsNullOrWhiteSpace(branchName)) { error = "Branch name is required."; return null; }
        if (branchCode.Length > 50) { error = "Branch code must be 50 characters or fewer."; return null; }
        if (branchName.Length > 200) { error = "Branch name must be 200 characters or fewer."; return null; }
        if (address is { Length: > 500 }) { error = "Address must be 500 characters or fewer."; return null; }
        if (contactNumber is { Length: > 50 }) { error = "Contact number must be 50 characters or fewer."; return null; }

        try
        {
            using var db = AppServices.CreateTenantContext(companyId);

            var exists = db.Branches.Any(x => x.BranchCode == branchCode);
            if (exists)
            {
                error = $"Branch code '{branchCode}' already exists.";
                return null;
            }

            var branch = new Branch
            {
                BranchCode = branchCode,
                BranchName = branchName,
                Address = address,
                ContactNumber = contactNumber,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            db.Branches.Add(branch);
            db.SaveChanges();

            return branch.BranchId;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return null;
        }
    }

    // ================================================================
    // UPDATE
    // ================================================================
    public bool UpdateBranch(int branchId,
                             int companyId,
                             string branchCode,
                             string branchName,
                             string? address,
                             string? contactNumber,
                             out string? error)
    {
        error = null;

        branchCode = (branchCode ?? "").Trim();
        branchName = (branchName ?? "").Trim();
        address = string.IsNullOrWhiteSpace(address) ? null : address.Trim();
        contactNumber = string.IsNullOrWhiteSpace(contactNumber) ? null : contactNumber.Trim();

        if (string.IsNullOrWhiteSpace(branchCode)) { error = "Branch code is required."; return false; }
        if (string.IsNullOrWhiteSpace(branchName)) { error = "Branch name is required."; return false; }
        if (branchCode.Length > 50) { error = "Branch code must be 50 characters or fewer."; return false; }
        if (branchName.Length > 200) { error = "Branch name must be 200 characters or fewer."; return false; }
        if (address is { Length: > 500 }) { error = "Address must be 500 characters or fewer."; return false; }
        if (contactNumber is { Length: > 50 }) { error = "Contact number must be 50 characters or fewer."; return false; }

        try
        {
            using var db = AppServices.CreateTenantContext(companyId);

            var branch = db.Branches.FirstOrDefault(x => x.BranchId == branchId);
            if (branch is null)
            {
                error = "Branch not found.";
                return false;
            }

            var dup = db.Branches.Any(x => x.BranchCode == branchCode && x.BranchId != branchId);
            if (dup)
            {
                error = $"Branch code '{branchCode}' already exists.";
                return false;
            }

            branch.BranchCode = branchCode;
            branch.BranchName = branchName;
            branch.Address = address;
            branch.ContactNumber = contactNumber;

            db.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }

    // ================================================================
    // ARCHIVE / UNARCHIVE
    // ================================================================
    public bool SetBranchActive(int branchId,
                                int companyId,
                                bool isActive,
                                out string? error)
    {
        error = null;

        try
        {
            using var db = AppServices.CreateTenantContext(companyId);

            var branch = db.Branches.FirstOrDefault(x => x.BranchId == branchId);
            if (branch is null)
            {
                error = "Branch not found.";
                return false;
            }

            branch.IsActive = isActive;
            db.SaveChanges();

            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}