using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FindMyBuddyWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPetService" in both code and config file together.
    [ServiceContract]
    public interface IPetService
    {
        [OperationContract]
        [WebInvoke(
            Method = "POST", 
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "NewPet")]
        WCFResponse<string> NewPet(Pet pet);

        [OperationContract]
        [WebInvoke(Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "DeletePet/{id}")]
        WCFResponse<string> DeletePet(string id);

        [OperationContract]
        [WebGet(
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "FindPet/{id}")]
        WCFResponse<Pet> FindPet(string id);//SHOULD BE INT BUT WCF DOES NOT ALLOW URL PARAMS TO BE ANYTHING OTHER THAN STRING

        [OperationContract]
        [WebGet(
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "FindAllPets")]
        WCFResponse<List<Pet>> FindAllPets();
    }

    [DataContract]
    public class WCFResponse<T>
    {
        [DataMember]
        public T Response { get; set; }
        [DataMember]
        public int ResponseCode { get; set; }
    }

    [DataContract]
    public class Pet
    {
        [DataMember(Order = 1)]public int Id { get; set; }
        [DataMember(Order = 2)]public string Name { get; set; }
        [DataMember(Order = 3)]public int Age { get; set; }
        [DataMember(Order = 4)]public string Description { get; set; }
        [DataMember(Order = 5)]public string ImagePath { get; set; }
        [DataMember(Order = 6)]public double LostLat { get; set; }
        [DataMember(Order = 7)]public double LostLon { get; set; }
        [DataMember(Order = 8)]public string Race { get; set; }
        [DataMember(Order = 9)]public string Size { get;    set; }
        //WE MARK ISREQUIERED TO PREVENT EXCEPTIONS WHEN DESERIALIZING THE REQUEST BODY AND EMITDEFAULTVALUE TO NOT RETURN THE PROPERTY IN THE RESPONSE BODY
        //SO THESE CAN ONLY BE RECEIEVED, BUT THEY ARE OPTIONAL AND ALSO THEY ARE NOT SENT WHEN NO VALUE IS ASSIGNED TO THEM
        [DataMember(IsRequired = false, EmitDefaultValue = false)]public string ImageBase64 { get; set; }
        [DataMember(IsRequired = false, EmitDefaultValue = false)]public string ImageExtension { get; set; }
    }
}
