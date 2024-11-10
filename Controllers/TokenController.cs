using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using youtubeApi.Services;
using youtubeApi.Servics;

namespace youtubeApi.Controllers
{
   [ApiController]
    [Route("api/[controller]")]
    public class AppTokenController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly EmailService _emailService;
        private readonly CodeStore _codeStore;
        

        public AppTokenController(TokenService tokenService, EmailService emailService , CodeStore codeStore )
        {
            _tokenService = tokenService;
            _emailService = emailService;
            _codeStore = codeStore;
        }

       


        [HttpGet("send-token")]
        public async Task<IActionResult> SendToken(string recipientEmail)
        {
            if (string.IsNullOrWhiteSpace(recipientEmail))
            {
                return BadRequest("Recipient email is required.");
            }

            // var token = _tokenService.GenerateToken();
            var token = _tokenService.Generates4DigitCode();
            _codeStore.StoreCode(recipientEmail , token);
            
            var message =  token;
           

            try
            {
                await _emailService.SendEmailAsync(recipientEmail, "Your Token", message);
                // return Ok(message);
                //   return Ok(new { UserToken = message });
                return Ok(new { message = "Token sent successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

 [HttpGet("verify-code")]
    public IActionResult VerifyCode(string email, string code)
    {
        var storedCode = _codeStore.GetCode(email);
        if (storedCode != null && storedCode == code)
        {
            return Ok(new { message = "Verification successful!" });
        }
        else
        {
            return BadRequest(new { message = "Invalid verification code." });
        }
    }

     

    

    }

    
}
