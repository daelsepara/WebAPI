using System;

namespace PrescriptionsApp.Models
{
    public class Prescription
    {
        public int Id;
        public DateTime? ExpirationDate = null;
        public string ProductName = null;
        public int? UsesLeft = null;
        public string Description = null;
        public bool? IsActive = null;
        public string PatientId = null;

        public Prescription(int id, DateTime expirationDate, string productName, int usesLeft, string description, bool isActive, string patientId)
        {
            Id = id;
            ExpirationDate = expirationDate;
            ProductName = productName;
            UsesLeft = usesLeft;
            Description = description;
            IsActive = isActive;
            PatientId = patientId;
        }
    }
}