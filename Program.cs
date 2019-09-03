using System;
using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace ldapPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            var userDN = @"user.name@my.domain";
            var userPasswd = "password@161245";

            var ldapHost = "127.0.0.1";
            var ldapPort = 389;

            // Creating an LdapConnection instance 
            LdapConnection ldapConn = new LdapConnection();

            //Connect function will create a socket connection to the server
            ldapConn.Connect(ldapHost, ldapPort);

            //Bind function will bind the user object Credentials to the Server
            ldapConn.Bind(userDN, userPasswd);

            // Searches in the Marketing container and return all child entries just below this container i.e.Single level search
            var lsc = ldapConn.Search("dc=my,dc=domain", LdapConnection.SCOPE_SUB, "(&(objectClass=user)(sAMAccountName=user.name))", null, false);

            while (lsc.hasMore())
            {
                LdapEntry nextEntry = null;
                try
                {
                    nextEntry = lsc.next();
                }
                catch (LdapException e)
                {
                    Console.WriteLine("Error: " + e.LdapErrorMessage);
                    //Exception is thrown, go for next entry
                    continue;
                }

                Console.WriteLine("\n" + nextEntry.DN);

                // Get the attribute set of the entry
                LdapAttributeSet attributeSet = nextEntry.getAttributeSet();
                System.Collections.IEnumerator ienum = attributeSet.GetEnumerator();

                // Parse through the attribute set to get the attributes and the corresponding values
                while (ienum.MoveNext())
                {
                    LdapAttribute attribute = (LdapAttribute)ienum.Current;
                    string attributeName = attribute.Name;
                    string attributeVal = attribute.StringValue;
                    Console.WriteLine(attributeName + "value:" + attributeVal);
                }
            }

            //While all the entries are parsed, disconnect   
            ldapConn.Disconnect();
        }
    }
}