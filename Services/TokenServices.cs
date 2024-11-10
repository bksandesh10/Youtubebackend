namespace youtubeApi.Servics {

    public class TokenService {

        private readonly Random random = new Random();

        public string Generates4DigitCode() {
            
            return random.Next(1000 , 9999).ToString();

        }
    }
}