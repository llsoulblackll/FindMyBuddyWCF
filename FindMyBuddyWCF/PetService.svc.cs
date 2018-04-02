using FindMyBuddyWCF.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace FindMyBuddyWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PetService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PetService.svc or PetService.svc.cs at the Solution Explorer and start debugging.
    public class PetService : IPetService
    {
        private PetDao petDao;

        public PetService()
        {
            this.petDao = new PetDao();
        }

        public WCFResponse<string> NewPet(Pet pet)
        {
            //bool result = File.Exists(savePath) ? petDao.Insert(pet) : false;
            int genId = petDao.Insert(pet);
            string savePath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Uploads\\{genId}_{pet.Name}_{DateTime.Now.Ticks}.{pet.ImageExtension}"));
            File.WriteAllBytes(savePath, Convert.FromBase64String(pet.ImageBase64));
            pet.Id = genId;
            pet.ImagePath = savePath;

            bool result;
            if (File.Exists(savePath))
                result = petDao.Update(pet);
            else
            {
                petDao.Delete(pet.Id);
                result = false;
            }

            //string message = "";
            return new WCFResponse<string>
            {
                Response = result.ToString(),
                ResponseCode = 1
            };
        }

        
        public WCFResponse<string> DeletePet(string id)
        {
            return new WCFResponse<string>
            {
                Response = petDao.Delete(int.Parse(id)).ToString(),
                ResponseCode = 1
            };
        }

        public WCFResponse<Pet> FindPet(string id)
        {
            return new WCFResponse<Pet>
            {
                Response = petDao.Find(int.Parse(id)),
                ResponseCode = 1
            };
        }

        public WCFResponse<List<Pet>> FindAllPets()
        {
            //Debug.WriteLine(HostingEnvironment.MapPath("/img"));
            /*StreamWriter sw = new StreamWriter("new.txt");
            sw.Write("Hello server path");
            sw.Flush();
            sw.Close();*/
            //HttpContext.Current.Request.Headers.Add("Access-Control-Allow-Origin", "*");
            return new WCFResponse<List<Pet>>
            {
                Response = petDao.FindAll(),/*new List<Pet>
                {
                    new Pet{Id = 1, Name = "Doggy"},
                    new Pet{Id = 2, Name = "Doggo"},
                    new Pet{Id = 3, Name = "Dog"},
                    new Pet{Id = 4, Name = "Dogster"},
                    new Pet{Id = 5, Name = "Dogger"},
                    new Pet{Id = 6, Name = "Doga"},
                },*/
                ResponseCode = 1
            };
        }   
    }
}
