﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace NaviatePage.Models.Data;

public partial class Outputinfo
{
    public string Idoutputinfo { get; set; }

    public string Idmaterial { get; set; }

    public string Idoutput { get; set; }

    public int Idcustomer { get; set; }

    public int? Count { get; set; }

    public string Status { get; set; }

    public virtual Customer IdcustomerNavigation { get; set; }

    public virtual Material IdmaterialNavigation { get; set; }

    public virtual Output IdoutputNavigation { get; set; }
}