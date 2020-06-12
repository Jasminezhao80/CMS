using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// KeyValue 的摘要说明
/// </summary>
public class KeyValue
{
    public KeyValue()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }
    private int key;
    private string value;
    private int type;

    public int Key { get => key; set => key = value; }
    public string Value { get => value; set => this.value = value; }
    public int Type { get => type; set => type = value; }

}