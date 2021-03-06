using System;
using DapperRepository;

namespace Auth.Entities
{
    [Table("Test")]
    public class TestModel
    {
        [KeyColumn]
        public int? Id { get; set; }

        public string Data { get; set; }

        [IgnoreColumn]
        public string IgnoreData { get; set; }

        [BindingColumn("RealColumnName")]
        public string FakeNameColumn { get; set; }

        [CreatedDate]
        public DateTime? CDate { get; set; }

        [LastModifiedDate]
        public DateTime? LDate { get; set; }

    }
}