//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SignalRMvc.App_Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Messages
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Messages()
        {
            this.Messages1 = new HashSet<Messages>();
        }
    
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MeetingRoomId { get; set; }
        public System.DateTime DateTime { get; set; }
        public Nullable<int> ParentMassegeId { get; set; }
        public string Message { get; set; }
    
        public virtual MeetingRoom MeetingRoom { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Messages> Messages1 { get; set; }
        public virtual Messages Messages2 { get; set; }
        public virtual Users Users { get; set; }
    }
}
