using System;
using System.ComponentModel.DataAnnotations;

namespace $rootnamespace$.Models
{
    public partial class File
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Original { get; set; }

        [Required]
        [StringLength(24)]
        public string Type { get; set; }

        public long Size { get; set; }

        public DateTime UploadTime { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string Preview { get; set; }
    }
}
