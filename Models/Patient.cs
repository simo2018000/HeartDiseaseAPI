
using MongoDB.Bson; 
using MongoDB.Bson.Serialization.Attributes; 
using System.Globalization; 

namespace HeartDiseaseAPI.Models
{
    public class Patient
    {
        [BsonId] // Marks this as the primary key for MongoDB
        [BsonRepresentation(BsonType.ObjectId)] // Tells the driver to treat this string as an ObjectId
        public string? Id { get; set; } // Changed from int ID to string? Id for MongoDB ObjectId

        // IDENTIFICATION DATA - these remain the same
        public string LastName { get; set; } = String.Empty;
        public string FirstName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty; // This will store the hashed password

        // HEALTH DATA - these remain the same
        public int Age { get; set; } // in days (as per original comment)
        public String Sex { get; set; } = string.Empty;
        public int Height { get; set; } // in cm
        public int Weight { get; set; } // in kg
        public float BloodPressureLow { get; set; } // in mmHg
        public float BloodPressureHigh { get; set; } // in mmHg
        public float Cholesterol { get; set; } // in mg/dL
        public float Glucose { get; set; } // in mg/dL
        public bool IsSmoker { get; set; }
        public bool IsAlcoholic { get; set; }
        public bool IsActive { get; set; }
        public bool HasHeartDisease { get; set; }

        
    }
}