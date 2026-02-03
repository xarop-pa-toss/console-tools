using System;
using System.Collections.Generic;
using System.Text;

namespace MotorDArranque.Modelos;

public class ProcessoResultado
{
    public int CodigoErro { get; set; }
    public string DescErro { get; set; } = string.Empty;
    public string StdOut { get; set; } = string.Empty;
    public string StdErr { get; set; } = string.Empty;
}
