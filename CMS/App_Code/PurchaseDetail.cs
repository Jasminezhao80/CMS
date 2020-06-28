using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Model;

/// <summary>
/// PurchaseDetail 的摘要说明
/// </summary>
public class PurchaseDetail
{
    public PurchaseDetail()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    private int id;
    private Product product;
    private int supplierId;
    private string deliveryDate;
    private string inWarehouseDate;
    private decimal unitPrice;
    private int quantity;
    private string memo;

    public int Id { get => id; set => id = value; }
    public int SupplierId { get => supplierId; set => supplierId = value; }
    public string DeliveryDate { get => deliveryDate; set => deliveryDate = value; }
    public string InWarehouseDate { get => inWarehouseDate; set => inWarehouseDate = value; }
    public decimal UnitPrice { get => unitPrice; set => unitPrice = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public string Memo { get => memo; set => memo = value; }
    public Product Product { get => product; set => product = value; }
}