using System;

namespace PrescriptionsApp.Models
{
    public class Prescription
    {
        public int Id { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ProductName { get; set; }
        public int UsesLeft { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string PatientId { get; set; }

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