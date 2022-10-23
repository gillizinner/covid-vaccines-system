using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace CovidVaccinationSystem.Models
{
    public class memberBE
    {
        [BsonId]
        public ObjectId ID { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [BsonElement("idOfMember")]
        [DisplayName("id")]
        public int idOfMember { get; set; }
        [BsonElement("firstName")]
        [DisplayName("First Name")]
        [Required]
        public string firstName { get; set; }
        [BsonElement("lastName")]
        [DisplayName("Last Name")]
        [Required]
        public string lastName { get; set; }
        [BsonElement("imgUrl")]
        [DisplayName("Image")]
        [Required]
        public string imgUrl { get; set; }
        [BsonElement("phoneNumber")]
        [DisplayName("Phone Number")]
        public int phoneNumber { get; set; }
        [Required]
        [BsonElement("cellphoneNumber")]
        [DisplayName("CellPhone Number")]
        public int cellphoneNumber { get; set; }
        [BsonElement("address")]
        [DisplayName("Address")]
        public Address address { get; set; }
        [BsonElement("dateOfBirth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of Birth")]
        public DateTime dateOfBirth { get; set; }
        [BsonElement("dateOfPositiveResult")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of Positive Result")]
        public DateTime dateOfPositiveResult { get; set; }
        [BsonElement("dateOfRecovery")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]//[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DisplayName("Date of Recovery")]
        public DateTime dateOfRecovery { get; set; }
        
        [BsonElement("vaccinesArray")]
        [DisplayName("list of vaccines")]
        public vaccine[] vaccinesArray { get; set; }






    }
}