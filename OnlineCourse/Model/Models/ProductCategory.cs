//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductCategory
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string MetaTitle { get; set; }
        public Nullable<long> ParentID { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> ShowOnHome { get; set; }
    }
}
