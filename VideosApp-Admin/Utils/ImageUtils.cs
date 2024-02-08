namespace VideosApp_Admin.Utils
{
	public class ImageUtils
	{
		public static byte[] FileToByteArray(IFormFile file)
        {
            byte[] buffer = new byte[1024];
            var bytesList = new List<byte>();

            using (var stream = file.OpenReadStream())
            {
                int read;
                while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int i = 0; i < read; i++)
                    {
                        bytesList.Add(buffer[i]);
                    }
                }
            }

            return bytesList.ToArray();
        }
	}
}
