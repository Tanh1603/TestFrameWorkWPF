﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace NaviatePage.Models.Data;

public partial class Material
{
    public string Idmaterial { get; set; }

    public string Displayname { get; set; }

    public int Idunit { get; set; }

    public int Idsupplier { get; set; }

    public string Qrcode { get; set; }

    public string Barcode { get; set; }

    public virtual Supplier IdsupplierNavigation { get; set; }

    public virtual Unit IdunitNavigation { get; set; }

    public virtual ICollection<Inputinfo> Inputinfos { get; set; } = new List<Inputinfo>();

    public virtual ICollection<Outputinfo> Outputinfos { get; set; } = new List<Outputinfo>();
}