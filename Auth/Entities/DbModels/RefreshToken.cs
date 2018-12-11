using System;
using DapperRepository;

namespace Auth.Entities
{
    //   CREATE TABLE `RefreshToken` ( 
    //   `Id` TEXT NOT NULL, 
    //   `Audience` TEXT NOT NULL, 
    //   `Token` TEXT, 
    //   CDate   DATE, 
    //   PRIMARY KEY(`Id`) )

    [Table("RefreshToken")]
    public class RefreshToken
    {
        [KeyColumn]
        public string Id { get; set; }
        public string Audience { get; set; }
        public string Token { get; set; }
        public DateTime? CDate { get; set; }
    }
}