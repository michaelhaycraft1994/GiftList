using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftList.Data;
using GiftList.DTO;

namespace GiftList.Service
{
    public class LoginService
    {
        public bool testCredentials(LoginDTO credentials)
        {

            if (credentials.username != "" && credentials.password != "")
            {
                try
                {
                    using (SearchDAL db = new SearchDAL(credentials.username, credentials.password))
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }
    }
}
