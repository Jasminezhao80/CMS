using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// CodeList 的摘要说明
/// </summary>
public enum CodeList
{
    PurchaseType_Internal = 1,//国内采购
    PurchaseType_External = 2,//国外采购

    ProjectType_Sales = 3,//物资销售
    ProjectType_Rent = 4,//设备租赁
    ProjectType_Service = 5,//技术服务
    ProjectType_Others = 6,//其他服务

    ProjectName_Hudong = 7,//沪东项目
    ProjectName_Jiangnan = 8,//江南项目
    ProjectName_Gabadi = 9,//江南gabadi
    ProjectName_Dachuan = 10,//大船项目
    ProjectName_Huisheng = 11,//惠生项目
    ProjectName_NingboKairong = 12,//宁波凯荣

    ContractType_Purchase = 13,//采购合同
    ContractType_Project = 14,//项目合同

    MoneyType_RMB = 15,
    MoneyType_America = 16,

    IsTrue_Y = 20,
    IsTrue_N = 21,
    //出库类型
    OutStore_Sale = 99,//销售出库
    OutStore_Produce =100,//生产领用
    OutStore_Borrow = 101,//借用

}