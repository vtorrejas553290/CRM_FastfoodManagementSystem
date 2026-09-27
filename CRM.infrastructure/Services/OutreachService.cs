using CRM.domain.Entities;
using CRM.domain.Models;

namespace CRM.infrastructure.Services;

public class BulkSendResult
{
    public int SentCount { get; set; }
    public int SkippedCount { get; set; }
    public List<string> SkippedReasons { get; set; } = new();
}

public static class OutreachService
{
    /// <summary>
    /// Logs an outreach entry for each selected recipient. Does NOT actually
    /// send SMS or email — this writes a durable record that can be viewed in
    /// the contact history and later hooked to a real provider. Also records
    /// the promotion (if any) on a CustomerOutreach row so the system can
    /// auto-suggest that promo when the customer returns.
    /// </summary>
    public static BulkSendResult Send(
        List<ContactRecipient> recipients,
        OutreachChannel channel,
        string messageTemplate,
        OutreachPromotion? promotion,
        int staffUserId,
        string staffUsername)
    {
        var result = new BulkSendResult();

        using var db = AppServices.CreateTenantContext();

        foreach (var r in recipients.Where(x => x.Selected))
        {
            // Channel availability checks
            if (channel == OutreachChannel.Sms && !r.CanSms)
            {
                result.SkippedCount++;
                result.SkippedReasons.Add($"{r.CustomerName}: no phone number");
                continue;
            }
            if (channel == OutreachChannel.Email && !r.CanEmail)
            {
                result.SkippedCount++;
                result.SkippedReasons.Add($"{r.CustomerName}: no email address");
                continue;
            }

            string rendered = Render(messageTemplate, r, promotion);

            string channelLabel = channel switch
            {
                OutreachChannel.Sms => "Bulk SMS",
                OutreachChannel.Email => "Bulk Email",
                OutreachChannel.Both => "Bulk SMS+Email",
                _ => "Bulk Contact"
            };

            db.ActivityLogs.Add(new ActivityLog
            {
                UserId = staffUserId,
                Username = staffUsername,
                ActionType = "Contact",
                EntityName = "Customer",
                EntityId = r.CustomerId,
                Description = $"{channelLabel} to '{r.CustomerName}': Sent — {rendered}",
                PerformedAt = DateTime.UtcNow
            });

            // Structured outreach record — carries the promo id so we can
            // auto-suggest it at the POS when the customer returns.
            db.CustomerOutreaches.Add(new CustomerOutreach
            {
                CustomerId = r.CustomerId,
                PromotionId = promotion?.PromotionId,
                Channel = channelLabel,
                MessageBody = rendered,
                SentAt = DateTime.UtcNow,
                SentByUserId = staffUserId
            });

            result.SentCount++;
        }

        db.SaveChanges();
        return result;
    }

    private static string Render(
        string template,
        ContactRecipient r,
        OutreachPromotion? promo)
    {
        string output = template
            .Replace("{CustomerName}", r.CustomerName)
            .Replace("{CustomerCode}", r.CustomerCode)
            .Replace("{Retention}", r.Retention);

        if (promo != null)
        {
            output = output
                .Replace("{PromoCode}", promo.PromotionCode)
                .Replace("{PromoName}", promo.PromotionName)
                .Replace("{DiscountDisplay}", promo.DiscountDisplay)
                .Replace("{EndDate}", promo.EndDate.ToString("yyyy-MM-dd"))
                .Replace("{MinPurchase}",
                    promo.MinimumPurchase.HasValue
                        ? $"Min purchase ₱{promo.MinimumPurchase.Value:N2}."
                        : "");
        }
        else
        {
            output = output
                .Replace("{PromoCode}", "")
                .Replace("{PromoName}", "")
                .Replace("{DiscountDisplay}", "")
                .Replace("{EndDate}", "")
                .Replace("{MinPurchase}", "");
        }

        return output.Trim();
    }
}