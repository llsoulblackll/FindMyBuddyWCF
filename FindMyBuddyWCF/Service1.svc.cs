using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FindMyBuddyWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public Service1()
        {
            //CONSOLE WRITELINE DOES NOT WORK
            //Debug.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string GetDirectory(string name)
        {
            //..\ need to be appended at the end of the path for it to take effect, it works implicitely for realative paths since it is applied to the current base path
            //PATH.GETFULLPATH IS NEEDED
            return Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..", name));
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
