using System.ComponentModel;

namespace IdentityService.Domain.Identity.Enums;
public enum UserType
{
    [Description("مدير النظام")]
    SuperAdmin = 0,

    [Description("شركة توزيع المنتجات النفطية")]
    DistributionCompany = 1,

    [Description("مستخدم المحطات")]
    Station = 2,

    EPaymentCompany = 3,

    Operation = 4,

    Reconciliation = 5,

    [Description("هيئة توزيع")]
    Authority = 6,

    [Description("قسم من هيئة التوزيع")]
    Branch = 7,
}