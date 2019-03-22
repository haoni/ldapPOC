using System;
using System.Collections.Generic;
using Novell.Directory.Ldap;

namespace ldapPOC
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            var tentativas = new List<Credentials>();

            var userdn = "some.user";
            var userpassword = "psw@123";

            tentativas.Add(new Credentials("127.0.0.1", "ldap://localhost", 389, userdn, userpassword));
            tentativas.Add(new Credentials("192.168.22.173", "ldap://domain.server", 389, userdn, userpassword));

            foreach(var tentativa in tentativas)
            {
                try {
                    using (var cn = new LdapConnection())
                    {
                        // connect
                        cn.Connect(tentativa.IP, 389);

                        //cn.Connect(tentativa.HostName, 389);

                        // bind with an username and password
                        // this how you can verify the password of an user
                        cn.Bind(tentativa.UserDn, tentativa.UserPassword);

                        // call ldap op
                        // cn.Delete("<<userdn>>")
                        // cn.Add(<<ldapEntryInstance>>)
                    }
                }
                catch(Exception ex) {
                    
                    Console.WriteLine($"Erro: {tentativa.HostName}:{tentativa.Port} ({tentativa.IP})");
                    Console.WriteLine($"Usuário: {tentativa.UserDn} Senha: {tentativa.UserPassword}");
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(Environment.NewLine);

                }
            }
        }
    }

    public class Credentials {

        public Credentials(string ip, string hostname, int port, string userdn, string userpassword)
        {
            this.IP = ip;
            this.HostName = hostname;
            this.Port = port;
            this.UserDn = userdn;
            this.UserPassword = userpassword;
        }

        public string IP { get; private set; }
        public string HostName { get; private set; }
        public int Port { get;  private set; }
        public string UserDn { get; private set; }
        public string UserPassword { get;  private set; }
    }
}