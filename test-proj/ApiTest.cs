using ApiClients;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

using SearchScope = System.DirectoryServices.Protocols.SearchScope;

namespace test_proj
{
    
    internal class ApiTest
    {
        // LDAP地址 例如：LDAP://my.com.cn
        private const string LDAP_HOST = "LDAP://auth.y-theta.cn";
        // 具有LDAP管理权限的特殊帐号
        private const string USER_NAME = "account";
        // 具有LDAP管理权限的特殊帐号的密码
        private const string PASSWORD = "password";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestFirefly1()
        {
            Program.Main();
        }

        [Test]
        public void TestLdap()
        {
            LdapDirectoryIdentifier serverId = new LdapDirectoryIdentifier("auth.y-theta.cn", 39000);
            NetworkCredential credentials = new NetworkCredential("uid=admin,ou=people,dc=y-theta,dc=cn", "20154530");

            LdapConnection conn = new LdapConnection(serverId, credentials);
            conn.AuthType = AuthType.Basic;
            conn.SessionOptions.ProtocolVersion = 3;
            conn.Bind();
            var request = new SearchRequest("cn=gitea,dc=y-theta,dc=cn", null, SearchScope.Subtree, null);
            var response = conn.SendRequest(request) as SearchResponse;
            foreach (SearchResultEntry rep in response.Entries)
            {
                Debug.WriteLine(rep.DistinguishedName);
                foreach (string item in rep.Attributes.AttributeNames)
                {
                    Debug.WriteLine($" {item}");
                    var vals = rep.Attributes[item];
                    foreach (var val in vals)
                    {
                        if (val is byte[] items)
                        {
                            Debug.WriteLine($"  {Encoding.UTF8.GetString(items)}");
                        }
                    }
                }
            }
            //LdapDirectoryIdentifier ldi = new LdapDirectoryIdentifier("103.224.80.35", 39000);
            //System.DirectoryServices.Protocols.LdapConnection ldapConnection =
            //     new System.DirectoryServices.Protocols.LdapConnection(ldi);
            //Debug.WriteLine("LdapConnection is created successfully.");
            //ldapConnection.AuthType = AuthType.Basic;
            //ldapConnection.SessionOptions.ProtocolVersion = 3;
            //NetworkCredential nc = new NetworkCredential("uid=test-user,dc=ytheta,dc=cn", "20154530"); //password
            //ldapConnection.Bind(nc);
            //Debug.WriteLine("LdapConnection authentication success");
        }
    }
}
