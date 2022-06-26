using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HDBMMVC.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名必填")]
        [StringLength(20, ErrorMessage = "{0}最多为{1}个字符。")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Display(Name = "专业")]
        [StringLength(20, ErrorMessage = "{0}最多为{1}个字符。")]
        [DataType(DataType.Text)]
        public string Major { get; set; }

        [Display(Name = "学号")]
        [Required(ErrorMessage = "学号必填")]
        [StringLength(12, ErrorMessage = "{0}最多为{1}个字符。")]
        [DataType(DataType.Text)]
        public string Sno { get; set; }

        [Display(Name = "手机号")]
        [Required(ErrorMessage = "手机号必填")]
        [RegularExpression(@"^1[3458][0-9]{9}$", ErrorMessage = "手机号格式不正确")]
        [DataType(DataType.Text)]
        public string Phone { get; set; }
        
        [Display(Name = "邮箱")]
        [EmailAddress]
        [DataType(DataType.Text)]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        public string userID { get; set; }
    }
}