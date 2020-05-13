using System;

namespace ho1a.applicationCore.Entities
{
    public class BaseEntityId : BaseEntity
    {
        public bool Active { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; }
        public DateTime? Edited { get; set; } = null;
        public string EditedBy { get; set; }
        public int Id { get; set; }
    }
}