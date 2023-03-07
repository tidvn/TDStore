using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace TDStore.Models
{
    public class ImageData
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = null!;


        [BsonElement("Name")]
        public string Title { get; set; } = null!;


        [Display(Name = "UploadOn")]
        [DataType(DataType.Date)]
        public DateTime UploadOn { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}
