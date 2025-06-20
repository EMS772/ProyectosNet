using MailKit.Net.Smtp;
using MimeKit;
using MoneyMindIA.Models.Entidades;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

namespace MoneyMindIA.Services
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(Usuario user, string confirmationLink);
        Task SendPasswordResetEmailAsync(Usuario user, string resetLink);

    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }
            public async Task SendConfirmationEmailAsync(Usuario user, string confirmationLink)
            {
                _logger.LogInformation($"Iniciando envío de correo a {user.Email}");

                try
                {
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("MoneyMindIA", _config["Email:From"]));
                    email.To.Add(new MailboxAddress(user.Nombre, user.Email));
                    email.Subject = "Confirma tu correo electrónico - MoneyMindIA";

                    var bodyBuilder = new BodyBuilder
                    {
                        HtmlBody = $@"
                    <h1>¡Bienvenido a MoneyMind!</h1>
                    <p>Hola {user.Nombre},</p>
                    <p>Gracias por registrarte. Por favor, confirma tu correo electrónico haciendo clic en el siguiente enlace:</p>
                    <p><a href='{confirmationLink}'>Confirmar mi correo electrónico</a></p>
                    <p>Si no solicitaste este registro, puedes ignorar este mensaje.</p>
                    <p>El enlace expirará en 24 horas.</p>"
                    };

                    email.Body = bodyBuilder.ToMessageBody();

                    using var client = new SmtpClient();

                    _logger.LogInformation("Conectando al servidor SMTP...");
                    await client.ConnectAsync(
                        _config["Email:SmtpServer"],
                        int.Parse(_config["Email:Port"]!),
                        SecureSocketOptions.StartTls
                    );

                    _logger.LogInformation("Autenticando...");
                    await client.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);

                    _logger.LogInformation("Enviando email...");
                    await client.SendAsync(email);

                    _logger.LogInformation("Desconectando...");
                    await client.DisconnectAsync(true);

                    _logger.LogInformation($"Correo enviado exitosamente a {user.Email}");
                }
                catch (AuthenticationException authEx)
                {
                    _logger.LogError(authEx, "Error de autenticación con el servidor SMTP");
                    throw new Exception("Error de autenticación con el servidor de correo", authEx);
                }
                catch (SmtpCommandException smtpEx)
                {
                    _logger.LogError(smtpEx, "Error en comando SMTP");
                    throw new Exception("Error en el servidor de correo", smtpEx);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error general enviando correo a {user.Email}");
                    throw new Exception("Error enviando el correo electrónico", ex);
                }
            }

        public async Task SendPasswordResetEmailAsync(Usuario user, string resetLink)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("MoneyMindIA", _config["Email:From"]));
                email.To.Add(new MailboxAddress(user.Nombre, user.Email));
                email.Subject = "Recuperación de contraseña - MoneyMindIA";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                <h1>Recuperación de contraseña</h1>
                <p>Hola {user.Nombre},</p>
                <p>Has solicitado restablecer tu contraseña. Haz clic en el siguiente enlace para crear una nueva contraseña:</p>
                <p><a href='{resetLink}'>Restablecer mi contraseña</a></p>
                <p>Si no solicitaste este cambio, puedes ignorar este mensaje.</p>
                <p>El enlace expirará en 1 hora.</p>"
                };

                email.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                await client.ConnectAsync(
                    _config["Email:SmtpServer"],
                    int.Parse(_config["Email:Port"]!),
                    SecureSocketOptions.StartTls
                );
                await client.AuthenticateAsync(_config["Email:Username"], _config["Email:Password"]);
                await client.SendAsync(email);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando correo de reseteo de contraseña a {user.Email}");
                throw;
            }
        }
    }
}
