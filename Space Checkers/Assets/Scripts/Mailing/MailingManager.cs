using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

// https://answers.unity.com/questions/378545/send-an-email-from-the-game.html

public class MailingManager : MonoBehaviour
{
    public string toMailAddress;
    public string toMailName;
    public string invitationCode;
    public string senderName;
    
    public void SendMail()
    {
        var fromAddress = new MailAddress("spacecheckers@gmail.com", "Space Checkers");
        var toAddress = new MailAddress(toMailAddress, toMailName);

        const string fromPassword = "anitalavalatina";
        const string subject = "Space Checkers Invitation";

        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword) as ICredentialsByHost
        };

        string body = string.Empty;

        string templatePath = Path.Combine("MailTemplates", "invitationMailTemplate.html");
		string filePath = Path.Combine(Application.streamingAssetsPath, templatePath);
        
        using (StreamReader reader = new StreamReader(filePath))
        {

            body = reader.ReadToEnd();

        }

        body = body.Replace("{invitationCode}", invitationCode);
        body = body.Replace("{senderName}", senderName);

        var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        };

        message.IsBodyHtml = true;

        //message.CC.Add(lanzi);

        message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess | DeliveryNotificationOptions.OnFailure;

        //message.Attachments.Add(new Attachment(_FileLocation + "\\" + _FileName));

        smtp.Send(message);

    }
}
