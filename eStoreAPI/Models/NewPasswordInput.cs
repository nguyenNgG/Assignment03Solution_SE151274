﻿using System.ComponentModel.DataAnnotations;

namespace eStoreAPI.Models
{
    public class NewPasswordInput
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
