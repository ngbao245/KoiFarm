using Repository.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Repository.Data.Entity
{
    public class Entity : ISoftDelete
    {
        protected Entity()
        {
            Id = Guid.NewGuid().ToString("N");
            CreatedTime = LastUpdatedTime = GetCurrenTime();
        }

        [Key]
        public string Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public DateTimeOffset? DeletedTime { get; set; }

        // Soft deletion properties
        public bool IsDeleted { get; set; } // Implement this instead of throwing an exception

        public DateTimeOffset? DeletedAt
        {
            get => DeletedTime; // Link this to the DeletedTime property
            set => DeletedTime = value;
        }

        // Method to get current time in Vietnam timezone
        protected DateTimeOffset GetCurrenTime()
        {
            DateTime serverTime = DateTime.UtcNow;
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh");
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(serverTime, vietnamTimeZone);
            return _localTime;
        }
    }
}
