using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using TangyRestaurantWebsite.Utility;

namespace TangyRestaurantWebsite.Extensions
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }

        public static Task SendOrderStatusAsync(this IEmailSender emailSender, string email, string orderNumber, string status)
        {
            string subject = "";
            string message = "";

            if (status == SD.StatusCancelled)
            {
                subject = "Order Cancelled";
                message = "Order Number " + orderNumber + " has been Cancelled! Please conatac us if you have any questions.";
            }
            if (status == SD.StatusSubmitted)
            {
                subject = "Order Created Successfully";
                message = "Order Number " + orderNumber + " has been created sucessfully.";
            }
            if (status == SD.StatusReady)
            {
                subject = "Order Ready for Pickup";
                message = "Order Number " + orderNumber + " is ready for Pickup! Please conatac us if you have any questions.";
            }

            return emailSender.SendEmailAsync(email, subject, message);
        }
    }
}

