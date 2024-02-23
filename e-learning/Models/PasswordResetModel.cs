namespace e_learning.Models
{
    public class PasswordResetModel
    {
        public string userid { get; set; }
        public string existingPassword { get; set; }
        public string newPassword { get; set; }
    }
}
