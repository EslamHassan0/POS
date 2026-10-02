using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Contract.Permissions;

public static class POSPermissions
{
    public const string GroupName = "POS";

    // 1. المبيعات (Sales)
    public static class Sales
    {
        public const string Default = GroupName + ".Sales";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Void = Default + ".Void";
        public const string View = Default + ".View";
        public const string Pay = Default + ".Pay";
        public const string Print = Default + ".Print"; // ⬅️ جديد لطباعة الفاتورة
    }

    // 2. المشتريات (Purchases) ⬅️ Group جديد بالكامل
    public static class Purchases
    {
        public const string Default = GroupName + ".Purchases";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
        public const string Pay = Default + ".Pay";       // دفع للمورد
        public const string Print = Default + ".Print";   // طباعة فاتورة الشراء
    }

    // 3. المخزون (Inventory)
    public static class Inventory
    {
        public const string Default = GroupName + ".Inventory";
        public const string AddProduct = Default + ".AddProduct";
        public const string EditProduct = Default + ".EditProduct";
        public const string AdjustStock = Default + ".AdjustStock";
        public const string ViewStock = Default + ".ViewStock";
    }

    // 4. العملاء والموردين (Partners)
    public static class Partners
    {
        public const string Default = GroupName + ".Partners";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string View = Default + ".View";
    }

    // 5. التقارير (Reports)
    public static class Reports
    {
        public const string Default = GroupName + ".Reports";
        public const string ViewSales = Default + ".ViewSales";
        public const string ViewInventory = Default + ".ViewInventory";
        public const string ViewFinancial = Default + ".ViewFinancial";
        public const string ViewPurchases = Default + ".ViewPurchases"; // ⬅️ جديد
    }

    // 6. الإدارة (Administration)
    public static class Administration
    {
        public const string Default = GroupName + ".Administration";
        public const string ManageUsers = Default + ".ManageUsers";
        public const string ManageRoles = Default + ".ManageRoles";
        public const string Settings = Default + ".Settings";
    }

    public static List<string> GetAll()
    {
        return typeof(POSPermissions)
           .GetNestedTypes()
           .SelectMany(t => t.GetFields(
               System.Reflection.BindingFlags.Public |
               System.Reflection.BindingFlags.Static |
               System.Reflection.BindingFlags.FlattenHierarchy))
           .Where(f => f.IsLiteral && !f.IsInitOnly && f.FieldType == typeof(string))
           .Select(f => (string)f.GetValue(null)!)
           .ToList();
    }
}
