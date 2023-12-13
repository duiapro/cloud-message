namespace Message.Sms.Web.Infrastructure.Tools
{
    public class UploadHelper
    {
        readonly static string _fileUrlBase = "upload";
        readonly static string _filePathBase = $"wwwroot\\{_fileUrlBase}\\";
        public static async Task<string?> ImageAsync(IFormFile file, string filePath = "images")
        {
            string fileName = file.FileName;
            if (string.IsNullOrEmpty(fileName))//服务器是否存在该文件
            {
                return null;
            }
            // 获取上传的图片名称和扩展名称
            string fileFullName = Path.GetFileName(file.FileName);
            string fileExtName = Path.GetExtension(fileFullName);
            //获取当前项目所在的物流路径
            string imgPath = $"{_filePathBase}{filePath}\\";
            var newPath = $"{Guid.NewGuid()}{fileExtName}";
            var src = imgPath + newPath;

            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }
            try
            {
                using (FileStream fs = System.IO.File.Create(src))
                {
                    await file.CopyToAsync(fs);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return $"/{_fileUrlBase}/{filePath}/{newPath}";
        }
    }
}
