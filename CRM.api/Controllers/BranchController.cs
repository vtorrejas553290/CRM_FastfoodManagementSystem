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

        return query
            .Where(x =>
                string.IsNullOrWhiteSpace(search) ||
                x.BranchCode.ToLower().Contains(search) ||
                x.BranchName.ToLower().Contains(search))
            .OrderBy(x => x.BranchCode)
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

            // Unique BranchCode check
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

            // Unique BranchCode check (excluding self)
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

            // Optional guard: don't let archiving a branch orphan users who
            // are currently assigned to it without warning. We allow it (FK is
            // SET NULL), but you could block here if desired.
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