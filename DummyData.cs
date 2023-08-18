using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowOfExecutionContext;

public class DummyData
{
    private static AsyncLocal<NestedData> _nestedData = new AsyncLocal<NestedData>();

    public static AsyncLocal<NestedData> NestedData
    {
        get => _nestedData;
        set => _nestedData = value;
    }
}

public class NestedData
{
    public int Id { get; set; }
    public string Type { get; set; }
}