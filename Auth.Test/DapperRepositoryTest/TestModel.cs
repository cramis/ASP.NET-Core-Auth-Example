using System;

namespace DapperRepository.Test
{
    [Table("Test")]
    public class TestClass
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


    [Table("Test2")]
    public class Test2Class
    {
        [KeyColumn]
        [AutoCreate]
        public int? Id { get; set; }

        [RequiredColumn]
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