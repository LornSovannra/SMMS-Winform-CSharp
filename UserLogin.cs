using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMMS
{
    static class UserLogin
    {
        //Static Fields
        private static string UserID;
        private static string UserName;
        private static string UserPassword;
        private static string UserType;
        private static byte UserPhoto;

        //Setter Methods
        public static void setUserID(string id)
        {
            UserID = id;
        }

        public static void setUserName(string name)
        {
            UserName = name;
        }

        public static void setPassword(string password)
        {
            UserPassword = password;
        }

        public static void setUserType(string type)
        {
            UserType = type;
        }

        public static void setUserPhoto(byte image)
        {
            UserPhoto = image;
        }

        //Getter Methods
        public static string getUserID()
        {
            return UserID;
        }
        public static string getUserName()
        {
            return UserName;
        }
        public static string getUserPassword()
        {
            return UserPassword;
        }
        public static string getUserType()
        {
            return UserType;
        }

        public static byte getUserPhoto()
        {
            return UserPhoto;
        }
    }
}
