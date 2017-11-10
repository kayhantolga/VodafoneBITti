using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodafoneBITti
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();

        }
        static async Task MainAsync()
        {
            List<Flocks> flocks;

            while (true)
            {
                Console.WriteLine("Telefon Numarası (örn: 905441234567):");
                var phoneNumber = Console.ReadLine();
                Console.WriteLine("Şifre:");
                var password = Console.ReadLine();
                try
                {
                    await VodafoneHelper.GetTokenAsync(phoneNumber, password);
                    Console.WriteLine("---Login işlemi başarılı---");
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Bir hata oluştu, bu bilgileriniz yanlış olmasından kaynaklı olabilir. Olmayadabilir.");
                }

            }
            while (true)
            {
                Console.WriteLine("Bu ksımda size yakın bir koordinat sallayabilirsiniz. Ama doğru koordinat yazmanız daha yararlı.");

                Console.WriteLine("Enlem koordinatın (örn: 41.34235):");
                var lati = Console.ReadLine()?.Replace(",",".");
                Console.WriteLine("Boylam koordinatın (örn: 26.65335):");
                var longi = Console.ReadLine()?.Replace(",", ".");
                Console.WriteLine("Bit konumları yükleniyor..");
                try
                {
                    flocks = await VodafoneHelper.GetFlocksAsync(lati, longi);
                    Console.WriteLine("---Bit konumları yüklendi---");
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Bir hata oluştu, bu bilgileriniz yanlış olmasından kaynaklı olabilir. Olmayadabilir.");
                }
            }


            while (true)
            {
                Console.WriteLine("Kaç puan yüklensin:");
                var input = Console.ReadLine();
                var randomFlock = flocks.OrderBy(r => new Guid()).FirstOrDefault();
                try
                {
                    var result = await VodafoneHelper.AddNewPointAsync(randomFlock.flockId, Convert.ToInt32(input));
                    Console.WriteLine(result.bitsHunted + " puan yüklendi.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Bir hata oluştu, bu bilgileriniz yanlış olmasından kaynaklı olabilir. Olmayadabilir.");
                }
            }
        }
    }
}
